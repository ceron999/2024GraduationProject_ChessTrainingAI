using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    int direction;
    public bool isFirstMove = true;                             //ó�� 2ĭ�� �̵��Ͽ����� Ȯ���ϴ� ����
    public bool isCanEnPassant = false;                        //���Ļ����� �ǰݵ� �� �ִ°�?

    public override void EvaluateMove()
    {
        // ��� : row�� �����ϴ� ���� <-> ������ : row�� �����ϴ� ������ ����.
        direction = (pieceColor == GameColor.White) ? 1 : -1;

        Tile nowTile = null;
        Vector2Int forward1Pos = new Vector2Int(nowPos.x, nowPos.y + direction);
        Vector2Int forward2Pos = new Vector2Int(nowPos.x, nowPos.y + direction * 2);

        Vector2Int attact1Pos = new Vector2Int(nowPos.x - direction, nowPos.y + direction);    // ���� �밢��
        Vector2Int attact2Pos = new Vector2Int(nowPos.x + direction, nowPos.y + direction);    // ���� �밢��

        // 1. �̵� Ÿ�� Ȯ��
        //���� 2ĭ �̵� ����?
        if (isFirstMove && ChessManager.instance.chessTileList[forward1Pos.x, forward1Pos.y].locatedPiece == null)
        {
            nowTile = ChessManager.instance.chessTileList[forward2Pos.x, forward2Pos.y];

            //�ش� ��ġ�� �⹰�� �������� ������ movableTIles�� �߰�
            if (nowTile.locatedPiece == null)
                movableTIleList.Add(nowTile);
        }

        //���� 1ĭ �̵� ����?
        if (IsAvailableTIle(forward1Pos))
        {
            nowTile = ChessManager.instance.chessTileList[forward1Pos.x, forward1Pos.y];

            //�ش� ��ġ�� �⹰�� �������� ������ movableTIles�� �߰�
            if (nowTile.locatedPiece == null)
                movableTIleList.Add(nowTile);
        }

        // 2. ���� �⹰ Ȯ��
        // ���� �밢�� 1ĭ ���� ����?
        if (IsAvailableTIle(attact1Pos))
        {
            nowTile = ChessManager.instance.chessTileList[attact1Pos.x, attact1Pos.y];

            //�ش� ��ġ�� �⹰ �� != ���õ� �⹰�� �� -> �߰�
            if (nowTile.locatedPiece != null)
            {
                if (nowTile.locatedPiece.pieceColor != pieceColor)
                {
                    movableTIleList.Add(nowTile);
                    SetIsColorAttack(nowTile);
                    attackPieceList.Add(nowTile.locatedPiece);
                }
            }
        }

        // ���� �밢�� 1ĭ ���� ����?
        if (IsAvailableTIle(attact2Pos))
        {
            nowTile = ChessManager.instance.chessTileList[attact2Pos.x, attact2Pos.y];

            //�ش� ��ġ�� �⹰ �� != ���õ� �⹰�� �� -> �߰�
            if (nowTile.locatedPiece != null)
            {
                if (nowTile.locatedPiece.pieceColor != pieceColor)
                {
                    movableTIleList.Add(nowTile);
                    SetIsColorAttack(nowTile);
                    attackPieceList.Add(nowTile.locatedPiece);
                }
            }
        }
    }

    public bool Promotion(Tile getTile)
    {
        return false;
    }

    public override void TestMove()
    {
        // ��� : row�� �����ϴ� ���� <-> ������ : row�� �����ϴ� ������ ����.
        direction = (pieceColor == GameColor.White) ? 1 : -1;

        Tile nowTile = null;

        Vector2Int forward1Pos = new Vector2Int(nowPos.x, nowPos.y + direction);
        Vector2Int forward2Pos = new Vector2Int(nowPos.x, nowPos.y + direction * 2);

        Vector2Int attact1Pos = new Vector2Int(nowPos.x - direction, nowPos.y + direction);    // ���� �밢��
        Vector2Int attact2Pos = new Vector2Int(nowPos.x + direction, nowPos.y + direction);    // ���� �밢��

        // 1. �̵� Ÿ�� Ȯ��
        //���� 2ĭ �̵� ����?
        if (isFirstMove && ChessManager.instance.chessTileList[forward1Pos.x, forward1Pos.y].locatedPiece == null)
        {
            nowTile = ChessManager.instance.chessTileList[forward2Pos.x, forward2Pos.y];

        }

        //���� 1ĭ �̵� ����?
        if (IsAvailableTIle(forward1Pos))
        {
            nowTile = ChessManager.instance.chessTileList[forward1Pos.x, forward1Pos.y];

        }

        // 2. ���� �⹰ Ȯ��
        // ���� �밢�� 1ĭ ���� ����?
        if (IsAvailableTIle(attact1Pos))
        {
            nowTile = ChessManager.instance.chessTileList[attact1Pos.x, attact1Pos.y];

            
        }

        // ���� �밢�� 1ĭ ���� ����?
        if (IsAvailableTIle(attact2Pos))
        {
            
        }
    }
}
