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


    // 매 턴마다 정보 초기화할때 쓸 함수
    public void ClearTileInfo()
    {
        isWhiteAttack = false;
        isWhiteBlockAttack = false;
        isBlackAttack = false;
        isBlackBlockAttack = false;
    }
}
