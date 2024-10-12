using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public bool isFirstMove = true;
    bool isBlock = false;
    bool isEvaluateSkip = false;            // ���� �� (���� ���⿡ �� �⹰�� ���� + �� �⹰�� ŷ�� �ƴ�)�̸� �򰡸� ��ŵ�ع���
                                            // ���� : �� �⹰ �ڱ��� ���ؼ� ���� Ÿ�Ϸ� ������ ������ �ϳ� �� �ʿ�����

    public override void EvaluateMove()
    {
        EvaluateLeftMoveTiles();
        EvaluateRightMoveTiles();
        EvaluateUpMoveTiles();
        EvaluateDownMoveTiles();
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

            EvaluateTiles(targetVector);
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

            EvaluateTiles(targetVector);
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

            EvaluateTiles(targetVector);
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

            EvaluateTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
    }

    void EvaluateTiles(Vector2Int getVector)
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
                SetIsColorAttack(nowTIle);
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
    #endregion
}
