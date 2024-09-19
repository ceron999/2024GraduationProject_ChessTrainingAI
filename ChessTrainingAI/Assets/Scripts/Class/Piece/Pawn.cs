using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    int direction;
    public bool isFirstMove = true;                             //처음 2칸을 이동하였는지 확인하는 변수
    public bool isCanEnPassant = false;                        //앙파상으로 피격될 수 있는가?

    public override void EvaluateMove()
    {
        // 흰색 : row가 증가하는 방향 <-> 검은색 : row가 감소하는 방향이 앞임.
        direction = (pieceColor == GameColor.White) ? 1 : -1;

        Tile nowTile = null;
        Vector2Int forward1Pos = new Vector2Int(nowPos.x, nowPos.y + direction);
        Vector2Int forward2Pos = new Vector2Int(nowPos.x, nowPos.y + direction * 2);

        Vector2Int attact1Pos = new Vector2Int(nowPos.x - direction, nowPos.y + direction);    // 좌측 대각선
        Vector2Int attact2Pos = new Vector2Int(nowPos.x + direction, nowPos.y + direction);    // 우측 대각선

        // 1. 이동 타일 확인
        //전방 2칸 이동 가능?
        if (isFirstMove && ChessManager.instance.chessTileList[forward1Pos.x, forward1Pos.y].locatedPiece == null)
        {
            nowTile = ChessManager.instance.chessTileList[forward2Pos.x, forward2Pos.y];

            //해당 위치에 기물이 존재하지 않으면 movableTIles에 추가
            if (nowTile.locatedPiece == null)
                movableTIleList.Add(nowTile);
        }

        //전방 1칸 이동 가능?
        if (IsAvailableTIle(forward1Pos))
        {
            nowTile = ChessManager.instance.chessTileList[forward1Pos.x, forward1Pos.y];

            //해당 위치에 기물이 존재하지 않으면 movableTIles에 추가
            if (nowTile.locatedPiece == null)
                movableTIleList.Add(nowTile);
        }

        // 2. 공격 기물 확인
        // 좌측 대각선 1칸 공격 가능?
        if (IsAvailableTIle(attact1Pos))
        {
            nowTile = ChessManager.instance.chessTileList[attact1Pos.x, attact1Pos.y];

            //해당 위치의 기물 색 != 선택된 기물의 색 -> 추가
            if (nowTile.locatedPiece != null)
            {
                if (nowTile.locatedPiece.pieceColor != pieceColor)
                {
                    movableTIleList.Add(nowTile);
                    SetIsColorAttack(nowTile);
                    attackPieceList.Add(nowTile.locatedPiece);
                }
            }
        }

        // 좌측 대각선 1칸 공격 가능?
        if (IsAvailableTIle(attact2Pos))
        {
            nowTile = ChessManager.instance.chessTileList[attact2Pos.x, attact2Pos.y];

            //해당 위치의 기물 색 != 선택된 기물의 색 -> 추가
            if (nowTile.locatedPiece != null)
            {
                if (nowTile.locatedPiece.pieceColor != pieceColor)
                {
                    movableTIleList.Add(nowTile);
                    SetIsColorAttack(nowTile);
                    attackPieceList.Add(nowTile.locatedPiece);
                }
            }
        }
    }

    public bool Promotion(Tile getTile)
    {
        return false;
    }

    public override void TestMove()
    {
        // 흰색 : row가 증가하는 방향 <-> 검은색 : row가 감소하는 방향이 앞임.
        direction = (pieceColor == GameColor.White) ? 1 : -1;

        Tile nowTile = null;

        Vector2Int forward1Pos = new Vector2Int(nowPos.x, nowPos.y + direction);
        Vector2Int forward2Pos = new Vector2Int(nowPos.x, nowPos.y + direction * 2);

        Vector2Int attact1Pos = new Vector2Int(nowPos.x - direction, nowPos.y + direction);    // 좌측 대각선
        Vector2Int attact2Pos = new Vector2Int(nowPos.x + direction, nowPos.y + direction);    // 우측 대각선

        // 1. 이동 타일 확인
        //전방 2칸 이동 가능?
        if (isFirstMove && ChessManager.instance.chessTileList[forward1Pos.x, forward1Pos.y].locatedPiece == null)
        {
            nowTile = ChessManager.instance.chessTileList[forward2Pos.x, forward2Pos.y];

        }

        //전방 1칸 이동 가능?
        if (IsAvailableTIle(forward1Pos))
        {
            nowTile = ChessManager.instance.chessTileList[forward1Pos.x, forward1Pos.y];

        }

        // 2. 공격 기물 확인
        // 좌측 대각선 1칸 공격 가능?
        if (IsAvailableTIle(attact1Pos))
        {
            nowTile = ChessManager.instance.chessTileList[attact1Pos.x, attact1Pos.y];

            
        }

        // 좌측 대각선 1칸 공격 가능?
        if (IsAvailableTIle(attact2Pos))
        {
            
        }
    }
}
