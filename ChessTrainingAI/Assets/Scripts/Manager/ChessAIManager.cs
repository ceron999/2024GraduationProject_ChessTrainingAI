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
    float[,,] state = new float[10, 8, 8];       // [���� ��, col, row]
    float[,] action = new float[64, 64];        // [���� Ÿ��, ���� Ÿ��]
    #endregion

    private void Start()
    {
        SetAllTables();
    }

    void Q_Learning()
    {

    }


    #region Table �ʱ�ȭ �� �缳��
    void SetAllTables()
    {
        for (int x = 0; x < 60; x++)
            for (int y = 0; y < 6; y++)
                for (int z = 0; z < 64; z++)
                    for (int w = 0; w < 64; w++)
                        Q_Table[x, y, z, w] = 0;

        for (int x = 0; x < 10; x++)
            for (int y = 0; y < 8; y++)
                for (int z = 0; z < 8; z++)
                    state[x, y, z] = 0;

        for (int x = 0; x < 5; x++)
            for (int y = 0; y < 8; y++)
                action[x, y] = 0;
    }

    public void SetQ_Table()
    {
        
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
        nowStateCount++;
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
                    action[startPos, targetPos] = 1;
                else
                    action[startPos, targetPos] = -1;
            }
        }

        DebugAction();
    }
    #endregion

    #region ����� �Լ�
    public void DebugQ_Table()
    {
        Debug.Log("Q_table");
        for (int x = 0; x < 60; x++)
            for (int y = 0; y < 6; y++)
                for (int z = 0; z < 64; z++)
                    for (int w = 0; w < 64; w++)
                        Q_Table[x, y, z, w] = 0;
    }
    public void DebugState()
    {
        Debug.Log("����");
        StringBuilder arr = new StringBuilder();
        for (int x = 0; x < 5; x++)
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
                if (action[y, z] == 1 || action[y, z] == -1)
                {
                    arr.Append((TIleName)Enum.Parse(typeof(TIleName), y.ToString())  + " -> " 
                        + (TIleName)Enum.Parse(typeof(TIleName), z.ToString()) + "\n");
                }
            }
        }
        Debug.Log(arr.ToString());
    }
    #endregion
}