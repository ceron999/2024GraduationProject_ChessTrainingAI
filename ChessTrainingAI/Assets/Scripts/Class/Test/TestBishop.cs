using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class TestBishop : TestPiece
{
    int nowDir = 0;
    int count = 0;

    bool isEvaluateSkip = false;
    bool isBlock = false;

    public override void SetAttackPieceList()
    {
        base.SetAttackPieceList();
        Vector2Int targetVector = nowPos;
        List<Vector2Int> direction = new List<Vector2Int>();
        direction.Add(new Vector2Int(-1, +1));
        direction.Add(new Vector2Int(+1, +1));
        direction.Add(new Vector2Int(-1, -1));
        direction.Add(new Vector2Int(+1, -1));


       while(nowDir < direction.Count)
        {
            targetVector = nowPos + direction[nowDir] * count;

            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(targetVector))
            {
                if (nowDir < 4)
                {
                    count = 0;
                    nowDir++;

                    if (nowDir > 1000)
                    {
                        Debug.Log("����!");
                        break;
                    }
                }
                else
                    break;
            }
            else
            {
                InsertAttackPieces(targetVector);
                count++;
            }
        }
    }

    public override void SetMovableTileList()
    {
        base.SetMovableTileList();

        EvaluateLeftUpMoveTiles();
        EvaluateLeftDownMoveTiles();
        EvaluateRightUpMoveTiles();
        EvaluateRightDownMoveTiles();
    }

    void InsertAttackPieces(Vector2Int getVector)
    {
        TestTile nowTIle = TestManager.Instance.testTileList[getVector.x, getVector.y];

        // 2. Ÿ���� �⹰�� ������ �̵� Ÿ�� �߰�
        if (nowTIle.locatedPiece != null)
        {
            if(nowTIle.locatedPiece.pieceColor != pieceColor)
                attackPieceList.Add(nowTIle.locatedPiece);
            else if (nowTIle.locatedPiece.pieceColor == pieceColor)
            {
                if (nowDir < 4)
                {
                    count = 0;
                    nowDir++;
                }
            }
        }
        
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
                SetIsColorBlockAttack(TestManager.Instance.testTileList[targetVector.x, targetVector.y]);
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
                SetIsColorBlockAttack(TestManager.Instance.testTileList[targetVector.x, targetVector.y]);
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
                SetIsColorBlockAttack(TestManager.Instance.testTileList[targetVector.x, targetVector.y]);
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
                SetIsColorBlockAttack(TestManager.Instance.testTileList[targetVector.x, targetVector.y]);
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
        TestTile nowTIle = TestManager.Instance.testTileList[getVector.x, getVector.y];

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
                //attackPieceList.Add(nowTIle.locatedPiece);
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