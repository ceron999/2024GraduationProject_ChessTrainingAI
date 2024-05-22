using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override List<Tile> FindMovableMoveTiles()
    {
        //흰색 : row가 증가하는 방향 <-> 검은색 : row가 감소하는 방향이 앞임.
        int direction = (ChessManager.chessManager.playerColor == PlayerColor.White) ? 1 : -1;
        Tile evaluatedTile = null;      //해당 타일 평가하기 위해 설정

        //1.전진 방향 1칸
        evaluatedTile = ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + direction];
        if (!evaluatedTile.isSelectedMovalleTile)
        {
            movableTIles.Add(evaluatedTile);
            ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + direction].isSelectedMovalleTile = true;
        }

        //2.전진 방향 2칸(흰색일때)
        evaluatedTile = ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + 2 * direction];
        if (ChessManager.chessManager.playerColor == PlayerColor.White && nowPos.y != 6)
        {
            if (!evaluatedTile.isSelectedMovalleTile)
            {
                movableTIles.Add(evaluatedTile);
                ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + 2 * direction].isSelectedMovalleTile = true;
            }
        }

        //3.전진 방향 2칸(검은색일때)
        else if (ChessManager.chessManager.playerColor == PlayerColor.Black && nowPos.y != 1)
            if (!evaluatedTile.isSelectedMovalleTile)
            {
                movableTIles.Add(evaluatedTile);
                ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y + 2 * direction].isSelectedMovalleTile = true;
            }

        //4, 대각선 방향에 적이 존재할 때


        DebugMovableTiles(movableTIles);

        return movableTIles;
    }

    //Pawn이 마지막 row에 도착하면 승격한다.
    public void PromotionPawn()
    {

    }

    void DebugMovableTiles(List<Tile> getMovableTileList)
    {
        for (int i = 0; i < getMovableTileList.Count; i++)
            Debug.Log(getMovableTileList[i].transform.position);
    }
}
