using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public bool isFirstMove = true;

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
        EvaluateLeftMoveTiles();
        EvaluateRightMoveTiles();
        EvaluateUpMoveTiles();
        EvaluateDownMoveTiles();

        SetMovablePiecesSelected();
    }

    void EvaluateLeftMoveTiles()
    {
        for (int i = 1; nowPos.x - i >= 0; i++)
        {
            if (ChessManager.chessManager.chessTileList[nowPos.x - i, nowPos.y].nowLocateColor == pieceColor)
                break;
            else if(ChessManager.chessManager.chessTileList[nowPos.x - i, nowPos.y].nowLocateColor == GameColor.Null)
                movableTIles.Add(ChessManager.chessManager.chessTileList[nowPos.x - i, nowPos.y]);
            else
            {
                movableTIles.Add(ChessManager.chessManager.chessTileList[nowPos.x - i, nowPos.y]);
                break;
            }
        }
    }

    void EvaluateRightMoveTiles()
    {
        for (int i = 1; nowPos.x + i <= 7; i++)
        {
            if (ChessManager.chessManager.chessTileList[nowPos.x + i, nowPos.y].nowLocateColor == pieceColor)
                break;
            else if (ChessManager.chessManager.chessTileList[nowPos.x + i, nowPos.y].nowLocateColor == GameColor.Null)
                movableTIles.Add(ChessManager.chessManager.chessTileList[nowPos.x + i, nowPos.y]);
            else
            {
                movableTIles.Add(ChessManager.chessManager.chessTileList[nowPos.x + i, nowPos.y]);
                break;
            }
        }
    }

    void EvaluateUpMoveTiles()
    {
        for (int i = 1; nowPos.y + i <= 7; i++)
        {
            if (ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + i].nowLocateColor == pieceColor)
                break;
            else if (ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + i].nowLocateColor == GameColor.Null)
                movableTIles.Add(ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + i]);
            else
            {
                movableTIles.Add(ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + i]);
                break;
            }
        }
    }

    void EvaluateDownMoveTiles()
    {
        for (int i = 1; nowPos.y - i >= 0; i++)
        {
            if (ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y - i].nowLocateColor == pieceColor)
                break;
            else if (ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y - i].nowLocateColor == GameColor.Null)
                movableTIles.Add(ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y - i]);
            else
            {
                movableTIles.Add(ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y - i]);
                break;
            }
        }
    }

    void EvaluateAttackTile()
    {
        for (int i = 1; nowPos.x - i >= 0; i++)
        {
            if (ChessManager.chessManager.chessTileList[nowPos.x - i, nowPos.y].nowLocateColor == pieceColor)
                break;
            else if (ChessManager.chessManager.chessTileList[nowPos.x - i, nowPos.y].nowLocateColor == GameColor.Null)
                attackTIles.Add(ChessManager.chessManager.chessTileList[nowPos.x - i, nowPos.y]);
            else
            {
                attackTIles.Add(ChessManager.chessManager.chessTileList[nowPos.x - i, nowPos.y]);
                break;
            }
        }

        for (int i = 1; nowPos.x + i <= 7; i++)
        {
            if (ChessManager.chessManager.chessTileList[nowPos.x + i, nowPos.y].nowLocateColor == pieceColor)
                break;
            else if (ChessManager.chessManager.chessTileList[nowPos.x + i, nowPos.y].nowLocateColor == GameColor.Null)
                attackTIles.Add(ChessManager.chessManager.chessTileList[nowPos.x + i, nowPos.y]);
            else
            {
                attackTIles.Add(ChessManager.chessManager.chessTileList[nowPos.x + i, nowPos.y]);
                break;
            }
        }

        for (int i = 1; nowPos.y + i <= 7; i++)
        {
            if (ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + i].nowLocateColor == pieceColor)
                break;
            else if (ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + i].nowLocateColor == GameColor.Null)
                attackTIles.Add(ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + i]);
            else
            {
                attackTIles.Add(ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + i]);
                break;
            }
        }

        for (int i = 1; nowPos.y - i >= 0; i++)
        {
            if (ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y - i].nowLocateColor == pieceColor)
                break;
            else if (ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y - i].nowLocateColor == GameColor.Null)
                attackTIles.Add(ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y - i]);
            else
            {
                attackTIles.Add(ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y - i]);
                break;
            }
        }
    }
}
