using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                    attackPieceList.Add(nowTile.locatedPiece);
                }
            }
            SetIsColorAttack(nowTile);
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
                    attackPieceList.Add(nowTile.locatedPiece);
                }
            }
            SetIsColorAttack(nowTile);
        }
    }

    public bool Promotion(Tile getTile)
    {
        if(getTile.transform.position.y == 0 || getTile.transform.position.y == 7)
        {
            // 끝에 도달할 경우 프로모션 시작
            // 2. 일반적인 움직임 판단
            // 2-1. 현재 Piece 위치 변경
            this.transform.position = getTile.transform.position;
            nowPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);

            // 2-2. 해당 타일에 적 piece가 존재할 경우 해당 기물 파괴
            if (getTile.locatedPiece != null)
            {
                Destroy(getTile.locatedPiece.gameObject);
            }

            // 2-3. 타일 정보 재설정
            ChessManager.instance.chessTileList[nowPos.x, nowPos.y].locatedPiece = null;

            ChessManager.instance.promotionUI.SetActive(true);
            return true;
        }
        return false;
    }
}
