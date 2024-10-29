using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

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
    float gamma = 0.5f;
    int nowStateCount = 0;
    #endregion

    #region Tables
    float[,,,] Q_Table = new float[60,6,64,64];   // [���� ������ state ����,�⹰ Ÿ��, ���� Ÿ��, ���� Ÿ��] 
    float[,,] state = new float[6, 8, 8];       // [�⹰ Ÿ��, col, row]
    float[,] action = new float[64, 64];        // [���� Ÿ��, ���� Ÿ��]
    #endregion

    private void Start()
    {
        SetAllTables();
    }

    void Q_Learning()
    {
        //Q(state, action) = R(state, reward) + gamma * Max(Q(next state, all actions))
    }


    #region Table �ʱ�ȭ �� �缳��
    void SetAllTables()
    {
        for (int x = 0; x < 60; x++)
            for (int y = 0; y < 6; y++)
                for (int z = 0; z < 64; z++)
                    for (int w = 0; w < 64; w++)
                        Q_Table[x, y, z, w] = 0;

        for (int x = 0; x < 6; x++)
            for (int y = 0; y < 8; y++)
                for (int z = 0; z < 8; z++)
                    state[x, y, z] = 0;

        for (int x = 0; x < 5; x++)
            for (int y = 0; y < 8; y++)
                action[x, y] = 0;
    }

    public void SetQ_Table()
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
                Q_Table[nowStateCount, nowPiece, x, y] += action[x, y];
            }
        }
    }

    public void SetState()
    {
        Tile[,] getTiles = ChessManager.instance.chessTileList;
        for (int x = 0;x < 8; x++)
            for(int y = 0;y < 8; y++)
            {
                if(getTiles[x, y].locatedPiece != null)
                    state[nowStateCount, x,y] = getTiles[x,y].locatedPiece.piecePoint;
            }
        DebugState();
    }

    public void SetAction()
    {
        Tile[,] getTiles = ChessManager.instance.chessTileList;

        List<Piece> nowPieces = new List<Piece>();

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

                if (nowPieces[i].pieceColor == GameColor.White)
                    action[startPos, targetPos] = nowPieces[i].piecePoint;
                else
                    action[startPos, targetPos] = nowPieces[i].piecePoint * -1;
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
        for (int x = 0; x < 6; x++)
        {
            arr.Append(x + " ����\n");
            for (int y = 0; y < 8; y++)
            {
                arr.Append("[");
                for (int z = 0; z < 8; z++)
                {
                    arr.Append(state[x,z,y]);
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
    #endregion
}