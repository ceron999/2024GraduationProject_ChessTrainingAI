using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override void EvaluateMove()
    {
        EvaluateLeftUpMoveTiles();
        EvaluateRightUpMoveTiles();
        EvaluateLeftDownMoveTiles();
        EvaluateRightDownMoveTiles();
    }

    #region 대각선 이동
    //(-1.+1)
    void EvaluateLeftUpMoveTiles()
    {
        Vector2Int targetVector = nowPos;
        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x - i, nowPos.y + i);

            if (!IsEvaluateTile(targetVector))
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

            if (!IsEvaluateTile(targetVector))
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

            if (!IsEvaluateTile(targetVector))
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

            if (!IsEvaluateTile(targetVector))
                break;
        }
    }

    /// <summary>
    /// 평가 함수에서 반복적으로 나타나는 부분을 뽑아서 함수로 정의함.
    /// 해당 타일을 무시, 이동 가능 타일, 공격 기물 설정하는 함수
    /// </summary>
    /// <param name="getVector"> 목표 타일 벡터 </param>
    bool IsEvaluateTile(Vector2Int getVector)
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
    #endregion
}
