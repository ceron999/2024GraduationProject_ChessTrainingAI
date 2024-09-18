using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    //castling 변수
    public bool isFirstMove = true;
    public Rook[] nowRooks = new Rook[2];

    public override void EvaluateMove()
    {
        if (isFirstMove)
        {
            EvaluateCastling();
        }

        List<Vector2Int> targetVector = new List<Vector2Int>();
        targetVector.Add(nowPos + new Vector2Int(-1, +1));
        targetVector.Add(nowPos + new Vector2Int(-1, -1));
        targetVector.Add(nowPos + new Vector2Int(+1, +1));
        targetVector.Add(nowPos + new Vector2Int(+1, -1));
        targetVector.Add(nowPos + new Vector2Int(0, +1));
        targetVector.Add(nowPos + new Vector2Int(0, -1));
        targetVector.Add(nowPos + new Vector2Int(+1, 0));
        targetVector.Add(nowPos + new Vector2Int(-1, 0));

        for (int i = 0; i < targetVector.Count; i++)
        {
            // 1. 해당하는 타일이 존재하지 않으면 넘어감
            if (!IsAvailableTIle(targetVector[i]))
                continue;

            Tile nowTile = ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y];
            
            // 2. 해당 위치가 공격당하는 위치일 경우 넘어감
            if (pieceColor == GameColor.White)
            {
                if (nowTile.isBlackAttack)
                    continue;
                else if (nowTile.isBlackBlockAttack)
                    continue;
            }
            else if (pieceColor == GameColor.Black)
            {
                if (nowTile.isWhiteAttack)
                    continue;
                else if (nowTile.isWhiteBolckAttack)
                    continue;
            }

            // 3. 해당 기물위치에 기물이 있으면 확인
            if (nowTile.locatedPiece != null)
            {
                // 3-1. 해당 타일 기물의 색 == 선택한 기물의 색이면 넘어감
                if (nowTile.locatedPiece.pieceColor == pieceColor)
                    continue;
                else
                {
                    // 3-2. 해당 위치로 이동했을 때 킹이 공격당하지 않는다면 공격 기물 추가 및 이동 타일 추가
                    attackPieceList.Add(nowTile.locatedPiece);
                    movableTIleList.Add(nowTile);
                    SetIsColorAttack(nowTile);
                }
            }
            else
            {
                // 3. 해당 타일에 기물이 존재하지 않았을 경우
                movableTIleList.Add(nowTile);
                SetIsColorAttack(nowTile);
            }
        }
    }
    void EvaluateCastling()
    {
        // 체크 당하면 캐슬링 불가
        if (ChessManager.instance.isCheck)
            return;

        EvaluateKingSideCastling();
        EvaluateQueenSideCastling();
    }

    void EvaluateKingSideCastling()
    {
        // 1. 킹 체크 상태면 캐슬링 불가
        if (ChessManager.instance.isCheck)
            return;

        List<Vector2Int> targetVector = new List<Vector2Int>();

        targetVector.Add(nowPos + new Vector2Int(1, 0));
        targetVector.Add(nowPos + new Vector2Int(2, 0)); 

        for (int i =0; i< targetVector.Count; i++)
        {
            // 2. 만일 킹과 룩 사이에 기물이 존재하면 캐슬링 불가능하므로 빠져나감
            if (ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y].locatedPiece != null)
                break;
            
            // 3. 마지막 Rook 처음 움직이는지 확인
            if (i == targetVector.Count - 1)
            {
                if (nowRooks[1].isFirstMove)
                    movableTIleList.Add(ChessManager.instance.chessTileList[6, nowPos.y]);
            }
        }

        return;
    }

    void EvaluateQueenSideCastling()
    {
        // 1. 킹 체크 상태면 캐슬링 불가
        if (ChessManager.instance.isCheck)
            return;

        List<Vector2Int> targetVector = new List<Vector2Int>();

        targetVector.Add(nowPos + new Vector2Int(-1, 0));
        targetVector.Add(nowPos + new Vector2Int(-2, 0));
        targetVector.Add(nowPos + new Vector2Int(-3, 0));

        for (int i = 0; i < targetVector.Count; i++)
        {
            // 2. 만일 킹과 룩 사이에 기물이 존재하면 캐슬링 불가능하므로 빠져나감
            if (ChessManager.instance.chessTileList[targetVector[i].x, targetVector[i].y].locatedPiece != null)
                break;

            // 3. 마지막 Rook 처음 움직이는지 확인
            if (i == targetVector.Count - 1)
            {
                if (nowRooks[0].isFirstMove)
                    movableTIleList.Add(ChessManager.instance.chessTileList[2, nowPos.y]);
            }
        }

        return;
    }

    public bool Castling(Tile getTile)
    {
        Tile nowTIle = ChessManager.instance.chessTileList[nowPos.x, nowPos.y];

        if (getTile.tileName == TIleName.a3)
        {
            // 흰색 퀸사이드 캐슬링
            // 1. 위치 이동
            this.transform.position = getTile.transform.position;
            nowRooks[0].transform.position = ChessManager.instance.chessTileList[3,0].transform.position;

            // 2. 해당 타일 정보 수정
            nowTIle.locatedPiece = null;
            getTile.locatedPiece = this;
            ChessManager.instance.chessTileList[3, 0].locatedPiece = nowRooks[0];

            return true;
        }
        else if (getTile.tileName == TIleName.a7)
        {
            // 흰색 킹사이드 캐슬링
            // 1. 위치 이동
            this.transform.position = getTile.transform.position;
            nowRooks[1].transform.position = ChessManager.instance.chessTileList[5, 0].transform.position;

            // 2. 해당 타일 정보 수정
            nowTIle.locatedPiece = null;
            getTile.locatedPiece = this;
            ChessManager.instance.chessTileList[5, 0].locatedPiece = nowRooks[1];

            return true;
        }
        else if (getTile.tileName == TIleName.h3)
        {
            // 검은색 퀸사이드 캐슬링
            // 1. 위치 이동
            this.transform.position = getTile.transform.position;
            nowRooks[0].transform.position = ChessManager.instance.chessTileList[3, 7].transform.position;

            // 2. 해당 타일 정보 수정
            nowTIle.locatedPiece = null;
            getTile.locatedPiece = this;
            ChessManager.instance.chessTileList[3, 7].locatedPiece = nowRooks[0];

            return true;
        }
        else if (getTile.tileName == TIleName.h7)
        {
            // 검은색 킹사이드 캐슬링
            // 1. 위치 이동
            this.transform.position = getTile.transform.position;
            nowRooks[1].transform.position = ChessManager.instance.chessTileList[5, 7].transform.position;

            // 2. 해당 타일 정보 수정
            nowTIle.locatedPiece = null;
            getTile.locatedPiece = this;
            ChessManager.instance.chessTileList[5, 7].locatedPiece = nowRooks[1];

            return true;
        }
        else return false;
    }
}
