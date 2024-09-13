using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public override void FindMovableTiles()
    {
        EvaluateMoveTiles();
    }

    public override void SetAttackTile()
    {
        //1. ���� ���� Ÿ�Ϸ� ������ Ÿ�ϵ��� ���� ����Ѵ�. 
        if (attackTIles.Count >= 0)
        {
            for (int i = 0; i < attackTIles.Count; i++)
            {
                attackTIles[i].isAttackedTile = false;
            }
            attackTIles.Clear();
        }

        //2. ���� Ÿ�� ����
        EvaluateAttackTile();

        //3. ������ ���� Ÿ���� ���� �缳��
        for (int i = 0; i < attackTIles.Count; i++)
        {
            attackTIles[i].isAttackedTile = true;
        }
    }

    //(-1.+1)
    void EvaluateMoveTiles()
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
            //�ش��ϴ� Ÿ���� �������� ������ return;
            if (!IsAvailableTIle(evaluateVector[i]))
                continue;

            if (ChessManager.chessManager.chessTileList[evaluateVector[i].x, evaluateVector[i].y].nowLocateColor == pieceColor)
                continue;
            else 
                movableTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector[i].x, evaluateVector[i].y]);
        }


        SetMovablePiecesSelected();
    }

    void EvaluateAttackTile()
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

            //�ش��ϴ� Ÿ���� �������� ������ return;
            if (!IsAvailableTIle(evaluateVector[i]))
                continue;

            if (ChessManager.chessManager.chessTileList[evaluateVector[i].x, evaluateVector[i].y].nowLocateColor == pieceColor)
                continue;
            else
                attackTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector[i].x, evaluateVector[i].y]);
        }
    }
}
