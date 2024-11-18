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

    // state를 0으로 초기화한다.
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

    // 파라미터로 받은 state를 복사한다.
    public void DeepCopyState(State getState)
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                nowState[i, j] = getState.nowState[i, j];

        nowAction = getState.nowAction;
    }

    // action을 진행하였을 때 시작 타일과 도착 타일의 점수를 변경한다.
    public void UpdateState(Vector2Int actionStartPos, Vector2Int actionEndPos)
    {
        nowState[actionEndPos.x, actionEndPos.y] = nowState[actionStartPos.x, actionStartPos.y];
        nowState[actionStartPos.x, actionStartPos.y] = 0;

        nowAction.UpdateAction(this);
    }

    // 현재 state를 출력한다
    public void DebugState()
    {
        Debug.Log("상태");
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
