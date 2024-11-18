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
    int nowStateCount = 0;
    #endregion

    #region Tables
    float[,,,] Q_Table = new float[60,6,64,64];     // [turn, �⹰ Ÿ��, ���� Ÿ��, ���� Ÿ��] 
    
    public State state = new State();
    List<State> nextStateList = new List<State>();
    List<Action> nextActionList = new List<Action>();
    #endregion

    int moveNum = -1;

    private void Start()
    {
        
    }
    
     public void MovePiece()
    {
        Q_Learning();

        // moveNum�� ������ ������ ��������� ���� return
        if (moveNum < 0)
            return;
        
        int startPosIndex = moveNum / 64;
        int endPosIndex = moveNum % 64;

        Vector2Int startPos = new Vector2Int(startPosIndex % 8, startPosIndex / 8);
        Vector2Int endPos = new Vector2Int(endPosIndex % 8, endPosIndex / 8);
        //Debug.Log(moveNum + "\n" + startPos + "\n" + ChessManager.instance.chessTileList[startPos.x, startPos.y].locatedPiece.pieceType);
        
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

        for (int i = 0; i < state.nowAction.nowActionArr.Length; i++)
        {
            // �⹰�� �̵��� �� ���� or ��� �⹰�̸� pass
            if (state.nowAction.nowActionArr[i / 64, i % 64] >= 0)
                continue;
            
            // 1. ���� i��° ������ ���� ���Ѵ�.
            int targetTileNum = i % 64;      // ���� �⹰ ���� ������ ��
            
            float nowReward = Reward(targetTileNum);

            // 2. ���� turn���� ���� ȿ������ �̵��� ����.
            SetNextState();
            float maxQReward = MaxAction();

            // 3. ���� ���� = R(state, reward) + gamma * Max(Q(next state, all actions))
            nowReward += GAMMA * maxQReward;

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
    float MaxAction()
    {
        // ���� �� �ִ� reward�� �������� �˰���
        // 1. ���� state ����Ʈ�� �����Ѵ�.
        // 2. �ش� state���� ��� action�� �����´�.
        // 3. �ش� state�� �ִ� reward�� �����´�.
        // 4. state ����Ʈ���� �ִ� reward�� return�Ѵ�
        return 0;
    } 
    
    // ���� ���� ������ �� ���� state ����Ʈ�� ������ �� �ֵ��� ����
    void SetNextState()
    {
        if(state.nowAction.availableActionList.Count == 0)
        {
            Debug.Log("������ �׼��� �������� ����");
            return;
        }
        TestManager.Instance.ConvertState2TestTileBoard(state);
        //State nextTempStage = new State();

        //// 1. ���� State���� ������ action�� �����´�.
        //for (int i = 0; i < state.nowAction.availableActionList.Count; i++)
        //{
        //    int startPosNum = state.nowAction.availableActionList[i].x; 
        //    int endPosNum = state.nowAction.availableActionList[i].y;
        //    Vector2Int startPos = new Vector2Int(startPosNum % 8, startPosNum / 8);
        //    Vector2Int endPos = new Vector2Int(endPosNum % 8, endPosNum / 8);

        //    nextTempStage.UpdateState(startPos, endPos);

        //    nextStateList.Add(nextTempStage);
        //}
    }

    #endregion
}