using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override void EvaluateMove()
    {
        EvaluateLeftMoveTiles();
        EvaluateRightMoveTiles();
        EvaluateUpMoveTiles();
        EvaluateDownMoveTiles();
        EvaluateLeftUpMoveTiles();
        EvaluateRightUpMoveTiles();
        EvaluateLeftDownMoveTiles();
        EvaluateRightDownMoveTiles();
    }

    #region 수직 이동
    void EvaluateLeftMoveTiles()
    {
        for (int i = 1; nowPos.x - i >= 0; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x - i, nowPos.y);

            if (!EvaluateCrossTiles(targetVector))
                break;
        }
    }

    void EvaluateRightMoveTiles()
    {
        for (int i = 1; nowPos.x + i <= 7; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x + i, nowPos.y);

            if (!EvaluateCrossTiles(targetVector))
                break;
        }
    }

    void EvaluateUpMoveTiles()
    {
        for (int i = 1; nowPos.y + i <= 7; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x, nowPos.y + i);

            if (!EvaluateCrossTiles(targetVector))
                break;
        }
    }

    void EvaluateDownMoveTiles()
    {
        for (int i = 1; nowPos.y - i >= 0; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x, nowPos.y - i);

            if (!EvaluateCrossTiles(targetVector))
                break;
        }
    }
    #endregion

    #region 대각선 이동
    //(-1.+1)
    void EvaluateLeftUpMoveTiles()
    {
        Vector2Int targetVector = nowPos;
        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x - i, nowPos.y + i);

            if (!EvaluateDiagonalTiles(targetVector))
                break;
        }
    }

    //(+1.+1)
    void EvaluateRightUpMoveTiles()
    {
        Vector2Int targetVector = nowPos;
        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x + i, nowPos.y + i);

            if (!EvaluateDiagonalTiles(targetVector))
                break;
        }
    }

    //(-1.-1)
    void EvaluateLeftDownMoveTiles()
    {
        Vector2Int targetVector = nowPos;
        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x - i, nowPos.y - i);

            if (!EvaluateDiagonalTiles(targetVector))
                break;
        }
    }

    //(+1.-1)
    void EvaluateRightDownMoveTiles()
    {
        Vector2Int targetVector = nowPos;
        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x + i, nowPos.y - i);

            if (!EvaluateDiagonalTiles(targetVector))
                break;
        }
    }
    #endregion

    bool EvaluateCrossTiles(Vector2Int getVector)
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

    bool EvaluateDiagonalTiles(Vector2Int getVector)
    {
        // 1. 해당하는 타일이 존재하지 않으면 넘어감
        if (!IsAvailableTIle(getVector))
            return false;

        Tile nowTIle = ChessManager.instance.chessTileList[getVector.x, getVector.y];

        // 2. 타일의 기물이 없으면 이동 타일 추가
        if (nowTIle.locatedPiece == null)
        {
            movableTIleList.Add(nowTIle);
            SetIsColorAttack(nowTIle);
        }
        else
        {
            // 3. 타일의 기물 색 == 선택한 기물의 색이면 넘어감
            if (nowTIle.locatedPiece.pieceColor == pieceColor)
                return false;

            // 4. 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
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
}
