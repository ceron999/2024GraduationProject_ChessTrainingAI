using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.ComponentModel;

public class ChessAIManager : MonoBehaviour
{
    #region Singleton
    public static ChessAIManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    #region Q ���� ���� ������
    const float GAMMA = 0.5f;
    #endregion

    #region Tables
    public State state = new State();
    public List<State> nextStateList = new List<State>();
    #endregion

    int moveNum = -1;

    private void Start()
    {
        
    }
    
     public void MovePiece()
    {
        //Q_Learning();
        CalculateReward();

        // moveNum�� ������ ������ ��������� ���� return
        if (moveNum < 0)
            return;
        
        int startPosIndex = moveNum / 64;
        int endPosIndex = moveNum % 64;

        Vector2Int startPos = new Vector2Int(startPosIndex % 8, startPosIndex / 8);
        Vector2Int endPos = new Vector2Int(endPosIndex % 8, endPosIndex / 8);
        
        // �⹰ �̵�
        ChessManager.instance.nowPiece = ChessManager.instance.chessTileList[startPos.x, startPos.y].locatedPiece;
        ChessManager.instance.chessTileList[startPos.x, startPos.y].locatedPiece.
            Move(ChessManager.instance.chessTileList[endPos.x, endPos.y]);
    }


    void Q_Learning()
    {
        // Q(state, action) = R(state, reward) + gamma * Max(Q(next state, all actions))
        // ������ �� = action�� currentQIndex�̰� �ش� ���� ���� ������� QReward�̴�
        List<int> currentQIndex = new List<int>();
        float currentQReward = 0;

        // ���� State ����Ʈ�� �̸� �����Ѵ�. 
        SetNextStateList();

        for (int i = 0; i < state.nowAction.nowActionArr.Length; i++)
        {
            // �⹰�� �̵��� �� ���� or ��� �⹰�̸� pass
            if (state.nowAction.nowActionArr[i / 64, i % 64] >= 0)
                continue;
            
            // 1. ���� i��° ������ ���� ���Ѵ�.
            int targetTileNum = i % 64;      // ���� �⹰ ���� ������ ��
            
            float nowReward = Reward(targetTileNum);

            // 2. ���� turn���� ���� ȿ������ �̵��� ����.
            State minRewardState = MinAction();

            // 3. ���� ���� = R(state, reward) + gamma * Max(Q(next state, all actions))
            nowReward += GAMMA * minRewardState.nowAction.minActionReward;

            // 4. ���� ������ ���� ���󺸴� ũ�ٸ� �߰�
            if (nowReward > currentQReward)
            {
                currentQIndex.Clear();
                currentQIndex.Add(i);
                
                currentQReward = nowReward;
            }
            else if(nowReward == currentQReward)
            {
                currentQIndex.Add(i);
            }
        }
        SearchBestMove(currentQIndex);

        // Q_Table�� ������Ʈ
    }

    void CalculateReward()
    {
        // ���� State ����Ʈ�� �̸� �����Ѵ�. 
        SetNextStateList();

        // 1. �ִ� reward�� ���� State ����
        List<State> maxRewardState = new List<State>();
        maxRewardState.Add(nextStateList[0]);
        float maxReward = Reward(nextStateList[0].lastAction.y) + nextStateList[0].nowAction.minActionReward;

        // 2. next state�� ��ȸ�ϸ� �ִ� reward�� ������ State�� ã��
        for (int i = 1; i < nextStateList.Count; i++) 
        {
            float nowReward = Reward(nextStateList[i].lastAction.y);

            float nextMinReward = nextStateList[i].nowAction.minActionReward;

            nowReward += GAMMA * nextMinReward;

            if (nowReward > maxReward)
            {
                maxRewardState.Clear();
                maxRewardState.Add(nextStateList[i]);
                Debug.Log("maxReward : " + maxReward);
                maxReward = nowReward;
            }
            else
            {
                maxRewardState.Add(nextStateList[i]);
            }
        }
        SearchBestMove(maxRewardState);
    }

    // ���õ� Q �ε��� �߿��� �������� �̱�
    void SearchBestMove(List<int> getQIndex)
    {
        if(getQIndex.Count == 0)
        {
            // �ƹ��͵� �̵��� ���� ������� ���̹Ƿ� �׳� �ƹ��ų� �ൿ���� ����
        }
        // ���� �� ����
        int selectedNum = UnityEngine.Random.Range(0, getQIndex.Count - 1);
        
        moveNum = getQIndex[selectedNum];
        getQIndex.RemoveAt(selectedNum);

        // moveNum�� ���� ���� �� Ȯ��
        int startPosIndex = moveNum / 64;
        int endPosIndex = moveNum % 64;

        Vector2Int startPos = new Vector2Int(startPosIndex % 8, startPosIndex / 8);
        Vector2Int endPos = new Vector2Int(endPosIndex % 8, endPosIndex / 8);

        if (ChessManager.instance.chessTileList[startPos.x, startPos.y].locatedPiece == null)
            SearchBestMove(getQIndex);

    }

