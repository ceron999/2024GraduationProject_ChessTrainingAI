using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[System.Serializable]
public class State
{
    public float[,] nowState = new float[8,8];
    public Action nowAction = new Action();

    public State()
    {
        SetState0();
    }

    // state�� 0���� �ʱ�ȭ�Ѵ�.
    public void SetState0()
    {
        for(int i = 0; i < 8; i++) 
            for(int j = 0; j< 8 ; j++)
                nowState[i,j] = 0;
    }

    public void SetNowState()
    {
        Tile[,] getTiles = ChessManager.instance.chessTileList;

        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
            {
                nowState[x, y] = 0;
                if (getTiles[x, y].locatedPiece != null)
                    nowState[x, y] = getTiles[x, y].locatedPiece.piecePoint;
            }

        nowAction.SetNowAction();
    }

    // �Ķ���ͷ� ���� state�� �����Ѵ�.
    public void DeepCopyState(State getState)
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                nowState[i, j] = getState.nowState[i, j];

        nowAction = getState.nowAction;
    }

    // action�� �����Ͽ��� �� ���� Ÿ�ϰ� ���� Ÿ���� ������ �����Ѵ�.
    public void UpdateState(Vector2Int actionStartPos, Vector2Int actionEndPos)
    {
        nowState[actionEndPos.x, actionEndPos.y] = nowState[actionStartPos.x, actionStartPos.y];
        nowState[actionStartPos.x, actionStartPos.y] = 0;

        nowAction.UpdateAction(this);
    }

    // ���� state�� ����Ѵ�
    public void DebugState()
    {
        Debug.Log("����");
        StringBuilder arr = new StringBuilder();
        for (int y = 7; y > 0; y--)
        {
            arr.Append("[");
            for (int z = 0; z < 8; z++)
            {
                arr.Append(nowState[z, y]);
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
}
