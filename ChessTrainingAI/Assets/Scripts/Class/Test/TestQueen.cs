using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQueen : TestPiece
{
    bool isSearch = true;
    int nowDir = 0;
    int count = 0;

    public override void SetAttackPieceList()
    {
        base.SetAttackPieceList();
        Vector2Int targetVector = nowPos;
        List<Vector2Int> direction = new List<Vector2Int>();
        direction.Add(new Vector2Int(-1, +1));
        direction.Add(new Vector2Int(+1, +1));
        direction.Add(new Vector2Int(-1, -1));
        direction.Add(new Vector2Int(+1, -1)); 
        direction.Add(new Vector2Int(-1, 0));
        direction.Add(new Vector2Int(+1, 0));
        direction.Add(new Vector2Int(0, -1));
        direction.Add(new Vector2Int(0, +1));


        while (isSearch)
        {
            targetVector = nowPos + direction[nowDir] * count;

            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(targetVector))
            {
                if (nowDir < 8)
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

            isSearch = true;
            InsertAttackPieces(targetVector);
        }
    }

    void InsertAttackPieces(Vector2Int getVector)
    {
        TestTile nowTIle = TestManager.Instance.testTileList[getVector.x, getVector.y];

        // 2. Ÿ���� �⹰�� ������ �̵� Ÿ�� �߰�
        if (nowTIle.locatedPiece != null)
        {
            if (nowTIle.locatedPiece.pieceColor != pieceColor)
                attackPieceList.Add(nowTIle.locatedPiece);
            else if (nowTIle.locatedPiece.pieceColor == pieceColor)
            {
                if (nowDir < 4)
                {
                    count = 0;
                    nowDir++;
                }
                else
                    isSearch = false;
            }
        }

    }
}
