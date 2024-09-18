using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public enum TIleName
{
    a1, a2, a3, a4, a5, a6, a7, a8, 
    b1, b2, b3, b4, b5, b6, b7, b8,
    c1, c2, c3, c4, c5, c6, c7, c8,
    d1, d2, d3, d4, d5, d6, d7, d8,
    e1, e2, e3, e4, e5, e6, e7, e8,
    f1, f2, f3, f4, f5, f6, f7, f8,
    g1, g2, g3, g4, g5, g6, g7, g8,
    h1, h2, h3, h4, h5, h6, h7, h8,
}

public class Tile : MonoBehaviour
{
    [SerializeField]
    GameObject availableCircle;
    public bool isWhiteAttack;
    public bool isWhiteBlockAttack;       // 비숍이나 룩, 퀸이 가로막힌 공격 경로일 경우
    public bool isBlackAttack;
    public bool isBlackBlockAttack;

    public TIleName tileName;

    public Piece locatedPiece;                    //현재 위치한 Piece

    //현재 이동 가능한 타일 위치를 쉽게 보여주기 위하여 설정
    public void SetAvailableCircle(bool isActive)
    {
        availableCircle.SetActive(isActive);
    }


    // 매 턴마다 정보 초기화할때 쓸 함수
    public void ClearTileInfo()
    {
        isWhiteAttack = false;
        isWhiteBlockAttack = false;
        isBlackAttack = false;
        isBlackBlockAttack = false;
    }

    private void OnMouseDown()
    {
        // 이동할 기물이 없으면 패스
        if (ChessManager.instance.nowPiece == null)
            return;

        // 클릭한 타일의 기물 == 선택한 기물이면 클릭 한번에 piece와 tile 의 클릭 함수가 동작한 것이므로 제거
        if (locatedPiece != null && ChessManager.instance.nowPiece.nowPos == locatedPiece.nowPos)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            ChessManager.instance.nowPiece.Move(this.GetComponent<Tile>());
        }
    }
}
