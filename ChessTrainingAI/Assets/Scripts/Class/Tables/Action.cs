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

    // state를 0으로 초기화한다.
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

        // 기물 정리
        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
            {
                if (getTiles[x, y].locatedPiece != null)
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

                nowActionArr[startPos, targetPos] = nowPieces[i].piecePoint;
                availableActionList.Add(new Vector2Int(startPos, targetPos));
            }
        }
    }

    // 파라미터로 받은 action을 복사한다.
    public void DeepCopyAction(float[,] getAction)
    {
        for (int i = 0; i < 64; i++)
            for (int j = 0; j < 64; j++)
                nowActionArr[i, j] = getAction[i, j];
    }

    // State를 받아 action table을 업데이트한다.
    public void UpdateAction(State getState)
    {
        // 현재 state를 테스트매니저에 전송하여 이동 가능 리스트를 받는다

        // 이동 가능한 리스트를 actionArr에 삽입
    }


    public void DebugActionText()
    {
        Debug.Log("액션");
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