    void SearchBestMove(List<State> getStateList)
    {
        if (getStateList.Count == 0)
        {
            // ���� state�� �ƹ��͵� ���ٴ� ������ ����
            Debug.Log("SearchBestMove���� �ּ� reward�� ���� state�� 0����");
            return;
        }

        // �������� State ����
        int selectedNum = UnityEngine.Random.Range(0, getStateList.Count - 1);

        State selectedState = getStateList[selectedNum];
        getStateList.RemoveAt(selectedNum);

        // ���õ� State�� lastAction��� �ൿ
        int startPosIndex = selectedState.lastAction.x;
        int endPosIndex = selectedState.lastAction.y;

        Vector2Int startPos = new Vector2Int(startPosIndex % 8, startPosIndex / 8);
        Vector2Int endPos = new Vector2Int(endPosIndex % 8, endPosIndex / 8);

        if (ChessManager.instance.chessTileList[startPos.x, startPos.y].locatedPiece == null)
            SearchBestMove(getStateList);

        moveNum = startPosIndex * 64 + endPosIndex;
    }

    #region Table �ʱ�ȭ �� �缳��
    public void SetState()
    {
        state.SetNowState();
    }
    #endregion

    #region ����� �Լ�

    void DebugPositions(int i)
    {
        int startPosNum = i / 64;
        int endPosNum = i % 64;

        Vector2 startPos = new Vector2(startPosNum % 8, startPosNum / 8);
        Vector2 endPos = new Vector2(endPosNum % 8, endPosNum / 8);

        Debug.Log("���� ���� ���� : " + startPos + "\n���� ���� : " + endPos);
    }

    void DebugStartPosNEndPos(Vector2Int getPositions)
    {
        Vector2 startPos = new Vector2(getPositions.x % 8, getPositions.x / 8);
        Vector2 endPos = new Vector2(getPositions.y % 8, getPositions.y / 8);

        Debug.Log("���� ���� ���� : " + startPos + "\n���� ���� : " + endPos);
    }
    #endregion

    #region ��� �Լ�
    float Reward(int actionY)
    {
        // 1. �ش� �׼��� �������� �������� Ȯ���Ѵ�.
        Vector2Int endPos = new Vector2Int(actionY / 8, actionY % 8);
        //Debug.Log("���� ���� �� :" + actionY + "\n" + endPos + "\n" + "���� state :" + state[endPos.y, endPos.x]);
        // 2. �������� �⹰ ������ ��ȯ�Ѵ�.
        return state.nowState[endPos.y, endPos.x];
    }

    /// <summary>
    /// ���� turn���� ���� ���� ���� ã���ϴ�.
    /// </summary>
    State MinAction()
    {
        // ���� �� �ִ� reward�� �������� �˰���
        // 1. �ش� state�� �ּ� reward�� �����´�.
        List<State> smallestStateList = new List<State> ();
        smallestStateList.Add(nextStateList[0]);

        for (int i = 0; i < nextStateList.Count; i++)
        {
            if (nextStateList[i].minRewardActionList.Count == 0)
                continue;

            State currentState = nextStateList[i];

            // ���� ���� Reward�� ������ State�� 1�� �̻��� ��츦 ���� ����Ʈ�� ����
            if (nextStateList[i].nowAction.minActionReward < smallestStateList[0].nowAction.minActionReward)
            {
                smallestStateList.Clear();
                smallestStateList.Add(currentState);
            }
            else if(nextStateList[i].nowAction.minActionReward == smallestStateList[0].nowAction.minActionReward)
                smallestStateList.Add(currentState);

        }

        // �ּ� reward�� ������ State�� 1������ ũ�ٸ� �������� �ϳ� ����
        if (smallestStateList.Count > 1)
        {
            int num = UnityEngine.Random.Range(0, smallestStateList.Count);
            return smallestStateList[num];
        }

        // 4. state ����Ʈ���� �ִ� reward�� ������ state�� return�Ѵ�
        return smallestStateList[0];
    } 
    
    // ���� ���� ������ �� ���� state ����Ʈ�� ������ �� �ֵ��� ����
    void SetNextStateList()
    {
        // ���� ������ �ൿ�� ���ٸ� return
        if(state.nowAction.availableActionList.Count == 0)
        {
            Debug.Log("������ �׼��� �������� ����");
            return;
        }

        // 1. �ʱ�ȭ
        nextStateList.Clear();

        // 2. ���� State�� ����
        for (int i = 0; i < state.nowAction.availableActionList.Count; i++)
        {
            // ���� ���� action�̶�� �н�
            if (state.IsPieceColorWhite(state.nowAction.availableActionList[i].x))
                continue;

            // 1. ���� State�� ����
            State nextTempState = new State();
            nextTempState.DeepCopyState(state);

            // action��� state ��ȯ
            nextTempState.UpdateState(state.nowAction.availableActionList[i]);
            nextTempState.lastAction = state.nowAction.availableActionList[i];
            nextStateList.Add(nextTempState);
        }
    }

    #endregion
}