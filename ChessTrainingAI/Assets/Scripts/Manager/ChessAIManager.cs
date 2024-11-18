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

    #region Q 러닝 관련 데이터
    const float GAMMA = 0.5f;
    int nowStateCount = 0;
    #endregion

    #region Tables
    float[,,,] Q_Table = new float[60,6,64,64];     // [turn, 기물 타입, 시작 타일, 도착 타일] 
    
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

        // moveNum이 음수면 문제가 생긴것으로 강제 return
        if (moveNum < 0)
            return;
        
        int startPosIndex = moveNum / 64;
        int endPosIndex = moveNum % 64;

        Vector2Int startPos = new Vector2Int(startPosIndex % 8, startPosIndex / 8);
        Vector2Int endPos = new Vector2Int(endPosIndex % 8, endPosIndex / 8);
        //Debug.Log(moveNum + "\n" + startPos + "\n" + ChessManager.instance.chessTileList[startPos.x, startPos.y].locatedPiece.pieceType);
        
        // 기물 이동

        ChessManager.instance.nowPiece = ChessManager.instance.chessTileList[startPos.x, startPos.y].locatedPiece;
        ChessManager.instance.chessTileList[startPos.x, startPos.y].locatedPiece.
            Move(ChessManager.instance.chessTileList[endPos.x, endPos.y]);
    }


    void Q_Learning()
    {
        // Q(state, action) = R(state, reward) + gamma * Max(Q(next state, all actions))
        // 최적의 수 = action의 currentQIndex이고 해당 값의 예측 리워드는 QReward이다
        List<int> currentQIndex = new List<int>();
        float currentQReward = 0;

        for (int i = 0; i < state.nowAction.nowActionArr.Length; i++)
        {
            // 기물이 이동할 수 없음 or 흰색 기물이면 pass
            if (state.nowAction.nowActionArr[i / 64, i % 64] >= 0)
                continue;
            
            // 1. 현재 i번째 리워드 값을 구한다.
            int targetTileNum = i % 64;      // 현재 기물 도착 지점의 값
            
            float nowReward = Reward(targetTileNum);

            // 2. 다음 turn에서 가장 효율적인 이동을 고른다.
            SetNextState();
            float maxQReward = MaxAction();

            // 3. 현재 보상값 = R(state, reward) + gamma * Max(Q(next state, all actions))
            nowReward += GAMMA * maxQReward;

            // 4. 현재 보상이 이전 보상보다 크다면 추가
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

        // Q_Table에 업데이트
    }

    // 선택된 Q 인덱스 중에서 랜덤으로 뽑기
    void SearchBestMove(List<int> getQIndex)
    {
        if(getQIndex.Count == 0)
        {
            // 아무것도 이득이 없는 행위라는 것이므로 그냥 아무거나 행동으로 설정
        }
        // 랜덤 값 설정
        int selectedNum = UnityEngine.Random.Range(0, getQIndex.Count - 1);
        
        moveNum = getQIndex[selectedNum];
        getQIndex.RemoveAt(selectedNum);

        // moveNum을 통한 랜덤 값 확인
        int startPosIndex = moveNum / 64;
        int endPosIndex = moveNum % 64;

        Vector2Int startPos = new Vector2Int(startPosIndex % 8, startPosIndex / 8);
        Vector2Int endPos = new Vector2Int(endPosIndex % 8, endPosIndex / 8);

        if (ChessManager.instance.chessTileList[startPos.x, startPos.y].locatedPiece == null)
            SearchBestMove(getQIndex);

    }

    #region Table 초기화 및 재설정
    public void SetState()
    {
        state.SetNowState();
    }
    #endregion

    #region 디버그 함수

    void DebugPositions(int i)
    {
        int startPosNum = i / 64;
        int endPosNum = i % 64;

        Vector2 startPos = new Vector2(startPosNum % 8, startPosNum / 8);
        Vector2 endPos = new Vector2(endPosNum % 8, endPosNum / 8);

        Debug.Log("현재 시작 지점 : " + startPos + "\n도착 지점 : " + endPos);
    }
    #endregion

    #region 계산 함수
    float Reward(int actionY)
    {
        // 1. 해당 액션의 시작점과 종착역을 확인한다.
        Vector2Int endPos = new Vector2Int(actionY / 8, actionY % 8);
        //Debug.Log("현재 들어온 값 :" + actionY + "\n" + endPos + "\n" + "현재 state :" + state[endPos.y, endPos.x]);
        // 2. 종착역의 기물 점수를 반환한다.
        return state.nowState[endPos.y, endPos.x];
    }

    /// <summary>
    /// 다음 turn에서 가장 좋은 수를 찾습니다.
    /// </summary>
    float MaxAction()
    {
        // 다음 턴 최대 reward를 가져오는 알고리즘
        // 1. 다음 state 리스트를 생성한다.
        // 2. 해당 state에서 모든 action을 가져온다.
        // 3. 해당 state별 최대 reward를 가져온다.
        // 4. state 리스트에서 최대 reward를 return한다
        return 0;
    } 
    
    // 다음 수를 예측할 떄 먼저 state 리스트를 설정할 수 있도록 구현
    void SetNextState()
    {
        if(state.nowAction.availableActionList.Count == 0)
        {
            Debug.Log("가능한 액션이 존재하지 않음");
            return;
        }
        TestManager.Instance.ConvertState2TestTileBoard(state);
        //State nextTempStage = new State();

        //// 1. 현재 State에서 가능한 action을 가져온다.
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