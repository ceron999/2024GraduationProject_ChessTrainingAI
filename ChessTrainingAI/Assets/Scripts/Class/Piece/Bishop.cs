using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override void FindMovableMoveTiles()
    {
        //1.전진 방향 확인
        EvaluateMoveTiles();

        SetMovablePiecesSelected();
        //DebugMovableTiles(movableTIles);
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
        EvaluateLeftUpMoveTiles();
        EvaluateRightUpMoveTiles();
        EvaluateLeftDownMoveTiles();
        EvaluateRightDownMoveTiles();

        SetMovablePiecesSelected();
    }

    //(-1.+1)
    void EvaluateLeftUpMoveTiles()
    {
        Vector2Int evaluateVector = nowPos;
        for (int i = 1; ; i++)
        {
            evaluateVector = new Vector2Int(nowPos.x - i, nowPos.y + i);

            //해당하는 타일이 존재하지 않으면 return;
            if (!IsAvailableTIle(evaluateVector))
                break;

            if (ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y].nowLocateColor == pieceColor)
                break;
            else if (ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y].nowLocateColor == GameColor.Null)
                movableTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y]);
            else
            {
                movableTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y]);
                break;
            }
        }
    }

    //(+1.+1)
    void EvaluateRightUpMoveTiles()
    {
        Vector2Int evaluateVector = nowPos;
        for (int i = 1; ; i++)
        {
            evaluateVector = new Vector2Int(nowPos.x + i, nowPos.y + i);

            //해당하는 타일이 존재하지 않으면 return;
            if (!IsAvailableTIle(evaluateVector))
                break;

            if (ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y].nowLocateColor == pieceColor)
                break;
            else if (ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y].nowLocateColor == GameColor.Null)
                movableTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y]);
            else
            {
                movableTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y]);
                break;
            }
        }
    }

    //(-1.-1)
    void EvaluateLeftDownMoveTiles()
    {
        Vector2Int evaluateVector = nowPos;
        for (int i = 1; ; i++)
        {
            evaluateVector = new Vector2Int(nowPos.x - i, nowPos.y - i);

            //해당하는 타일이 존재하지 않으면 return;
            if (!IsAvailableTIle(evaluateVector))
                break;

            if (ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y].nowLocateColor == pieceColor)
                break;
            else if (ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y].nowLocateColor == GameColor.Null)
                movableTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y]);
            else
            {
                movableTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y]);
                break;
            }
        }
    }

    //(+1.-1)
    void EvaluateRightDownMoveTiles()
    {
        Vector2Int evaluateVector = nowPos;
        for (int i = 1; ; i++)
        {
            evaluateVector = new Vector2Int(nowPos.x + i, nowPos.y - i);

            //해당하는 타일이 존재하지 않으면 return;
            if (!IsAvailableTIle(evaluateVector))
                break;

            if (ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y].nowLocateColor == pieceColor)
                break;
            else if (ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y].nowLocateColor == GameColor.Null)
                movableTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y]);
            else
            {
                movableTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y]);
                break;
            }
        }
    }

    void EvaluateAttackTile()
    {
        Vector2Int evaluateVector = nowPos;
        for (int i = 1; ; i++)
        {
            evaluateVector = new Vector2Int(nowPos.x - i, nowPos.y + i);

            //해당하는 타일이 존재하지 않으면 return;
            if (!IsAvailableTIle(evaluateVector))
                break;

            if (ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y].nowLocateColor == pieceColor)
                break;
            else if (ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y].nowLocateColor == GameColor.Null)
                attackTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y]);
            else
            {
                attackTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y]);
                break;
            }
        }

        for (int i = 1; ; i++)
        {
            evaluateVector = new Vector2Int(nowPos.x + i, nowPos.y + i);

            //해당하는 타일이 존재하지 않으면 return;
            if (!IsAvailableTIle(evaluateVector))
                break;

            if (ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y].nowLocateColor == pieceColor)
                break;
            else if (ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y].nowLocateColor == GameColor.Null)
                attackTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y]);
            else
            {
                attackTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y]);
                break;
            }
        }

        for (int i = 1; ; i++)
        {
            evaluateVector = new Vector2Int(nowPos.x - i, nowPos.y - i);

            //해당하는 타일이 존재하지 않으면 return;
            if (!IsAvailableTIle(evaluateVector))
                break;

            if (ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y].nowLocateColor == pieceColor)
                break;
            else if (ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y].nowLocateColor == GameColor.Null)
                attackTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y]);
            else
            {
                attackTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y]);
                break;
            }
        }

        for (int i = 1; ; i++)
        {
            evaluateVector = new Vector2Int(nowPos.x + i, nowPos.y - i);

            //해당하는 타일이 존재하지 않으면 return;
            if (!IsAvailableTIle(evaluateVector))
                break;

            if (ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y].nowLocateColor == pieceColor)
                break;
            else if (ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y].nowLocateColor == GameColor.Null)
                attackTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y]);
            else
            {
                attackTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector.x, evaluateVector.y]);
                break;
            }
        }
    }
}
