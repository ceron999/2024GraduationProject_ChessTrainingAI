using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    //castling 변수
    public bool isFirstMove = true;
    public Rook[] nowRooks = new Rook[2];

    public bool isCheck = false;
    public bool isCheckMate = false;

    public override void EvaluateMove()
    {
        if (isFirstMove)
        {
            EvaluateKingSideCastling();
            EvaluateQueenSideCastling();
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

            // 2. 해당 기물위치에 기물이 있으면 확인
            if (nowTile.locatedPiece != null)
            {
                // 2-1. 해당 타일 기물의 색 == 선택한 기물의 색이면 넘어감
                if (nowTile.locatedPiece.pieceColor == pieceColor)
                    continue;

                else
                {
                    // 2-2. 해당 위치가 공격당하는 위치일 경우 넘어감
                    if (pieceColor == GameColor.White)
                    {
                        if(nowTile.isBlackAttack)
                            continue;
                    }
                    else if(pieceColor == GameColor.Black)
                    {
                        if (nowTile.isWhiteAttack)
                            continue;
                    }

                    // 2-3. 해당 위치로 이동했을 때 킹이 공격당하지 않는다면 공격 기물 추가 및 이동 타일 추가
                    attackPieceList.Add(nowTile.locatedPiece);
                    movableTIleList.Add(nowTile);
                    SetIsColorAttack(nowTile);
                }
            }
            else
            {
                // 3. 해당 타일에 기물이 존재하지 않았을 경우
                // 3-1. 해당 위치로 이동했을 때 킹이 공격당하면 넘어감
                if (ChessManager.instance.isCheck)
                    continue;
                
                // 2-3. 해당 위치로 이동했을 때 킹이 공격당하지 않는다면 이동 타일 추가
                else
                {
                    movableTIleList.Add(nowTile);
                    SetIsColorAttack(nowTile);
                }
            }
        }
    }
    void EvaluateCastling()
    {
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
}
