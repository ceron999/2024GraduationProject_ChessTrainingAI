using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestKing : TestPiece
{
    public override void SetMovableTileList()
    {
        base.SetMovableTileList();

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

            TestTile nowTile = TestManager.Instance.testTileList[targetVector[i].x, targetVector[i].y];
            SetIsColorAttack(nowTile);
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
                else if (nowTile.isWhiteBlockAttack)
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
                    //attackPieceList.Add(nowTile.locatedPiece);
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
}
