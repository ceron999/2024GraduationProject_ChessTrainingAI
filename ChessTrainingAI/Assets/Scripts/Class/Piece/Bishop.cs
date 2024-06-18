using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override void FindMovableMoveTiles()
    {
        //1.���� ���� Ȯ��
        EvaluateMoveTiles();

        SetMovablePiecesSelected();
        //DebugMovableTiles(movableTIles);
    }

    public override void SetAttackTile()
    {
        //1. ���� ���� Ÿ�Ϸ� ������ Ÿ�ϵ��� ���� ����Ѵ�. 
        if (attackTIles.Count >= 0)
        {
            for (int i = 0; i < attackTIles.Count; i++)
            {
                attackTIles[i].isAttackedTile = false;
            }
            attackTIles.Clear();
        }

        //2. ���� Ÿ�� ����
        EvaluateAttackTile();

        //3. ������ ���� Ÿ���� ���� �缳��
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

            //�ش��ϴ� Ÿ���� �������� ������ return;
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

            //�ش��ϴ� Ÿ���� �������� ������ return;
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

            //�ش��ϴ� Ÿ���� �������� ������ return;
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

            //�ش��ϴ� Ÿ���� �������� ������ return;
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

            //�ش��ϴ� Ÿ���� �������� ������ return;
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

            //�ش��ϴ� Ÿ���� �������� ������ return;
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

            //�ش��ϴ� Ÿ���� �������� ������ return;
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

            //�ش��ϴ� Ÿ���� �������� ������ return;
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
