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
    
    float[,] state = new float[8, 8];               // [ col, row]
    float[,] action = new float[64, 64];            // [���� Ÿ��, ���� Ÿ��]
    #endregion

    int moveNum = -1;

    private void Start()
    {
        SetAllTables();
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

    public void AvoidCheck()
    {
        // üũ ���ϱ� �Լ�
    }


    void Q_Learning()
    {
        // Q(state, action) = R(state, reward) + gamma * Max(Q(next state, all actions))
        // ������ �� = action�� currentQIndex�̰� �ش� ���� ���� ������� QReward�̴�
        List<int> currentQIndex = new List<int>();
        float currentQReward = 0;

        for (int i = 0; i < action.Length; i++)
        {
            // �⹰�� �̵��� �� ���� or ��� �⹰�̸� pass
            if (action[i / 64, i % 64] >= 0)
                continue;
            
            // 1. ���� i��° ������ ���� ���Ѵ�.
            int targetTileNum = i % 64;      // ���� �⹰ ���� ������ ��
            
            float nowReward = Reward(targetTileNum);

            // 2. ���� turn���� ���� ȿ������ �̵��� ����.
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
        // ������ ������ �޾Ƽ� �����Ѵ�.
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
                //���⼭ ���̺� ���� ������ �߰��Ѵ�
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

        // �⹰ ����
        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
            {
                if(getTiles[x, y].locatedPiece != null)
                    nowPieces.Add(getTiles[x, y].locatedPiece);
            }

        // ��� �⹰ Ȯ��
        for (int i = 0; i < nowPieces.Count; i++)
        {
            // �⹰�� �̵� ������ Ÿ���� ��� action�� ����
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

    #region ����� �Լ�
    public void DebugQ_TableArr()
    {
        StringBuilder arr = new StringBuilder();
        arr.Append("Q_Table �����\n\n");

        for (int x = 0; x < 10; x++)
        {
            arr.Append("���� �� ��: " + x + "\n");
            for (int y = 0; y < 6; y++)
            {
                arr.Append("���� �⹰ : ");
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
        arr.Append("Q_Table �����\n\n");

        for (int x = 0; x < 10; x++)
        {
            arr.Append("���� �� ��: " + x + "\n");
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
        Debug.Log("����");
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
        Debug.Log("�׼�");
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
        return state[endPos.y, endPos.x];
    }

    /// <summary>
    /// ���� turn���� ���� ���� ���� ã���ϴ�.
    /// </summary>
    float MaxAction()
    {
        // 0. ������ ����(�ִ� Q���� ã�� ���� ������
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
                    // 1. Q_table�� ���� ���� �ִ񰪺��� ������?
                    if (Q_Table[nowStateCount +1, pieceType, q_Start, q_End] > maxQ_Value)
                    {
                        // 2. Q_Table�� �ִ� �ִ��� ���� action�� �־� ��� �����Ѱ�?
                        if (IsQAvailable(pieceType, q_Start, q_End))
                        {
                            // 3. �����ϴٸ� ����
                            maxQPieceType = pieceType;
                            maxQ_Start = q_Start;
                            maxQ_End = q_End;
                        }
                    }
                }
            }
        }

        //�ִ� Q �� Reward�� ��ȯ
        if (maxQPieceType == -1)
            return 0;
        else
            return 0;
    }

    //��������@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
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