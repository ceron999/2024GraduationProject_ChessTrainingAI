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

    public float maxActionReward = 0;
    public float minActionReward = 0;

    public Action()
    {
        ClearAction0();
    }

    #region Action ������ ���� �Լ�
    // �Ķ���ͷ� ���� action�� ����
    public void DeepCopyAction(Action getAction)
    {
        for (int i = 0; i < 64; i++)
            for (int j = 0; j < 64; j++)
                nowActionArr[i, j] = getAction.nowActionArr[i, j];
    }

    // state�� 0���� �ʱ�ȭ
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

        ClearAction0();

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
    public void ConvertTestBoard2NowAction()
    {
        TestTile[,] getTiles = TestManager.Instance.testTileList;

        List<TestPiece> nowPieces = new List<TestPiece>();
        ClearAction0();

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

    // State�� �޾� action table�� ������Ʈ
    public void UpdateAction(State getState)
    {
        // 1. getState�� ���·� TestBoard�� ����
        TestManager.Instance.ConvertState2TestTileBoard(getState);
        
        // 2. TestBoard ���¿��� �̵��� �� �ִ� Ÿ���� Ž���Ѵ�. 
        TestManager.Instance.EvaluateMovableTiles();

        // 3. �׽�Ʈ�� �� �⹰�� movableTile�� nowAction���� ��ȯ�Ѵ�.
        ConvertTestBoard2NowAction();
    }
    #endregion

    // nowAction���� ���� ���� reward�� ������ action�� row�� col�� return�Դϴ�
    public List<Vector2Int> GetMaxAction(State getState)
    {
        List<Vector2Int> largestIndexList = new List<Vector2Int>();
        largestIndexList.Add(availableActionList[0]);

        Vector2Int currentIndex;
        for (int i=0; i< availableActionList.Count; i++)
        {
            //Debug.Log("�˻� ����");
            currentIndex = availableActionList[i];
            if (getState.nowState[currentIndex.y % 8, currentIndex.y / 8] 
                        > getState.nowState[largestIndexList[0].y % 8, largestIndexList[0].y / 8])
            {
                //Debug.Log(availableActionList[i] + " �ʱ�ȭ �� �߰�");
                maxActionReward = getState.nowState[currentIndex.y % 8, currentIndex.y / 8];
                largestIndexList.Clear();
                largestIndexList.Add(availableActionList[i]);
            }
            else if(getState.nowState[currentIndex.y % 8, currentIndex.y / 8] 
                        == getState.nowState[largestIndexList[0].y % 8, largestIndexList[0].y / 8])
            {
                //Debug.Log(availableActionList[i] + " �߰�");
                largestIndexList.Add(availableActionList[i]);
            }
        }

        return largestIndexList;
    }

    // nowAction���� ���� ���� reward�� ������ action�� row�� col�� return�Դϴ�
    public List<Vector2Int> GetMinAction(State getState)
    {
        List<Vector2Int> smallestIndexList = new List<Vector2Int>();
        smallestIndexList.Add(availableActionList[0]);

        Vector2Int currentIndex;
        for (int i = 0; i < availableActionList.Count; i++)
        {
            currentIndex = availableActionList[i];
            if (getState.nowState[currentIndex.y % 8, currentIndex.y / 8] 
                        < getState.nowState[smallestIndexList[0].y % 8, smallestIndexList[0].y / 8])
            {
                //Debug.Log(currentIndex + "�ּҰ� Ȯ����: " + getState.nowState[currentIndex.y % 8, currentIndex.y / 8]);
                minActionReward = getState.nowState[currentIndex.y % 8, currentIndex.y / 8];
                smallestIndexList.Clear();
                smallestIndexList.Add(availableActionList[i]);
            }
            else if (getState.nowState[currentIndex.y % 8, currentIndex.y / 8] 
                        == getState.nowState[smallestIndexList[0].y % 8, smallestIndexList[0].y / 8])
            {
                smallestIndexList.Add(availableActionList[i]);
            }
        }

        return smallestIndexList;
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
