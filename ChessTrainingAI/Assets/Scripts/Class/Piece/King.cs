using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    //castling ����
    public bool isFirstMove = true;
    public Rook[] nowRooks = new Rook[2];

    public override void EvaluateMove()
    {
        if (isFirstMove)
        {
            EvaluateCastling();
        }

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

            Tile nowTile = ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y];
            
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
                else if (nowTile.isWhiteBolckAttack)
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
                    attackPieceList.Add(nowTile.locatedPiece);
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
    void EvaluateCastling()
    {
        // üũ ���ϸ� ĳ���� �Ұ�
        if (ChessManager.instance.isCheck)
            return;

        EvaluateKingSideCastling();
        EvaluateQueenSideCastling();
    }

    void EvaluateKingSideCastling()
    {
        // 1. ŷ üũ ���¸� ĳ���� �Ұ�
        if (ChessManager.instance.isCheck)
            return;

        List<Vector2Int> targetVector = new List<Vector2Int>();

        targetVector.Add(nowPos + new Vector2Int(1, 0));
        targetVector.Add(nowPos + new Vector2Int(2, 0)); 

        for (int i =0; i< targetVector.Count; i++)
        {
            // 2. ���� ŷ�� �� ���̿� �⹰�� �����ϸ� ĳ���� �Ұ����ϹǷ� ��������
            if (ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y].locatedPiece != null)
                break;
            
            // 3. ������ Rook ó�� �����̴��� Ȯ��
            if (i == targetVector.Count - 1)
            {
                if (nowRooks[1].isFirstMove)
                    movableTIleList.Add(ChessManager.instance.chessTileList[6, nowPos.y]);
            }
        }

        return;
    }

    void EvaluateQueenSideCastling()
    {
        // 1. ŷ üũ ���¸� ĳ���� �Ұ�
        if (ChessManager.instance.isCheck)
            return;

        List<Vector2Int> targetVector = new List<Vector2Int>();

        targetVector.Add(nowPos + new Vector2Int(-1, 0));
        targetVector.Add(nowPos + new Vector2Int(-2, 0));
        targetVector.Add(nowPos + new Vector2Int(-3, 0));

        for (int i = 0; i < targetVector.Count; i++)
        {
            // 2. ���� ŷ�� �� ���̿� �⹰�� �����ϸ� ĳ���� �Ұ����ϹǷ� ��������
            if (ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y].locatedPiece != null)
                break;

            // 3. ������ Rook ó�� �����̴��� Ȯ��
            if (i == targetVector.Count - 1)
            {
                if (nowRooks[0].isFirstMove)
                    movableTIleList.Add(ChessManager.instance.chessTileList[2, nowPos.y]);
            }
        }

        return;
    }

    public bool Castling(Tile getTile)
    {
        Tile nowTIle = ChessManager.instance.chessTileList[nowPos.x, nowPos.y];

        if (getTile.tileName == TIleName.a3)
        {
            // ��� �����̵� ĳ����
            // 1. ��ġ �̵�
            this.transform.position = getTile.transform.position;
            nowRooks[0].transform.position = ChessManager.instance.chessTileList[3,0].transform.position;

            // 2. �ش� Ÿ�� ���� ����
            nowTIle.locatedPiece = null;
            getTile.locatedPiece = this;
            ChessManager.instance.chessTileList[3, 0].locatedPiece = nowRooks[0];

            return true;
        }
        else if (getTile.tileName == TIleName.a7)
        {
            // ��� ŷ���̵� ĳ����
            // 1. ��ġ �̵�
            this.transform.position = getTile.transform.position;
            nowRooks[1].transform.position = ChessManager.instance.chessTileList[5, 0].transform.position;

            // 2. �ش� Ÿ�� ���� ����
            nowTIle.locatedPiece = null;
            getTile.locatedPiece = this;
            ChessManager.instance.chessTileList[5, 0].locatedPiece = nowRooks[1];

            return true;
        }
        else if (getTile.tileName == TIleName.h3)
        {
            // ������ �����̵� ĳ����
            // 1. ��ġ �̵�
            this.transform.position = getTile.transform.position;
            nowRooks[0].transform.position = ChessManager.instance.chessTileList[3, 7].transform.position;

            // 2. �ش� Ÿ�� ���� ����
            nowTIle.locatedPiece = null;
            getTile.locatedPiece = this;
            ChessManager.instance.chessTileList[3, 7].locatedPiece = nowRooks[0];

            return true;
        }
        else if (getTile.tileName == TIleName.h7)
        {
            // ������ ŷ���̵� ĳ����
            // 1. ��ġ �̵�
            this.transform.position = getTile.transform.position;
            nowRooks[1].transform.position = ChessManager.instance.chessTileList[5, 7].transform.position;

            // 2. �ش� Ÿ�� ���� ����
            nowTIle.locatedPiece = null;
            getTile.locatedPiece = this;
            ChessManager.instance.chessTileList[5, 7].locatedPiece = nowRooks[1];

            return true;
        }
        else return false;
    }
}
