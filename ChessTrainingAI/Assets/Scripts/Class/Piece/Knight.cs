using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    //(-1.+1)
    public override void EvaluateMove()
    {
        List<Vector2Int> evaluateVector = new List<Vector2Int>();
        evaluateVector.Add(nowPos + new Vector2Int(-2, +1));
        evaluateVector.Add(nowPos + new Vector2Int(-2, -1));
        evaluateVector.Add(nowPos + new Vector2Int(+2, +1));
        evaluateVector.Add(nowPos + new Vector2Int(+2, -1));
        evaluateVector.Add(nowPos + new Vector2Int(-1, +2));
        evaluateVector.Add(nowPos + new Vector2Int(-1, -2));
        evaluateVector.Add(nowPos + new Vector2Int(+1, +2));
        evaluateVector.Add(nowPos + new Vector2Int(+1, -2));

        for (int i = 0; i < evaluateVector.Count; i++)
        {
            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(evaluateVector[i]))
                continue;

            // 2. �ش��ϴ� Ÿ���� �⹰�� ������ �߰���
            if (ChessManager.instance.chessTileList[evaluateVector[i].x, evaluateVector[i].y].locatedPiece == null)
            {
                movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector[i].x, evaluateVector[i].y]);
                continue;
            }

            // 3. �ش��ϴ� Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
            if (ChessManager.instance.chessTileList[evaluateVector[i].x, evaluateVector[i].y].locatedPiece.pieceColor != pieceColor)
            {
                attackPieceList.Add(ChessManager.instance.chessTileList[evaluateVector[i].x, evaluateVector[i].y].locatedPiece);
                movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector[i].x, evaluateVector[i].y]);
                continue;
            }

            // 4. �ش��ϴ� Ÿ���� �⹰ �� == ������ �⹰�� ���̸� �Ѿ
            if (ChessManager.instance.chessTileList[evaluateVector[i].x, evaluateVector[i].y].locatedPiece.pieceColor == pieceColor)
                continue;
        }
    }
}
