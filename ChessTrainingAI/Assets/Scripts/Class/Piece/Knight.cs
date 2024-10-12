using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    //(-1.+1)
    public override void EvaluateMove()
    {
        List<Vector2Int> targetVector = new List<Vector2Int>();
        targetVector.Add(nowPos + new Vector2Int(-2, +1));
        targetVector.Add(nowPos + new Vector2Int(-2, -1));
        targetVector.Add(nowPos + new Vector2Int(+2, +1));
        targetVector.Add(nowPos + new Vector2Int(+2, -1));
        targetVector.Add(nowPos + new Vector2Int(-1, +2));
        targetVector.Add(nowPos + new Vector2Int(-1, -2));
        targetVector.Add(nowPos + new Vector2Int(+1, +2));
        targetVector.Add(nowPos + new Vector2Int(+1, -2));

        for (int i = 0; i < targetVector.Count; i++)
        {
            // 1. 해당하는 타일이 존재하지 않으면 넘어감
            if (!IsAvailableTIle(targetVector[i]))
                continue;

            SetIsColorAttack(ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y]);

            // 2. 해당하는 타일의 기물이 없으면 추가함
            if (ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y].locatedPiece == null)
            {
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y]);
                continue;
            }

            // 3. 해당하는 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
            if (ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y].locatedPiece.pieceColor != pieceColor)
            {
                attackPieceList.Add(ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y].locatedPiece);
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y]);
                continue;
            }

            // 4. 해당하는 타일의 기물 색 == 선택한 기물의 색이면 넘어감
            if (ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y].locatedPiece.pieceColor == pieceColor)
                continue;
        }
    }
}
