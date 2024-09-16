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

    #region �밢�� �̵�
    //(-1.+1)
    void EvaluateLeftUpMoveTiles()
    {
        Vector2Int evaluateVector = nowPos;
        for (int i = 1; ; i++)
        {
            evaluateVector = new Vector2Int(nowPos.x - i, nowPos.y + i);

            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(evaluateVector))
                break;

            // 2. Ÿ���� �⹰�� ������ �̵� Ÿ�� �߰�
            if (ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece == null)
                movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y]);
            else
            {
                // 3. Ÿ���� �⹰ �� == ������ �⹰�� ���̸� �Ѿ
                if (ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 4. Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y]);
                    break;
                }
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

            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(evaluateVector))
                break;

            // 2. Ÿ���� �⹰�� ������ �̵� Ÿ�� �߰�
            if (ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece == null)
                movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y]);
            else
            {
                // 3. Ÿ���� �⹰ �� == ������ �⹰�� ���̸� �Ѿ
                if (ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 4. Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y]);
                    break;
                }
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

            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(evaluateVector))
                break;

            // 2. Ÿ���� �⹰�� ������ �̵� Ÿ�� �߰�
            if (ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece == null)
                movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y]);
            else
            {
                // 3. Ÿ���� �⹰ �� == ������ �⹰�� ���̸� �Ѿ
                if (ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 4. Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y]);
                    break;
                }
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

            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(evaluateVector))
                break;

            // 2. Ÿ���� �⹰�� ������ �̵� Ÿ�� �߰�
            if (ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece == null)
                movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y]);
            else
            {
                // 3. Ÿ���� �⹰ �� == ������ �⹰�� ���̸� �Ѿ
                if (ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 4. Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y]);
                    break;
                }
            }
        }
    }
    #endregion
}
