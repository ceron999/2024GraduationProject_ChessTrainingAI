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

    public override void SetAttackTile(bool isActive)
    {
        if (isActive)
        {
            
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
}
