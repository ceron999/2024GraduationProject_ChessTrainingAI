using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPiece : MonoBehaviour
{
    #region 기물 정보
    [Header("기물 정보")]
    public PieceType pieceType;
    public GameColor pieceColor;
    #endregion

    [Header("기물 이동 정보 리스트")]
    public Vector2Int nowPos;
    public List<TestTile> movableTIleList = null;                      //현재 위치에서 이동 가능한 타일
    public List<TestPiece> attackPieceList = null;                      //현재 위치에서 이동 가능한 타일

    public void Move(TestTile getTile)
    {
        TestTile nowTile = TestManager.Instance.testTileList[nowPos.x, nowPos.y];

        nowTile.locatedPiece = null;

        // 해당 위치에 기물 존재하면 해당 기물 
        if (getTile.locatedPiece != null)
        {

        }
        getTile.locatedPiece = this;
    }

    #region 타일 이동 가능 및 공격 기물 확인 함수
    //해당 타일이 존재하는가? -> 인덱스 초과를 확인하기 위해 만든 함수
    protected bool IsAvailableTIle(Vector2Int getTIleVector)
    {
        if (getTIleVector.x < 0 || getTIleVector.x > 7)
            return false;
        else if (getTIleVector.y < 0 || getTIleVector.y > 7)
            return false;

        return true;
    }

    public bool IsAttackKing()
    {
        if (attackPieceList.Count == 0)
            return false;

        for (int i = 0; i < attackPieceList.Count; i++)
        {
            if (attackPieceList[i].pieceType == PieceType.King)
                return true;
        }
        return false;
    }
    #endregion


}
