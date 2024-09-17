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
        Vector2Int targetVector = nowPos;
        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x - i, nowPos.y + i);

            if (!IsEvaluateTile(targetVector))
                break;
        }
    }

    //(+1.+1)
    void EvaluateRightUpMoveTiles()
    {
        Vector2Int targetVector = nowPos;
        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x + i, nowPos.y + i);

            if (!IsEvaluateTile(targetVector))
                break;
        }
    }

    //(-1.-1)
    void EvaluateLeftDownMoveTiles()
    {
        Vector2Int targetVector = nowPos;
        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x - i, nowPos.y - i);

            if (!IsEvaluateTile(targetVector))
                break;
        }
    }

    //(+1.-1)
    void EvaluateRightDownMoveTiles()
    {
        Vector2Int targetVector = nowPos;
        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x + i, nowPos.y - i);

            if (!IsEvaluateTile(targetVector))
                break;
        }
    }

    /// <summary>
    /// �� �Լ����� �ݺ������� ��Ÿ���� �κ��� �̾Ƽ� �Լ��� ������.
    /// �ش� Ÿ���� ����, �̵� ���� Ÿ��, ���� �⹰ �����ϴ� �Լ�
    /// </summary>
    /// <param name="getVector"> ��ǥ Ÿ�� ���� </param>
    bool IsEvaluateTile(Vector2Int getVector)
    {
        // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
        if (!IsAvailableTIle(getVector))
            return false;

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
                return false;

            // 4. Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
            else
            {
                attackPieceList.Add(nowTIle.locatedPiece);
                movableTIleList.Add(nowTIle);
                SetIsColorAttack(nowTIle);
                return false;
            }
        }

        return true;
    }
    #endregion
}
