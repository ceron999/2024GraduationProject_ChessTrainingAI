using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPawn : TestPiece
{
    public override void SetAttackPieceList()
    {
        base.SetAttackPieceList();

        // ��� : row�� �����ϴ� ���� <-> ������ : row�� �����ϴ� ������ ����.
        int direction = (pieceColor == GameColor.White) ? 1 : -1;

        TestTile nowTile = null;

        Vector2Int attact1Pos = new Vector2Int(nowPos.x - direction, nowPos.y + direction);    // ���� �밢��
        Vector2Int attact2Pos = new Vector2Int(nowPos.x + direction, nowPos.y + direction);    // ���� �밢��


        // 2. ���� �⹰ Ȯ��
        // ���� �밢�� 1ĭ ���� ����?
        if (IsAvailableTIle(attact1Pos))
        {
            nowTile = TestManager.Instance.testTileList[attact1Pos.x, attact1Pos.y];

            //�ش� ��ġ�� �⹰ �� != ���õ� �⹰�� �� -> �߰�
            if (nowTile.locatedPiece != null)
            {
                if (nowTile.locatedPiece.pieceColor != pieceColor)
                {
                    attackPieceList.Add(nowTile.locatedPiece);
                }
            }
        }

        // ���� �밢�� 1ĭ ���� ����?
        if (IsAvailableTIle(attact2Pos))
        {
            nowTile = TestManager.Instance.testTileList[attact2Pos.x, attact2Pos.y];

            //�ش� ��ġ�� �⹰ �� != ���õ� �⹰�� �� -> �߰�
            if (nowTile.locatedPiece != null)
            {
                if (nowTile.locatedPiece.pieceColor != pieceColor)
                {
                    attackPieceList.Add(nowTile.locatedPiece);
                }
            }
        }
    }
}
