using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public bool isCheck = false;
    public bool isCheckMate = false;

    public override void FindMovableMoveTiles()
    {
        EvaluateMoveTiles();
    }

    public override void SetAttackTile()
    {
        //1. 이전 공격 타일로 설정한 타일들을 설정 취소한다. 
        if (attackTIles.Count >= 0)
        {
            for (int i = 0; i < attackTIles.Count; i++)
            {
                attackTIles[i].isAttackedTile = false;
            }
            attackTIles.Clear();
        }

        //2. 공격 타일 설정
        EvaluateAttackTile();

        //3. 설정된 공격 타일의 변수 재설정
        for (int i = 0; i < attackTIles.Count; i++)
        {
            attackTIles[i].isAttackedTile = true;
        }
    }

    void EvaluateMoveTiles()
    {
        List<Vector2Int> evaluateVector = new List<Vector2Int>();
        evaluateVector.Add(nowPos + new Vector2Int(-1, +1));
        evaluateVector.Add(nowPos + new Vector2Int(-1, -1));
        evaluateVector.Add(nowPos + new Vector2Int(+1, +1));
        evaluateVector.Add(nowPos + new Vector2Int(+1, -1));
        evaluateVector.Add(nowPos + new Vector2Int(0, +1));
        evaluateVector.Add(nowPos + new Vector2Int(0, -1));
        evaluateVector.Add(nowPos + new Vector2Int(+1, 0));
        evaluateVector.Add(nowPos + new Vector2Int(-1, 0));

        for (int i = 0; i < evaluateVector.Count; i++)
        {
            //해당하는 타일이 존재하지 않으면 return;
            if (!IsAvailableTIle(evaluateVector[i]))
                continue;

            //공격당하는 타일의 경우 continue
            if (ChessManager.chessManager.chessTileList[evaluateVector[i].x, evaluateVector[i].y].isAttackedTile)
                continue;

            if (ChessManager.chessManager.chessTileList[evaluateVector[i].x, evaluateVector[i].y].nowLocateColor == pieceColor)
                continue;

            else
                movableTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector[i].x, evaluateVector[i].y]);
        }


        SetMovablePiecesSelected();
    }

    void EvaluateAttackTile()
    {
        List<Vector2Int> evaluateVector = new List<Vector2Int>();
        evaluateVector.Add(nowPos + new Vector2Int(-1, +1));
        evaluateVector.Add(nowPos + new Vector2Int(-1, -1));
        evaluateVector.Add(nowPos + new Vector2Int(+1, +1));
        evaluateVector.Add(nowPos + new Vector2Int(+1, -1));
        evaluateVector.Add(nowPos + new Vector2Int(0, +1));
        evaluateVector.Add(nowPos + new Vector2Int(0, -1));
        evaluateVector.Add(nowPos + new Vector2Int(+1, 0));
        evaluateVector.Add(nowPos + new Vector2Int(-1, 0));

        for (int i = 0; i < evaluateVector.Count; i++)
        {
            //해당하는 타일이 존재하지 않으면 return;
            if (!IsAvailableTIle(evaluateVector[i]))
                continue;

            if (ChessManager.chessManager.chessTileList[evaluateVector[i].x, evaluateVector[i].y].nowLocateColor == pieceColor)
                continue;

            else
                attackTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector[i].x, evaluateVector[i].y]);
        }
    }
}
