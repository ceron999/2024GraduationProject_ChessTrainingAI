using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    int direction;
    public bool isFirstMove = true;                             //처음 2칸을 이동하였는지 확인하는 변수
    public bool isEnPassant = false;                    //앙파상으로 피격될 수 있는가?

    public override void FindMovableMoveTiles()
    {
        //흰색 : row가 증가하는 방향 <-> 검은색 : row가 감소하는 방향이 앞임.
        direction = (pieceColor == GameColor.White) ? 1 : -1;

        //1.전진 방향 확인
        EvaluateMoveTiles(direction);

        //2. 공격 확인
        EvaluateAttackTiles(direction);

        SetMovablePiecesSelected();
        //DebugMovableTiles(movableTIles);
    }

    public override void SetAttackTile(bool isActive)
    {
        if (isActive)
        {
            Tile nowTile = null;
            Vector2Int attack1Pos = new Vector2Int(nowPos.x - 1, nowPos.y + direction);
            Vector2Int attack2Pos = new Vector2Int(nowPos.x + 1, nowPos.y + direction);

            //1. 대각선에 적 기물이 존재하는가?
            if (IsAvailableTIle(attack1Pos))
            {
                nowTile = ChessManager.chessManager.chessTileList[attack1Pos.x, attack1Pos.y];
                if (nowTile.nowLocateColor != pieceColor && nowTile.nowLocateColor != GameColor.Null)
                {
                    attackTIles.Add(nowTile);
                }
            }
            if (IsAvailableTIle(attack2Pos))
            {
                nowTile = ChessManager.chessManager.chessTileList[attack2Pos.x, attack2Pos.y];
                if (nowTile.nowLocateColor != pieceColor && nowTile.nowLocateColor != GameColor.Null)
                {
                    attackTIles.Add(nowTile);
                }
            }
        }

        //만약 isActive가 참이면 해당 공격 타일을 재설정
        //isActive가 거짓이면 현재 공격 타일 false로 만들어 움직일 준비
        for(int i =0;i<attackTIles.Count;i++)
        {
            attackTIles[i].isAttackedTile = isActive;
        }
    }

    void EvaluateMoveTiles(int getDir)
    {
        Tile nowTile = null;
        Vector2Int forward1Pos = new Vector2Int(nowPos.x, nowPos.y + getDir);
        Vector2Int forward2Pos = new Vector2Int(nowPos.x, nowPos.y + getDir * 2);

        //전방 2칸 이동 가능?
        if (isFirstMove)
        {
            nowTile = ChessManager.chessManager.chessTileList[forward2Pos.x, forward2Pos.y];

            //해당 위치에 기물이 존재하지 않으면 movableTIles에 추가
            if (nowTile.nowLocateColor == GameColor.Null)
                movableTIles.Add(nowTile);
        }

        //전방 1칸 이동가능?
        if (IsAvailableTIle(forward1Pos))
        {
            nowTile = ChessManager.chessManager.chessTileList[forward1Pos.x, forward1Pos.y];

            //해당 위치에 기물이 존재하지 않으면 movableTIles에 추가
            if (nowTile.nowLocateColor == GameColor.Null)
                movableTIles.Add(nowTile);
        }
    }

    void EvaluateAttackTiles(int getDir)
    {
        Tile nowTile = null;
        Vector2Int attack1Pos = new Vector2Int(nowPos.x - 1, nowPos.y + getDir);
        Vector2Int attack2Pos = new Vector2Int(nowPos.x + 1, nowPos.y + getDir);

        //1. 대각선에 적 기물이 존재하는가?
        if(IsAvailableTIle(attack1Pos))
        {
            nowTile = ChessManager.chessManager.chessTileList[attack1Pos.x, attack1Pos.y];
            if(nowTile.nowLocateColor != pieceColor && nowTile.nowLocateColor != GameColor.Null)
            {
                movableTIles.Add(nowTile);
            }
        }
        if(IsAvailableTIle(attack2Pos))
        {
            nowTile = ChessManager.chessManager.chessTileList[attack2Pos.x, attack2Pos.y];
            if (nowTile.nowLocateColor != pieceColor && nowTile.nowLocateColor != GameColor.Null)
            {
                movableTIles.Add(nowTile);
            }
        }

        //2. 앙파상인가?
    }

    //Pawn이 마지막 row에 도착하면 승격한다.
    public void PromotionPawn()
    {

    }
}
