using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    int direction;
    public bool isFirstMove = true;                             //ó�� 2ĭ�� �̵��Ͽ����� Ȯ���ϴ� ����
    public bool isEnPassant = false;                    //���Ļ����� �ǰݵ� �� �ִ°�?

    public override void FindMovableMoveTiles()
    {
        //��� : row�� �����ϴ� ���� <-> ������ : row�� �����ϴ� ������ ����.
        direction = (pieceColor == GameColor.White) ? 1 : -1;

        //1.���� ���� Ȯ��
        EvaluateMoveTiles(direction);

        //2. ���� Ȯ��
        EvaluateAttackTiles(direction);

        SetMovablePiecesSelected();
        //DebugMovableTiles(movableTIles);
    }

    public override void SetAttackTile(bool isActive)
    {
        if (isActive)
        {
            Tile nowTile = null;
            Vector2Int attack1Pos = new Vector2Int(nowPos.x - 1, nowPos.y + direction);
            Vector2Int attack2Pos = new Vector2Int(nowPos.x + 1, nowPos.y + direction);

            //1. �밢���� �� �⹰�� �����ϴ°�?
            if (IsAvailableTIle(attack1Pos))
            {
                nowTile = ChessManager.chessManager.chessTileList[attack1Pos.x, attack1Pos.y];
                if (nowTile.nowLocateColor != pieceColor && nowTile.nowLocateColor != GameColor.Null)
                {
                    attackTIles.Add(nowTile);
                }
            }
            if (IsAvailableTIle(attack2Pos))
            {
                nowTile = ChessManager.chessManager.chessTileList[attack2Pos.x, attack2Pos.y];
                if (nowTile.nowLocateColor != pieceColor && nowTile.nowLocateColor != GameColor.Null)
                {
                    attackTIles.Add(nowTile);
                }
            }
        }

        //���� isActive�� ���̸� �ش� ���� Ÿ���� �缳��
        //isActive�� �����̸� ���� ���� Ÿ�� false�� ����� ������ �غ�
        for(int i =0;i<attackTIles.Count;i++)
        {
            attackTIles[i].isAttackedTile = isActive;
        }
    }

    void EvaluateMoveTiles(int getDir)
    {
        Tile nowTile = null;
        Vector2Int forward1Pos = new Vector2Int(nowPos.x, nowPos.y + getDir);
        Vector2Int forward2Pos = new Vector2Int(nowPos.x, nowPos.y + getDir * 2);

        //���� 2ĭ �̵� ����?
        if (isFirstMove)
        {
            nowTile = ChessManager.chessManager.chessTileList[forward2Pos.x, forward2Pos.y];

            //�ش� ��ġ�� �⹰�� �������� ������ movableTIles�� �߰�
            if (nowTile.nowLocateColor == GameColor.Null)
                movableTIles.Add(nowTile);
        }

        //���� 1ĭ �̵�����?
        if (IsAvailableTIle(forward1Pos))
        {
            nowTile = ChessManager.chessManager.chessTileList[forward1Pos.x, forward1Pos.y];

            //�ش� ��ġ�� �⹰�� �������� ������ movableTIles�� �߰�
            if (nowTile.nowLocateColor == GameColor.Null)
                movableTIles.Add(nowTile);
        }
    }

    void EvaluateAttackTiles(int getDir)
    {
        Tile nowTile = null;
        Vector2Int attack1Pos = new Vector2Int(nowPos.x - 1, nowPos.y + getDir);
        Vector2Int attack2Pos = new Vector2Int(nowPos.x + 1, nowPos.y + getDir);

        //1. �밢���� �� �⹰�� �����ϴ°�?
        if(IsAvailableTIle(attack1Pos))
        {
            nowTile = ChessManager.chessManager.chessTileList[attack1Pos.x, attack1Pos.y];
            if(nowTile.nowLocateColor != pieceColor && nowTile.nowLocateColor != GameColor.Null)
            {
                movableTIles.Add(nowTile);
            }
        }
        if(IsAvailableTIle(attack2Pos))
        {
            nowTile = ChessManager.chessManager.chessTileList[attack2Pos.x, attack2Pos.y];
            if (nowTile.nowLocateColor != pieceColor && nowTile.nowLocateColor != GameColor.Null)
            {
                movableTIles.Add(nowTile);
            }
        }

        //2. ���Ļ��ΰ�?
    }

    //Pawn�� ������ row�� �����ϸ� �°��Ѵ�.
    public void PromotionPawn()
    {

    }
}
