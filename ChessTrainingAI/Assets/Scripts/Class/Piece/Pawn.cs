using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override List<Tile> FindMovableMoveTiles()
    {
        //��� : row�� �����ϴ� ���� <-> ������ : row�� �����ϴ� ������ ����.
        int direction = (ChessManager.chessManager.playerColor == PlayerColor.White) ? 1 : -1;
        Tile evaluatedTile = null;      //�ش� Ÿ�� ���ϱ� ���� ����

        //1.���� ���� 1ĭ
        evaluatedTile = ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + direction];
        if (!evaluatedTile.isSelectedMovalleTile)
        {
            movableTIles.Add(evaluatedTile);
            ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + direction].isSelectedMovalleTile = true;
        }

        //2.���� ���� 2ĭ(����϶�)
        evaluatedTile = ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + 2 * direction];
        if (ChessManager.chessManager.playerColor == PlayerColor.White && nowPos.y != 6)
        {
            if (!evaluatedTile.isSelectedMovalleTile)
            {
                movableTIles.Add(evaluatedTile);
                ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + 2 * direction].isSelectedMovalleTile = true;
            }
        }

        //3.���� ���� 2ĭ(�������϶�)
        else if (ChessManager.chessManager.playerColor == PlayerColor.Black && nowPos.y != 1)
            if (!evaluatedTile.isSelectedMovalleTile)
            {
                movableTIles.Add(evaluatedTile);
                ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + 2 * direction].isSelectedMovalleTile = true;
            }

        //4, �밢�� ���⿡ ���� ������ ��


        DebugMovableTiles(movableTIles);

        return movableTIles;
    }

    //Pawn�� ������ row�� �����ϸ� �°��Ѵ�.
    public void PromotionPawn()
    {

    }

    void DebugMovableTiles(List<Tile> getMovableTileList)
    {
        for (int i = 0; i < getMovableTileList.Count; i++)
            Debug.Log(getMovableTileList[i].transform.position);
    }
}
