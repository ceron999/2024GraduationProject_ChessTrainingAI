using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestKnight : TestPiece
{
    public override void SetAttackPieceList()
    {
        base.SetAttackPieceList();

        List<Vector2Int> targetVector = new List<Vector2Int>();
        targetVector.Add(nowPos + new Vector2Int(-2, +1));
        targetVector.Add(nowPos + new Vector2Int(-2, -1));
        targetVector.Add(nowPos + new Vector2Int(+2, +1));
        targetVector.Add(nowPos + new Vector2Int(+2, -1));
        targetVector.Add(nowPos + new Vector2Int(-1, +2));
        targetVector.Add(nowPos + new Vector2Int(-1, -2));
        targetVector.Add(nowPos + new Vector2Int(+1, +2));
        targetVector.Add(nowPos + new Vector2Int(+1, -2));

        for (int i = 0; i < targetVector.Count; i++)
        {
            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(targetVector[i]))
                continue;

            // 3. �ش��ϴ� Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
            if (TestManager.Instance.testTileList[targetVector[i].x, targetVector[i].y].locatedPiece.pieceColor != pieceColor)
            {
                attackPieceList.Add(TestManager.Instance.testTileList[targetVector[i].x, targetVector[i].y].locatedPiece);
                continue;
            }
        }
    }

    public override void SetMovableTileList()
    {
        base.SetMovableTileList();
        List<Vector2Int> targetVector = new List<Vector2Int>();
        targetVector.Add(nowPos + new Vector2Int(-2, +1));
        targetVector.Add(nowPos + new Vector2Int(-2, -1));
        targetVector.Add(nowPos + new Vector2Int(+2, +1));
        targetVector.Add(nowPos + new Vector2Int(+2, -1));
        targetVector.Add(nowPos + new Vector2Int(-1, +2));
        targetVector.Add(nowPos + new Vector2Int(-1, -2));
        targetVector.Add(nowPos + new Vector2Int(+1, +2));
        targetVector.Add(nowPos + new Vector2Int(+1, -2));

        for (int i = 0; i < targetVector.Count; i++)
        {
            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(targetVector[i]))
                continue;

            SetIsColorAttack(TestManager.Instance.testTileList[targetVector[i].x, targetVector[i].y]);

            // 2. �ش��ϴ� Ÿ���� �⹰�� ������ �߰���
            if (TestManager.Instance.testTileList[targetVector[i].x, targetVector[i].y].locatedPiece == null)
            {
                movableTIleList.Add(TestManager.Instance.testTileList[targetVector[i].x, targetVector[i].y]);
                continue;
            }

            // 3. �ش��ϴ� Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
            if (TestManager.Instance.testTileList[targetVector[i].x, targetVector[i].y].locatedPiece.pieceColor != pieceColor)
            {
                attackPieceList.Add(TestManager.Instance.testTileList[targetVector[i].x, targetVector[i].y].locatedPiece);
                movableTIleList.Add(TestManager.Instance.testTileList[targetVector[i].x, targetVector[i].y]);
                continue;
            }

            // 4. �ش��ϴ� Ÿ���� �⹰ �� == ������ �⹰�� ���̸� �Ѿ
            if (TestManager.Instance.testTileList[targetVector[i].x, targetVector[i].y].locatedPiece.pieceColor == pieceColor)
                continue;
        }
    }
}
