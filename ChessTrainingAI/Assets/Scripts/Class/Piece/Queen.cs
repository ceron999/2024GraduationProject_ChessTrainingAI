using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override void FindMovableMoveTiles()
    {
        EvaluateMoveTiles();
    }

    public override void SetAttackTile(bool isActive)
    {
        if (isActive)
        {
            Tile nowTile = null;

            
        }

        //���� isActive�� ���̸� �ش� ���� Ÿ���� �缳��
        //isActive�� �����̸� ���� ���� Ÿ�� false�� ����� ������ �غ�
        for (int i = 0; i < attackTIles.Count; i++)
        {
            attackTIles[i].isAttackedTile = isActive;
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
}
