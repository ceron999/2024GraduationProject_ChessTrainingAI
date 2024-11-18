using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Action
{
    public float[,] nowActionArr = new float[64, 64];
    public List<Vector2Int> availableActionList = new List<Vector2Int>();

    public Action()
    {
        ClearAction0();
    }

    // state�� 0���� �ʱ�ȭ�Ѵ�.
    public void ClearAction0()
    {
        availableActionList.Clear();
        for (int i = 0; i < 64; i++)
            for (int j = 0; j < 64; j++)
                nowActionArr[i, j] = 0;
    }

    public void SetNowAction()
    {
        Tile[,] getTiles = ChessManager.instance.chessTileList;

        List<Piece> nowPieces = new List<Piece>();

        for (int x = 0; x < 64; x++)
            for (int y = 0; y < 64; y++)
            {
                nowActionArr[x, y] = 0;
            }

        // �⹰ ����
        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
            {
                if (getTiles[x, y].locatedPiece != null)
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

                nowActionArr[startPos, targetPos] = nowPieces[i].piecePoint;
                availableActionList.Add(new Vector2Int(startPos, targetPos));
            }
        }
    }

    // �Ķ���ͷ� ���� action�� �����Ѵ�.
    public void DeepCopyAction(float[,] getAction)
    {
        for (int i = 0; i < 64; i++)
            for (int j = 0; j < 64; j++)
                nowActionArr[i, j] = getAction[i, j];
    }

    // State�� �޾� action table�� ������Ʈ�Ѵ�.
    public void UpdateAction(State getState)
    {
        // ���� state�� �׽�Ʈ�Ŵ����� �����Ͽ� �̵� ���� ����Ʈ�� �޴´�

        // �̵� ������ ����Ʈ�� actionArr�� ����
    }


    public void DebugActionText()
    {
        Debug.Log("�׼�");
        StringBuilder arr = new StringBuilder();
        for (int y = 0; y < 64; y++)
        {
            for (int z = 0; z < 64; z++)
            {
                if (MathF.Abs(nowActionArr[y, z]) > 0)
                {
                    if (nowActionArr[y, z] > 0)
                        arr.Append("White ");
                    else if (nowActionArr[y, z] < 0)
                        arr.Append("Black ");

                    switch (MathF.Abs(nowActionArr[y, z]))
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


                    arr.Append((TIleName)Enum.Parse(typeof(TIleName), y.ToString()) + " -> "
                        + (TIleName)Enum.Parse(typeof(TIleName), z.ToString()) + "\n");
                }
            }
        }
        Debug.Log(arr.ToString());
    }
}
