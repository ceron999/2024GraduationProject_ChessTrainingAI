using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    GameObject availableCircle;
    public GameColor nowLocateColor;                    //현재 위치한 Piece;
    public bool isAttackedTile = false;                 //공격당하는 타일임(킹 움직임 제한 목적

    public bool isSelectedMovalleTile = false;      //MovableTiles에 들어갔을 때 중복으로 나타나지 않기 위해 추가

    //현재 이동 가능한 타일 위치를 쉽게 보여주기 위하여 설정
    public void SetAvailableCircle(bool isActive)
    {
        availableCircle.SetActive(isActive);
    }

    void EvaluateAttackTile()
    {

    }
}
