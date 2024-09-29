using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPawn : TestPiece
{
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
}
