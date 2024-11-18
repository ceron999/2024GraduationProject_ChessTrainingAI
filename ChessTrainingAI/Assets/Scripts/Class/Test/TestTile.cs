using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTile : MonoBehaviour
{
    public bool isWhiteAttack;
    public bool isWhiteBlockAttack;       // 비숍이나 룩, 퀸이 가로막힌 공격 경로일 경우
    public bool isBlackAttack;
    public bool isBlackBlockAttack;

    public TIleName tileName;

    public TestPiece locatedPiece;                    //현재 위치한 Piece

    /// <summary>
    /// 타일의 정보를 가져와 테스트 타일에 저장합니다
    /// </summary>
    /// <param name="getTile"> 저장하길 원하는 타일 </param>
    public void SetTileInfo(Tile getTile)
    {
        isWhiteAttack = getTile.isWhiteAttack;
        isBlackAttack = getTile.isBlackAttack;
        isBlackBlockAttack = getTile.isBlackBlockAttack;

        tileName = getTile.tileName;

        if (getTile.locatedPiece != null)
        {
            locatedPiece.SetPieceInfo(getTile.locatedPiece);
        }
    }

    // 매 턴마다 정보 초기화할때 쓸 함수
    public void ClearTileInfo()
    {
        locatedPiece = null;
        isWhiteAttack = false;
        isWhiteBlockAttack = false;
        isBlackAttack = false;
        isBlackBlockAttack = false;
    }

    // 매 턴마다 정보 초기화할때 쓸 함수
    public void ResetTileInfo()
    {
        isWhiteAttack = false;
        isWhiteBlockAttack = false;
        isBlackAttack = false;
        isBlackBlockAttack = false;

        locatedPiece = null;
    }
}
