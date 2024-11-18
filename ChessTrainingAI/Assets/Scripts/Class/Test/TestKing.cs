using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestKing : TestPiece
{
    public override void SetMovableTileList()
    {
        base.SetMovableTileList();

        List<Vector2Int> targetVector = new List<Vector2Int>();
        targetVector.Add(nowPos + new Vector2Int(-1, +1));
        targetVector.Add(nowPos + new Vector2Int(-1, -1));
        targetVector.Add(nowPos + new Vector2Int(+1, +1));
        targetVector.Add(nowPos + new Vector2Int(+1, -1));
        targetVector.Add(nowPos + new Vector2Int(0, +1));
        targetVector.Add(nowPos + new Vector2Int(0, -1));
        targetVector.Add(nowPos + new Vector2Int(+1, 0));
        targetVector.Add(nowPos + new Vector2Int(-1, 0));

        for (int i = 0; i < targetVector.Count; i++)
        {
            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(targetVector[i]))
                continue;

            TestTile nowTile = TestManager.Instance.testTileList[targetVector[i].x, targetVector[i].y];
            SetIsColorAttack(nowTile);
            // 2. �ش� ��ġ�� ���ݴ��ϴ� ��ġ�� ��� �Ѿ
            if (pieceColor == GameColor.White)
            {
                if (nowTile.isBlackAttack)
                    continue;
                else if (nowTile.isBlackBlockAttack)
                    continue;
            }
            else if (pieceColor == GameColor.Black)
            {
                if (nowTile.isWhiteAttack)
                    continue;
                else if (nowTile.isWhiteBlockAttack)
                    continue;
            }

            // 3. �ش� �⹰��ġ�� �⹰�� ������ Ȯ��
            if (nowTile.locatedPiece != null)
            {
                // 3-1. �ش� Ÿ�� �⹰�� �� == ������ �⹰�� ���̸� �Ѿ
                if (nowTile.locatedPiece.pieceColor == pieceColor)
                    continue;
                else
                {
                    // 3-2. �ش� ��ġ�� �̵����� �� ŷ�� ���ݴ����� �ʴ´ٸ� ���� �⹰ �߰� �� �̵� Ÿ�� �߰�
                    //attackPieceList.Add(nowTile.locatedPiece);
                    movableTIleList.Add(nowTile);
                    SetIsColorAttack(nowTile);
                }
            }
            else
            {
                // 3. �ش� Ÿ�Ͽ� �⹰�� �������� �ʾ��� ���
                movableTIleList.Add(nowTile);
                SetIsColorAttack(nowTile);
            }
        }
    }
}
