using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PieceType
{
    Null, 
    WhitePawn, WhiteKnight, WhiteBishop, WhilteRook, WhiteQueen, WhiteKing, //1~ 6
    BlackPawn, BlackKnight, BlackBishop, BlackRook, BlackQueen, BlackKing   //7~12
}

public abstract class Piece : MonoBehaviour
{
    public PieceType pieceType;
    public Vector2Int nowPos;
    public List<Tile> movableTIles = null;                         //현재 위치에서 이동 가능한 타일
    
    public bool isClickPiece = false;                       //현재 이 기물이 선택되었는가?

    public abstract List<Tile> FindMovableMoveTiles();      //움직일 수 있는 타일 찾는 함수

    //나중에 이 함수 유사하게 
    public void Move()
    {
        //현재 선택한 기물의 이동을 설정합니다. 
         if(isClickPiece)
        {
            ///if(이동 불가능한 땅 선택하면)
            ///     isClickPiece = false;
        }
    }

    private void OnMouseDown()
    {
        //이미 선택한 생태에서 한 번 더 누르면 누른 행위 취소하고 이동 가능한 타일 리스트 초기화
        if (isClickPiece)
        {
            isClickPiece = false;
            movableTIles.Clear();
        }

        //처음 클릭하면 해당 기물을 선택했다고 표시하고 이동 가능한 타일을 찾음
        isClickPiece = true;
        Debug.Log("MovableTiles Count : " + movableTIles.Count);
        FindMovableMoveTiles();
    }
}
