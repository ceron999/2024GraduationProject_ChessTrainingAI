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

    #region ���� �̵�
    void EvaluateLeftMoveTiles()
    {
        for (int i = 1; nowPos.x - i >= 0; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x - i, nowPos.y);

            // 0. �ش� Ÿ���� �������� ������ �ߴ�
            if (!IsAvailableTIle(targetVector))
                break;

            // 1. �ش� Ÿ���� ��������� �̵� Ÿ�Ϸ� �߰�
            if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece == null)
            {
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
            }
            else
            {
                // 2. �ش� Ÿ���� �⹰�� �� == ������ �⹰�� ���̸� �Ѿ
                if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 3. �ش� Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                    break;
                }
            }
        }
    }

    void EvaluateRightMoveTiles()
    {
        for (int i = 1; nowPos.x + i <= 7; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x + i, nowPos.y);

            // 0. �ش� Ÿ���� �������� ������ �ߴ�
            if (!IsAvailableTIle(targetVector))
                break;

            // 1. �ش� Ÿ���� ��������� �̵� Ÿ�Ϸ� �߰�
            if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece == null)
            {
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
            }
            else
            {
                // 2. �ش� Ÿ���� �⹰�� �� == ������ �⹰�� ���̸� �Ѿ
                if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 3. �ش� Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                    break;
                }
            }
        }
    }

    void EvaluateUpMoveTiles()
    {
        for (int i = 1; nowPos.y + i <= 7; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x, nowPos.y + i);

            // 0. �ش� Ÿ���� �������� ������ �ߴ�
            if (!IsAvailableTIle(targetVector))
                break;

            // 1. �ش� Ÿ���� ��������� �̵� Ÿ�Ϸ� �߰�
            if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece == null)
            {
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
            }
            else
            {
                // 2. �ش� Ÿ���� �⹰�� �� == ������ �⹰�� ���̸� �Ѿ
                if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 3. �ش� Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                    break;
                }
            }
        }
    }

    void EvaluateDownMoveTiles()
    {
        for (int i = 1; nowPos.y - i >= 0; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x, nowPos.y - i);

            // 0. �ش� Ÿ���� �������� ������ �ߴ�
            if (!IsAvailableTIle(targetVector))
                break;

            // 1. �ش� Ÿ���� ��������� �̵� Ÿ�Ϸ� �߰�
            if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece == null)
            {
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
            }
            else
            {
                // 2. �ش� Ÿ���� �⹰�� �� == ������ �⹰�� ���̸� �Ѿ
                if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 3. �ش� Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                    break;
                }
            }
        }
    }
    #endregion

    #region �밢�� �̵�
    //(-1.+1)
    void EvaluateLeftUpMoveTiles()
    {
        Vector2Int targetVector = nowPos;
        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x - i, nowPos.y + i);

            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(targetVector))
                break;

            // 2. Ÿ���� �⹰�� ������ �̵� Ÿ�� �߰�
            if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece == null)
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
            else
            {
                // 3. Ÿ���� �⹰ �� == ������ �⹰�� ���̸� �Ѿ
                if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 4. Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                    break;
                }
            }
        }
    }

    //(+1.+1)
    void EvaluateRightUpMoveTiles()
    {
        Vector2Int targetVector = nowPos;
        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x + i, nowPos.y + i);

            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(targetVector))
                break;

            // 2. Ÿ���� �⹰�� ������ �̵� Ÿ�� �߰�
            if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece == null)
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
            else
            {
                // 3. Ÿ���� �⹰ �� == ������ �⹰�� ���̸� �Ѿ
                if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 4. Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                    break;
                }
            }
        }
    }

    //(-1.-1)
    void EvaluateLeftDownMoveTiles()
    {
        Vector2Int targetVector = nowPos;
        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x - i, nowPos.y - i);

            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(targetVector))
                break;

            // 2. Ÿ���� �⹰�� ������ �̵� Ÿ�� �߰�
            if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece == null)
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
            else
            {
                // 3. Ÿ���� �⹰ �� == ������ �⹰�� ���̸� �Ѿ
                if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 4. Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                    break;
                }
            }
        }
    }

    //(+1.-1)
    void EvaluateRightDownMoveTiles()
    {
        Vector2Int targetVector = nowPos;
        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x + i, nowPos.y - i);

            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(targetVector))
                break;

            // 2. Ÿ���� �⹰�� ������ �̵� Ÿ�� �߰�
            if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece == null)
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
            else
            {
                // 3. Ÿ���� �⹰ �� == ������ �⹰�� ���̸� �Ѿ
                if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 4. Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                    break;
                }
            }
        }
    }
    #endregion
}
