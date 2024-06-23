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

    public override void FindMovableMoveTiles()
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

    void EvaluateMoveTiles()
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
            //�ش��ϴ� Ÿ���� �������� ������ return;
            if (!IsAvailableTIle(evaluateVector[i]))
                continue;

            //���ݴ��ϴ� Ÿ���� ��� continue
            if (ChessManager.chessManager.chessTileList[evaluateVector[i].x, evaluateVector[i].y].isAttackedTile)
                continue;

            if (ChessManager.chessManager.chessTileList[evaluateVector[i].x, evaluateVector[i].y].nowLocateColor == pieceColor)
                continue;

            else
                movableTIles.Add(ChessManager.chessManager.chessTileList[evaluateVector[i].x, evaluateVector[i].y]);
        }


        SetMovablePiecesSelected();
    }

    void EvaluateKingSideCastling()
    {
        List<Vector2Int> evaluateRightVector = new List<Vector2Int>();

        evaluateRightVector.Add(nowPos + new Vector2Int(1, 0));
        evaluateRightVector.Add(nowPos + new Vector2Int(2, 0)); 
        evaluateRightVector.Add(nowPos + new Vector2Int(3, 0));

        Vector2Int nowVector;
        int count = (pieceColor == GameColor.White) ? 2 : 3;

        for (int i =0; i< count; i++)
        {
            Debug.Log(" Ȯ���ϴ� Ÿ�� : " +  evaluateRightVector[i]);
            nowVector = evaluateRightVector[i];

            if (ChessManager.chessManager.chessTileList[nowVector.x, nowVector.y].nowLocateColor != GameColor.Null)
                break;

            //������ Rook ó�� �����̴��� Ȯ��
            if (i == count - 1)
            {
                if (nowRooks[1].isFirstMove)
                    movableTIles.Add(ChessManager.chessManager.chessTileList[6, nowPos.y]);
            }
        }

        return;
    }

    void EvaluateQueenSideCastling()
    {
        List<Vector2Int> evaluateLeftVector = new List<Vector2Int>();

        evaluateLeftVector.Add(nowPos + new Vector2Int(-1, 0));
        evaluateLeftVector.Add(nowPos + new Vector2Int(-2, 0));
        evaluateLeftVector.Add(nowPos + new Vector2Int(-3, 0));

        Vector2Int nowVector = evaluateLeftVector[0];

        for (int i = 0; i < evaluateLeftVector.Count; i++)
        {
            nowVector = evaluateLeftVector[i];

            if (ChessManager.chessManager.chessTileList[nowVector.x, nowVector.y].nowLocateColor != GameColor.Null)
                break;

            //������ Rook ó�� �����̴��� Ȯ��
            if (i == evaluateLeftVector.Count - 1)
            {
                if (nowRooks[0].isFirstMove)
                    movableTIles.Add(ChessManager.chessManager.chessTileList[1, nowPos.y]);
            }
        }

        return;
    }

    void EvaluateAttackTile()
    {
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
