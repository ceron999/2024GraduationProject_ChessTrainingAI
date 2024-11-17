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
    
    float[,] state = new float[8, 8];               // [ col, row]
    float[,] action = new float[64, 64];            // [시작 타일, 도착 타일]
    #endregion

    int moveNum = -1;

    private void Start()
    {
        SetAllTables();
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

    public void AvoidCheck()
    {
        // 체크 피하기 함수
    }


    void Q_Learning()
    {
        // Q(state, action) = R(state, reward) + gamma * Max(Q(next state, all actions))
        // 최적의 수 = action의 currentQIndex이고 해당 값의 예측 리워드는 QReward이다
        List<int> currentQIndex = new List<int>();
        float currentQReward = 0;

        for (int i = 0; i < action.Length; i++)
        {
            // 기물이 이동할 수 없음 or 흰색 기물이면 pass
            if (action[i / 64, i % 64] >= 0)
                continue;
            
            // 1. 현재 i번째 리워드 값을 구한다.
            int targetTileNum = i % 64;      // 현재 기물 도착 지점의 값
            
            float nowReward = Reward(targetTileNum);

            // 2. 다음 turn에서 가장 효율적인 이동을 고른다.
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
    void SetAllTables()
    {
        for (int x = 0; x < 60; x++)
            for (int y = 0; y < 6; y++)
                for (int z = 0; z < 64; z++)
                    for (int w = 0; w < 64; w++)
                        Q_Table[x, y, z, w] = 0;

        for (int y = 0; y < 8; y++)
            for (int z = 0; z < 8; z++)
                state[y, z] = 0;

        for (int x = 0; x < 5; x++)
            for (int y = 0; y < 8; y++)
                action[x, y] = 0;
    }

    public void LoadQ_Table()
    {
        // 데이터 파일을 받아서 저장한다.
    }

    public void UpdateQ_Table()
    {
        
        for(int x = 0; x < 64; x++)
        {
            for(int y = 0; y < 64; y++)
            {
                int nowPiece = 0;
                if(action[x, y] != 0)
                {
                    switch (MathF.Abs(action[x, y]))
                    {
                        case 1:
                            nowPiece = 0; break;
                        case 3:
                            nowPiece = 1; break;
                        case 3.5f:
                            nowPiece = 2; break;
                        case 5:
                            nowPiece = 3; break;
                        case 8:
                            nowPiece = 4; break;
                        case 10000:
                            nowPiece = 5; break;
                    }
                }
                //여기서 테이블 값에 보상값을 추가한다
                Q_Table[nowStateCount, nowPiece, x, y] += 0;
            }
        }
    }

    public void SetState()
    {
        Tile[,] getTiles = ChessManager.instance.chessTileList;
        
        for (int x = 0;x < 8; x++)
            for(int y = 0;y < 8; y++)
            {
                state[x, y] = 0;
                if (getTiles[x, y].locatedPiece != null)
                    state[x,y] = getTiles[x,y].locatedPiece.piecePoint;
            }
        //DebugState();
    }

    public void SetAction()
    {
        Tile[,] getTiles = ChessManager.instance.chessTileList;

        List<Piece> nowPieces = new List<Piece>();

        for (int x = 0; x < 64; x++)
            for (int y = 0; y < 64; y++)
            {
                action[x, y] = 0;
            }

        // 기물 정리
        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
            {
                if(getTiles[x, y].locatedPiece != null)
                    nowPieces.Add(getTiles[x, y].locatedPiece);
            }

        // 모든 기물 확인
        for (int i = 0; i < nowPieces.Count; i++)
        {
            // 기물의 이동 가능한 타일을 모두 action에 저장
            for (int movableTileCount = 0; movableTileCount < nowPieces[i].movableTIleList.Count; movableTileCount++)
            {
                int startPos = nowPieces[i].nowPos.y * 8 + nowPieces[i].nowPos.x;
                int targetPos = (int)nowPieces[i].movableTIleList[movableTileCount].tileName;

                action[startPos, targetPos] = nowPieces[i].piecePoint;
            }
        }

        DebugAction();
    }
    #endregion

    #region 디버그 함수
    public void DebugQ_TableArr()
    {
        StringBuilder arr = new StringBuilder();
        arr.Append("Q_Table 디버깅\n\n");

        for (int x = 0; x < 10; x++)
        {
            arr.Append("현재 턴 수: " + x + "\n");
            for (int y = 0; y < 6; y++)
            {
                arr.Append("현재 기물 : ");
                switch (y)
                {
                    case 0:
                        arr.Append("Pawn\n"); break;
                    case 1:
                        arr.Append("Knight\n"); break;
                    case 2:
                        arr.Append("Bishop\n"); break;
                    case 3:
                        arr.Append("Rook\n"); break;
                    case 4:
                        arr.Append("Queen\n"); break;
                    case 5:
                        arr.Append("King\n"); break;
                }
                for (int z = 0; z < 64; z++)
                {
                    for (int w = 0; w < 64; w++)
                    {
                        arr.Append(Q_Table[x,y, z, w]);
                        if (w < 63)
                        {
                            arr.Append(", ");
                        }
                        else
                        {
                            arr.Append("]\n");
                        }
                    }
                }
            }
        }

        Debug.Log(arr.ToString());
    }

    public void DebugQ_TableNotation()
    {
        StringBuilder arr = new StringBuilder();
        arr.Append("Q_Table 디버깅\n\n");

        for (int x = 0; x < 10; x++)
        {
            arr.Append("현재 턴 수: " + x + "\n");
            for (int y = 0; y < 6; y++)
            {
                for (int z = 0; z < 64; z++)
                {
                    for (int w = 0; w < 64; w++)
                    {
                        switch (MathF.Abs(Q_Table[x, y, z, w]))
                        {
                            case 1:
                                arr.Append("Pawn : ");
                                arr.Append((TIleName)Enum.Parse(typeof(TIleName), z.ToString()) + " -> "
                                        + (TIleName)Enum.Parse(typeof(TIleName), w.ToString()) + "\n");
                                break;
                            case 3:
                                arr.Append("Knight : ");
                                arr.Append((TIleName)Enum.Parse(typeof(TIleName), z.ToString()) + " -> "
                                        + (TIleName)Enum.Parse(typeof(TIleName), w.ToString()) + "\n");
                                break;
                            case 3.5f:
                                arr.Append("Bishop : ");
                                arr.Append((TIleName)Enum.Parse(typeof(TIleName), z.ToString()) + " -> "
                                        + (TIleName)Enum.Parse(typeof(TIleName), w.ToString()) + "\n");
                                break;
                            case 5:
                                arr.Append("Rook : ");
                                arr.Append((TIleName)Enum.Parse(typeof(TIleName), z.ToString()) + " -> "
                                        + (TIleName)Enum.Parse(typeof(TIleName), w.ToString()) + "\n");
                                break;
                            case 8:
                                arr.Append("Queen : ");
                                arr.Append((TIleName)Enum.Parse(typeof(TIleName), z.ToString()) + " -> "
                                        + (TIleName)Enum.Parse(typeof(TIleName), w.ToString()) + "\n");
                                break;
                            case 10000:
                                arr.Append("King : ");
                                arr.Append((TIleName)Enum.Parse(typeof(TIleName), z.ToString()) + " -> "
                                        + (TIleName)Enum.Parse(typeof(TIleName), w.ToString()) + "\n"); 
                                break;
                            default:
                                continue;
                        }

                        
                    }
                }
            }
        }

        Debug.Log(arr.ToString());
    }

    public void DebugState()
    {
        Debug.Log("상태");
        StringBuilder arr = new StringBuilder();
        for (int y = 7; y > 0; y--)
        {
            arr.Append("[");
            for (int z = 0; z < 8; z++)
            {
                arr.Append(state[z, y]);
                if (z < 7)
                {
                    arr.Append(", ");
                }
                else
                {
                    arr.Append("]\n");
                }
            }
        }

        Debug.Log(arr.ToString());
    }
    public void DebugAction()
    {
        Debug.Log("액션");
        StringBuilder arr = new StringBuilder();
        for (int y = 0; y < 64; y++)
        {
            for (int z = 0; z < 64; z++)
            {
                if(MathF.Abs(action[y, z]) > 0)
                {
                    if (action[y, z] > 0)
                        arr.Append("White ");
                    else if (action[y, z] < 0)
                        arr.Append("Black ");

                        switch (MathF.Abs(action[y, z]))
                    {
                        case 1:
                            arr.Append("Pawn : "); break;
                        case 3:
                            arr.Append("Knight : "); break;
                        case 3.5f:
                            arr.Append("Bishop : "); break;
                        case 5:
                            arr.Append("Rook : "); break;
                        case 8:
                            arr.Append("Queen : "); break;
                        case 10000:
                            arr.Append("King : "); break;
                    }


                    arr.Append((TIleName)Enum.Parse(typeof(TIleName), y.ToString())  + " -> " 
                        + (TIleName)Enum.Parse(typeof(TIleName), z.ToString()) + "\n");
                }
            }
        }
        Debug.Log(arr.ToString());
    }

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
        return state[endPos.y, endPos.x];
    }

    /// <summary>
    /// 다음 turn에서 가장 좋은 수를 찾습니다.
    /// </summary>
    float MaxAction()
    {
        // 0. 데이터 설정(최대 Q값을 찾기 위한 데이터
        int maxQPieceType = -1;
        int maxQ_Start = -1;
        int maxQ_End = -1;
        float maxQ_Value = Q_Table[nowStateCount + 1,0,0,0];
        
        for (int pieceType =0; pieceType < 6; pieceType++)
        {
            for(int q_Start =0; q_Start < 64;q_Start++)
            {
                for (int q_End = 0; q_End < 64; q_End++)
                {
                    // 1. Q_table의 현재 값이 최댓값보다 높은가?
                    if (Q_Table[nowStateCount +1, pieceType, q_Start, q_End] > maxQ_Value)
                    {
                        // 2. Q_Table에 있는 최댓값이 지금 action에 있어 사용 가능한가?
                        if (IsQAvailable(pieceType, q_Start, q_End))
                        {
                            // 3. 가능하다면 생성
                            maxQPieceType = pieceType;
                            maxQ_Start = q_Start;
                            maxQ_End = q_End;
                        }
                    }
                }
            }
        }

        //최대 Q 의 Reward값 반환
        if (maxQPieceType == -1)
            return 0;
        else
            return 0;
    }

    //문제있음@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    bool IsQAvailable(int getPieceType, int getStartPos, int getEndPos)
    {
        if(action[getStartPos,getEndPos] == 0)
            return false;
        else
        {
            switch (getPieceType)
            {
                case 0:
                    if (Mathf.Abs(action[getStartPos, getEndPos]) != 1)
                        return false;
                    break;
                case 1:
                    if (Mathf.Abs(action[getStartPos, getEndPos]) != 3)
                        return false;
                    break;
                case 2:
                    if (Mathf.Abs(action[getStartPos, getEndPos]) != 3.5)
                        return false;
                    break;
                case 3:
                    if (Mathf.Abs(action[getStartPos, getEndPos]) != 5)
                        return false;
                    break;
                case 4:
                    if (Mathf.Abs(action[getStartPos, getEndPos]) != 8)
                        return false;
                    break;
                case 5:
                    if (Mathf.Abs(action[getStartPos, getEndPos]) != 10000)
                        return false;
                    break;
            }
        }

        return true;
    }
    #endregion
}