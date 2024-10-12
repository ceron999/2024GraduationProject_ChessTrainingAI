using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    //(-1.+1)
    public override void EvaluateMove()
    {
        List<Vector2Int> targetVector = new List<Vector2Int>();
        targetVector.Add(nowPos + new Vector2Int(-2, +1));
        targetVector.Add(nowPos + new Vector2Int(-2, -1));
        targetVector.Add(nowPos + new Vector2Int(+2, +1));
        targetVector.Add(nowPos + new Vector2Int(+2, -1));
        targetVector.Add(nowPos + new Vector2Int(-1, +2));
        targetVector.Add(nowPos + new Vector2Int(-1, -2));
        targetVector.Add(nowPos + new Vector2Int(+1, +2));
        targetVector.Add(nowPos + new Vector2Int(+1, -2));

        for (int i = 0; i < targetVector.Count; i++)
        {
            // 1. �ش��ϴ� Ÿ���� �������� ������ �Ѿ
            if (!IsAvailableTIle(targetVector[i]))
                continue;

            SetIsColorAttack(ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y]);

            // 2. �ش��ϴ� Ÿ���� �⹰�� ������ �߰���
            if (ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y].locatedPiece == null)
            {
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y]);
                continue;
            }

            // 3. �ش��ϴ� Ÿ���� �⹰ �� != ������ �⹰�� ���̸� ���� �⹰ �߰�, �̵� Ÿ�� �߰�
            if (ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y].locatedPiece.pieceColor != pieceColor)
            {
                attackPieceList.Add(ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y].locatedPiece);
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y]);
                continue;
            }

            // 4. �ش��ϴ� Ÿ���� �⹰ �� == ������ �⹰�� ���̸� �Ѿ
            if (ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y].locatedPiece.pieceColor == pieceColor)
                continue;
        }
    }
}
