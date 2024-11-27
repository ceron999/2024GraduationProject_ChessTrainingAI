using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPawn : TestPiece
{
    int direction = 0;
    bool isFirstMove = true;

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

    public override void SetMovableTileList()
    {
        base.SetMovableTileList();

        // ��� : row�� �����ϴ� ���� <-> ������ : row�� �����ϴ� ������ ����.
        direction = (pieceColor == GameColor.White) ? 1 : -1;

        // isFIrstMove �����ϱ�
        if (this.pieceColor == GameColor.White)
        {
            if (this.nowPos.y > 1) isFirstMove = false;
        }
        else if (this.pieceColor == GameColor.Black)
        { 
            if (this.nowPos.y < 6) isFirstMove = false;
        }

        TestTile nowTile = null;
        Vector2Int forward1Pos = new Vector2Int(nowPos.x, nowPos.y + direction);
        Vector2Int forward2Pos = new Vector2Int(nowPos.x, nowPos.y + direction * 2);

        Vector2Int attact1Pos = new Vector2Int(nowPos.x - direction, nowPos.y + direction);    // ���� �밢��
        Vector2Int attact2Pos = new Vector2Int(nowPos.x + direction, nowPos.y + direction);    // ���� �밢��

        // 1. �̵� Ÿ�� Ȯ��
        //���� 2ĭ �̵� ����?
        if (isFirstMove && TestManager.Instance.testTileList[forward1Pos.x, forward1Pos.y].locatedPiece == null)
        {
            nowTile = TestManager.Instance.testTileList[forward2Pos.x, forward2Pos.y];

            //�ش� ��ġ�� �⹰�� �������� ������ movableTIles�� �߰�
            if (nowTile.locatedPiece == null)
                movableTIleList.Add(nowTile);
        }

        //���� 1ĭ �̵� ����?
        if (IsAvailableTIle(forward1Pos))
        {
            nowTile = TestManager.Instance.testTileList[forward1Pos.x, forward1Pos.y];

            //�ش� ��ġ�� �⹰�� �������� ������ movableTIles�� �߰�
            if (nowTile.locatedPiece == null)
                movableTIleList.Add(nowTile);
        }

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
                    movableTIleList.Add(nowTile);
                }
            }
            SetIsColorAttack(nowTile);
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
                    movableTIleList.Add(nowTile);
                }
            }
            SetIsColorAttack(nowTile);
        }
    }
}
