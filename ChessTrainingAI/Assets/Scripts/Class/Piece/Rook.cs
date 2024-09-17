using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public bool isFirstMove = true;

    public override void EvaluateMove()
    {
        EvaluateLeftMoveTiles();
        EvaluateRightMoveTiles();
        EvaluateUpMoveTiles();
        EvaluateDownMoveTiles();
    }

    #region 수직 이동
    void EvaluateLeftMoveTiles()
    {

        for (int i = 1; nowPos.x - i >= 0; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x - i, nowPos.y);

            if (!EvaluateTiles(targetVector))
                break;
        }
    }

    void EvaluateRightMoveTiles()
    {
        for (int i = 1; nowPos.x + i <= 7; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x + i, nowPos.y);

            if (!EvaluateTiles(targetVector))
                break;
        }
    }

    void EvaluateUpMoveTiles()
    {
        for (int i = 1; nowPos.y + i <= 7; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x, nowPos.y + i);

            if (!EvaluateTiles(targetVector))
                break;
        }
    }

    void EvaluateDownMoveTiles()
    {
        for (int i = 1; nowPos.y + i <= 7; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x, nowPos.y - i);

            if (!EvaluateTiles(targetVector))
                break;
        }
    }

    bool EvaluateTiles(Vector2Int getVector)
    {
        // 0. 해당 타일이 존재하지 않으면 중단
        if (!IsAvailableTIle(getVector))
            return false;

        Tile nowTIle = ChessManager.instance.chessTileList[getVector.x, getVector.y];

        // 1. 해당 타일이 비어있으면 이동 타일로 추가
        if (nowTIle.locatedPiece == null)
        {
            movableTIleList.Add(nowTIle);
            SetIsColorAttack(nowTIle);
        }
        else
        {
            // 2. 해당 타일의 기물의 색 == 선택한 기물의 색이면 넘어감
            if (nowTIle.locatedPiece.pieceColor == pieceColor)
                return false;

            // 3. 해당 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
            else
            {
                attackPieceList.Add(nowTIle.locatedPiece);
                movableTIleList.Add(nowTIle);
                SetIsColorAttack(nowTIle);
                return false;
            }
        }

        return true;
    }
    #endregion
}
