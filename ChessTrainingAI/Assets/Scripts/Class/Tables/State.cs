using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[System.Serializable]
public class State
{
    public Vector2Int lastAction;       // �ش� state�� �Ǳ� �ٷ� �� action�� ����

    public float[,] nowState = new float[8,8];
    public Action nowAction = new Action();

    public List<Vector2Int> maxRewardActionList = new List<Vector2Int>();
    public List<Vector2Int> minRewardActionList = new List<Vector2Int>();

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
        maxRewardActionList = nowAction.GetMaxAction(this);
        minRewardActionList = nowAction.GetMinAction(this);
    }

    // �Ķ���ͷ� ���� state�� �����Ѵ�.
    public void DeepCopyState(State getState)
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                nowState[i, j] = getState.nowState[i, j];

        nowAction.DeepCopyAction(getState.nowAction);
    }

    // action�� �����Ͽ��� �� ���� Ÿ�ϰ� ���� Ÿ���� ������ �����Ѵ�.
    public void UpdateState(Vector2Int getAction)
    {
        Vector2Int actionStartPos = new Vector2Int(getAction.x % 8, getAction.x / 8);
        Vector2Int actionEndPos = new Vector2Int(getAction.y % 8, getAction.y / 8); ;

        nowState[actionEndPos.x, actionEndPos.y] = nowState[actionStartPos.x, actionStartPos.y];
        nowState[actionStartPos.x, actionStartPos.y] = 0;

        nowAction.UpdateAction(this);
        maxRewardActionList = nowAction.GetMaxAction(this);
        minRewardActionList = nowAction.GetMinAction(this);
    }

    // ���� state�� ����Ѵ�
    public void DebugState()
    {
        StringBuilder arr = new StringBuilder();
        for (int y = 7; y >= 0; y--)
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

    public bool IsPieceColorWhite(int getStartPosNum)
    {
        Vector2Int startPos = new Vector2Int(getStartPosNum % 8, getStartPosNum / 8);

        if (nowState[startPos.x, startPos.y] > 0)
            return true;
        else
            return false;
    }
}
