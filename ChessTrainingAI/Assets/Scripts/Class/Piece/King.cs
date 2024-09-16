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

        List<Vector2Int> evaluateVector = new List<Vector2Int>();
        evaluateVector.Add(nowPos + new Vector2Int(-1, +1));
        evaluateVector.Add(nowPos + new Vector2Int(-1, -1));
        evaluateVector.Add(nowPos + new Vector2Int(+1, +1));
        evaluateVector.Add(nowPos + new Vector2Int(+1, -1));
        evaluateVector.Add(nowPos + new Vector2Int(0, +1));
        evaluateVector.Add(nowPos + new Vector2Int(0, -1));
        evaluateVector.Add(nowPos + new Vector2Int(+1, 0));
        evaluateVector.Add(nowPos + new Vector2Int(-1, 0));

        for (int i = 0; i < evaluateVector.Count; i++)
        {
            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(evaluateVector[i]))
                continue;

            // 2. �ش� �⹰��ġ�� �⹰�� ������ Ȯ��
            if(ChessManager.instance.chessTileList[evaluateVector[i].x, evaluateVector[i].y].locatedPiece != null)
            {
                // 2-1. �ش� Ÿ�� �⹰�� �� == ������ �⹰�� ���̸� �Ѿ
                if (ChessManager.instance.chessTileList[evaluateVector[i].x, evaluateVector[i].y].locatedPiece.pieceColor == pieceColor)
                    continue;

                else
                {
                    // 2-2. �ش� ��ġ�� �̵����� �� ŷ�� ���ݴ��ϸ� �Ѿ
                    if (ChessManager.instance.isCheck)
                        continue;

                    // 2-3. �ش� ��ġ�� �̵����� �� ŷ�� ���ݴ����� �ʴ´ٸ� ���� �⹰ �߰� �� �̵� Ÿ�� �߰�
                    else
                    {
                        attackPieceList.Add(ChessManager.instance.chessTileList[evaluateVector[i].x, evaluateVector[i].y].locatedPiece);
                        movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector[i].x, evaluateVector[i].y]);
                    }
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
                    movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector[i].x, evaluateVector[i].y]);
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

        List<Vector2Int> evaluateRightVector = new List<Vector2Int>();

        evaluateRightVector.Add(nowPos + new Vector2Int(1, 0));
        evaluateRightVector.Add(nowPos + new Vector2Int(2, 0)); 

        for (int i =0; i< evaluateRightVector.Count; i++)
        {
            // 2. ���� ŷ�� �� ���̿� �⹰�� �����ϸ� ĳ���� �Ұ����ϹǷ� ��������
            if (ChessManager.instance.chessTileList[evaluateRightVector[i].x, evaluateRightVector[i].y].locatedPiece != null)
                break;
            
            // 3. ������ Rook ó�� �����̴��� Ȯ��
            if (i == evaluateRightVector.Count - 1)
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

        List<Vector2Int> evaluateLeftVector = new List<Vector2Int>();

        evaluateLeftVector.Add(nowPos + new Vector2Int(-1, 0));
        evaluateLeftVector.Add(nowPos + new Vector2Int(-2, 0));
        evaluateLeftVector.Add(nowPos + new Vector2Int(-3, 0));

        for (int i = 0; i < evaluateLeftVector.Count; i++)
        {
            // 2. ���� ŷ�� �� ���̿� �⹰�� �����ϸ� ĳ���� �Ұ����ϹǷ� ��������
            if (ChessManager.instance.chessTileList[evaluateLeftVector[i].x, evaluateLeftVector[i].y].locatedPiece != null)
                break;

            // 3. ������ Rook ó�� �����̴��� Ȯ��
            if (i == evaluateLeftVector.Count - 1)
            {
                if (nowRooks[0].isFirstMove)
                    movableTIleList.Add(ChessManager.instance.chessTileList[2, nowPos.y]);
            }
        }

        return;
    }
}
