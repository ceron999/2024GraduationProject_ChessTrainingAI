using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override void FindMovableTiles()
    {
        EvaluateMoveTiles();
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
        EvaluateLeftMoveTiles();
        EvaluateRightMoveTiles();
        EvaluateUpMoveTiles();
        EvaluateDownMoveTiles();
        EvaluateLeftUpMoveTiles();
        EvaluateRightUpMoveTiles();
        EvaluateLeftDownMoveTiles();
        EvaluateRightDownMoveTiles();

        SetMovablePiecesSelected();
    }

    void EvaluateLeftMoveTiles()
    {
        for (int i = 1; nowPos.x - i >= 0; i++)
        {
            if (ChessManager.chessManager.chessTileList[nowPos.x - i, nowPos.y].nowLocateColor == pieceColor)
                break;
            else if (ChessManager.chessManager.chessTileList[nowPos.x - i, nowPos.y].nowLocateColor == GameColor.Null)
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
