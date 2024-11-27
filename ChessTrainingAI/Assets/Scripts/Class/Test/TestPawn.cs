using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPawn : TestPiece
{
    int direction = 0;
    bool isFirstMove = true;

    public override void SetAttackPieceList()
    {
        base.SetAttackPieceList();

        // 흰색 : row가 증가하는 방향 <-> 검은색 : row가 감소하는 방향이 앞임.
        int direction = (pieceColor == GameColor.White) ? 1 : -1;

        TestTile nowTile = null;

        Vector2Int attact1Pos = new Vector2Int(nowPos.x - direction, nowPos.y + direction);    // 좌측 대각선
        Vector2Int attact2Pos = new Vector2Int(nowPos.x + direction, nowPos.y + direction);    // 우측 대각선


        // 2. 공격 기물 확인
        // 좌측 대각선 1칸 공격 가능?
        if (IsAvailableTIle(attact1Pos))
        {
            nowTile = TestManager.Instance.testTileList[attact1Pos.x, attact1Pos.y];

            //해당 위치의 기물 색 != 선택된 기물의 색 -> 추가
            if (nowTile.locatedPiece != null)
            {
                if (nowTile.locatedPiece.pieceColor != pieceColor)
                {
                    attackPieceList.Add(nowTile.locatedPiece);
                }
            }
        }

        // 좌측 대각선 1칸 공격 가능?
        if (IsAvailableTIle(attact2Pos))
        {
            nowTile = TestManager.Instance.testTileList[attact2Pos.x, attact2Pos.y];

            //해당 위치의 기물 색 != 선택된 기물의 색 -> 추가
            if (nowTile.locatedPiece != null)
            {
                if (nowTile.locatedPiece.pieceColor != pieceColor)
                {
                    attackPieceList.Add(nowTile.locatedPiece);
                }
            }
        }
    }

    public override void SetMovableTileList()
    {
        base.SetMovableTileList();

        // 흰색 : row가 증가하는 방향 <-> 검은색 : row가 감소하는 방향이 앞임.
        direction = (pieceColor == GameColor.White) ? 1 : -1;

        // isFIrstMove 설정하기
        if (this.pieceColor == GameColor.White)
        {
            if (this.nowPos.y > 1) isFirstMove = false;
        }
        else if (this.pieceColor == GameColor.Black)
        { 
            if (this.nowPos.y < 6) isFirstMove = false;
        }

        TestTile nowTile = null;
        Vector2Int forward1Pos = new Vector2Int(nowPos.x, nowPos.y + direction);
        Vector2Int forward2Pos = new Vector2Int(nowPos.x, nowPos.y + direction * 2);

        Vector2Int attact1Pos = new Vector2Int(nowPos.x - direction, nowPos.y + direction);    // 좌측 대각선
        Vector2Int attact2Pos = new Vector2Int(nowPos.x + direction, nowPos.y + direction);    // 우측 대각선

        // 1. 이동 타일 확인
        //전방 2칸 이동 가능?
        if (isFirstMove && TestManager.Instance.testTileList[forward1Pos.x, forward1Pos.y].locatedPiece == null)
        {
            nowTile = TestManager.Instance.testTileList[forward2Pos.x, forward2Pos.y];

            //해당 위치에 기물이 존재하지 않으면 movableTIles에 추가
            if (nowTile.locatedPiece == null)
                movableTIleList.Add(nowTile);
        }

        //전방 1칸 이동 가능?
        if (IsAvailableTIle(forward1Pos))
        {
            nowTile = TestManager.Instance.testTileList[forward1Pos.x, forward1Pos.y];

            //해당 위치에 기물이 존재하지 않으면 movableTIles에 추가
            if (nowTile.locatedPiece == null)
                movableTIleList.Add(nowTile);
        }

        // 2. 공격 기물 확인
        // 좌측 대각선 1칸 공격 가능?
        if (IsAvailableTIle(attact1Pos))
        {
            nowTile = TestManager.Instance.testTileList[attact1Pos.x, attact1Pos.y];

            //해당 위치의 기물 색 != 선택된 기물의 색 -> 추가
            if (nowTile.locatedPiece != null)
            {
                if (nowTile.locatedPiece.pieceColor != pieceColor)
                {
                    movableTIleList.Add(nowTile);
                }
            }
            SetIsColorAttack(nowTile);
        }

        // 좌측 대각선 1칸 공격 가능?
        if (IsAvailableTIle(attact2Pos))
        {
            nowTile = TestManager.Instance.testTileList[attact2Pos.x, attact2Pos.y];

            //해당 위치의 기물 색 != 선택된 기물의 색 -> 추가
            if (nowTile.locatedPiece != null)
            {
                if (nowTile.locatedPiece.pieceColor != pieceColor)
                {
                    movableTIleList.Add(nowTile);
                }
            }
            SetIsColorAttack(nowTile);
        }
    }
}
