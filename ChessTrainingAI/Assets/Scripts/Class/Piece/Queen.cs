using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    bool isBlock = false;
    bool isEvaluateSkip = false;            // ���� �� (���� ���⿡ �� �⹰�� ���� + �� �⹰�� ŷ�� �ƴ�)�̸� �򰡸� ��ŵ�ع���
                                            // ���� : �� �⹰ �ڱ��� ���ؼ� ���� Ÿ�Ϸ� ������ ������ �ϳ� �� �ʿ�����

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

            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(targetVector))
                break;

            // 2. �򰡸� �ߴ��ϰ� �����Ƿ� �ߴ�
            if (isEvaluateSkip)
                break;

            if (isBlock)
            {
                SetIsColorBlockAttack(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                continue;
            }

            EvaluateCrossTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
    }

    void EvaluateRightMoveTiles()
    {
        for (int i = 1; nowPos.x + i <= 7; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x + i, nowPos.y);

            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(targetVector))
                break;

            // 2. �򰡸� �ߴ��ϰ� �����Ƿ� �ߴ�
            if (isEvaluateSkip)
                break;

            if (isBlock)
            {
                SetIsColorBlockAttack(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                continue;
            }

            EvaluateCrossTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
    }

    void EvaluateUpMoveTiles()
    {
        for (int i = 1; nowPos.y + i <= 7; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x, nowPos.y + i);

            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(targetVector))
                break;

            // 2. �򰡸� �ߴ��ϰ� �����Ƿ� �ߴ�
            if (isEvaluateSkip)
                break;

            if (isBlock)
            {
                SetIsColorBlockAttack(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                continue;
            }

            EvaluateCrossTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
    }

    void EvaluateDownMoveTiles()
    {
        for (int i = 1; nowPos.y - i >= 0; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x, nowPos.y - i);

            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(targetVector))
                break;

            // 2. �򰡸� �ߴ��ϰ� �����Ƿ� �ߴ�
            if (isEvaluateSkip)
                break;

            if (isBlock)
            {
                SetIsColorBlockAttack(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                continue;
            }

            EvaluateCrossTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
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

            // 2. �򰡸� �ߴ��ϰ� �����Ƿ� �ߴ�
            if (isEvaluateSkip)
                break;

            if (isBlock)
            {
                SetIsColorBlockAttack(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                continue;
            }

            EvaluateDiagonalTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
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

            // 2. �򰡸� �ߴ��ϰ� �����Ƿ� �ߴ�
            if (isEvaluateSkip)
                break;

            if (isBlock)
            {
                SetIsColorBlockAttack(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                continue;
            }

            EvaluateDiagonalTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
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

            // 2. �򰡸� �ߴ��ϰ� �����Ƿ� �ߴ�
            if (isEvaluateSkip)
                break;

            if (isBlock)
            {
                SetIsColorBlockAttack(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                continue;
            }

            EvaluateDiagonalTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
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

            // 2. �򰡸� �ߴ��ϰ� �����Ƿ� �ߴ�
            if (isEvaluateSkip)
                break;

            if (isBlock)
            {
                SetIsColorBlockAttack(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                continue;
            }

            EvaluateDiagonalTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
    }
    #endregion

    void EvaluateCrossTiles(Vector2Int getVector)
    {
        Tile nowTIle = ChessManager.instance.chessTileList[getVector.x, getVector.y];

        // 1. �ش� Ÿ���� ��������� �̵� Ÿ�Ϸ� �߰�
        if (nowTIle.locatedPiece == null)
        {
            movableTIleList.Add(nowTIle);
            SetIsColorAttack(nowTIle);
        }
        else
        {
            // 2. �ش� Ÿ���� �⹰�� �� == ������ �⹰�� ���̸� �Ѿ
            if (nowTIle.locatedPiece.pieceColor == pieceColor)
            {
                isEvaluateSkip = true;
                return;
            }

            // 3. �ش� Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
            else
            {
                attackPieceList.Add(nowTIle.locatedPiece);
                movableTIleList.Add(nowTIle);
                SetIsColorAttack(nowTIle);

                if (nowTIle.locatedPiece.pieceType == PieceType.King)
                {
                    isBlock = true;
                    SetIsColorBlockAttack(nowTIle);
                }
                else
                    isEvaluateSkip = true;

                return;
            }
        }
    }

    void EvaluateDiagonalTiles(Vector2Int getVector)
    {
        Tile nowTIle = ChessManager.instance.chessTileList[getVector.x, getVector.y];

        // 2. Ÿ���� �⹰�� ������ �̵� Ÿ�� �߰�
        if (nowTIle.locatedPiece == null)
        {
            movableTIleList.Add(nowTIle);
            SetIsColorAttack(nowTIle);
        }
        else
        {
            // 3. Ÿ���� �⹰ �� == ������ �⹰�� ���̸� �Ѿ
            if (nowTIle.locatedPiece.pieceColor == pieceColor)
            {
                isEvaluateSkip = true;
                return;
            }

            // 4. Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
            else
            {
                attackPieceList.Add(nowTIle.locatedPiece);
                movableTIleList.Add(nowTIle);
                SetIsColorAttack(nowTIle);

                if (nowTIle.locatedPiece.pieceType == PieceType.King)
                {
                    isBlock = true;
                    SetIsColorBlockAttack(nowTIle);
                }
                else
                    isEvaluateSkip = true;

                return;
            }
        }
    }
}
