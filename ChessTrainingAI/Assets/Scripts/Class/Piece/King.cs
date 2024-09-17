using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    //castling ����
    public bool isFirstMove = true;
    public Rook[] nowRooks = new Rook[2];

    public bool isCheck = false;
    public bool isCheckMate = false;

    public override void EvaluateMove()
    {
        if (isFirstMove)
        {
            EvaluateKingSideCastling();
            EvaluateQueenSideCastling();
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

            // 2. �ش� �⹰��ġ�� �⹰�� ������ Ȯ��
            if (nowTile.locatedPiece != null)
            {
                // 2-1. �ش� Ÿ�� �⹰�� �� == ������ �⹰�� ���̸� �Ѿ
                if (nowTile.locatedPiece.pieceColor == pieceColor)
                    continue;

                else
                {
                    // 2-2. �ش� ��ġ�� ���ݴ��ϴ� ��ġ�� ��� �Ѿ
                    if (pieceColor == GameColor.White)
                    {
                        if(nowTile.isBlackAttack)
                            continue;
                    }
                    else if(pieceColor == GameColor.Black)
                    {
                        if (nowTile.isWhiteAttack)
                            continue;
                    }

                    // 2-3. �ش� ��ġ�� �̵����� �� ŷ�� ���ݴ����� �ʴ´ٸ� ���� �⹰ �߰� �� �̵� Ÿ�� �߰�
                    attackPieceList.Add(nowTile.locatedPiece);
                    movableTIleList.Add(nowTile);
                    SetIsColorAttack(nowTile);
                }
            }
            else
            {
                // 3. �ش� Ÿ�Ͽ� �⹰�� �������� �ʾ��� ���
                // 3-1. �ش� ��ġ�� �̵����� �� ŷ�� ���ݴ��ϸ� �Ѿ
                if (ChessManager.instance.isCheck)
                    continue;
                
                // 2-3. �ش� ��ġ�� �̵����� �� ŷ�� ���ݴ����� �ʴ´ٸ� �̵� Ÿ�� �߰�
                else
                {
                    movableTIleList.Add(nowTile);
                    SetIsColorAttack(nowTile);
                }
            }
        }
    }
    void EvaluateCastling()
    {
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
}
