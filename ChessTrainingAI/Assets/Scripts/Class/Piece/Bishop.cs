using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Bishop : Piece
{
    bool isBlock = false;
    bool isEvaluateSkip = false;            // ���� �� (���� ���⿡ �� �⹰�� ���� + �� �⹰�� ŷ�� �ƴ�)�̸� �򰡸� ��ŵ�ع���
                                            // ���� : �� �⹰ �ڱ��� ���ؼ� ���� Ÿ�Ϸ� ������ ������ �ϳ� �� �ʿ�����

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

            EvaluateTiles(targetVector);
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

            EvaluateTiles(targetVector);
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

            EvaluateTiles(targetVector);
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

            EvaluateTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
    }

    /// <summary>
    /// �� �Լ����� �ݺ������� ��Ÿ���� �κ��� �̾Ƽ� �Լ��� ������.
    /// �ش� Ÿ���� ����, �̵� ���� Ÿ��, ���� �⹰ �����ϴ� �Լ�
    /// </summary>
    /// <param name="getVector"> ��ǥ Ÿ�� ���� </param>
    void EvaluateTiles(Vector2Int getVector)
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
                SetIsColorAttack(nowTIle);
                return;
            }

            // 4. Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
            else
            {
                attackPieceList.Add(nowTIle.locatedPiece);
                movableTIleList.Add(nowTIle);
                SetIsColorAttack(nowTIle);

                if (nowTIle.locatedPiece.pieceType == PieceType.K)
                {
                    isBlock = true;
                    SetIsColorBlockAttack(nowTIle);
                }
                else isEvaluateSkip = true;

                return;
            }

        }
    }
    #endregion
}
