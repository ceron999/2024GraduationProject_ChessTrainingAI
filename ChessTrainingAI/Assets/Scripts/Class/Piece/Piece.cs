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

    //Parameter :   isSkip = Piece 이동 모션을 스킵할 것인가?
    //              selectTile = ChessManager에서 마우스로 선택한 Tile
    public void Move(Tile selectTIle, bool isSkip = true)
    {
        //0. 이동 가능한 타일로 선택되어져 있는가?
        if (!selectTIle.isSelectedMovalleTile)
            return;

        //1. Piece가 해당 타일에 존재하지 않을 경우 그냥 이동
        if (selectTIle.nowLocateColor == PlayerColor.Null)
        {
            //기존에 존재한 타일 정보 변경
            ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y].nowLocateColor = PlayerColor.Null;
            
            //piece가 앞으로 위치할 Tile의 정보 변경
            if((int)pieceType < 7)
                selectTIle.nowLocateColor = PlayerColor.White;
            else
                selectTIle.nowLocateColor = PlayerColor.Black;

            //현재 Piece 정보 변경
            this.transform.position = selectTIle.transform.position;
            nowPos = new Vector2Int((int)selectTIle.transform.position.x, (int)selectTIle.transform.position.y);

            //모두 끝낸 후 정보 초기화
            ClearPieceInfo();
        }
        
        //2. Piece가 해당 타일에 존재할 경우
        //2-1. 적일 경우
        //2-2. 아군일 경우
    }

    private void OnMouseDown()
    {
        //1. 선택한 Piece가 있는 상태에서 다른 Piece를 고를 때
        //이전에 선택한 Piece의 이동 가능 타일을 표시한 것들을 초기화
        if (ChessManager.chessManager.nowPiece != null)
        {
            ClearPieceInfo();
        }

        //처음 클릭하면 해당 기물을 선택했다고 표시하고 이동 가능한 타일을 찾음
        ChessManager.chessManager.nowPiece = this;
        isClickPiece = true;
        FindMovableMoveTiles();

        //찾은 타일의 AvailableCircle을 활성화하여 움직일 수 있는 위치를 확인한다.
        for (int i = 0; i < movableTIles.Count; i++)
        {
            movableTIles[i].SetAvailableCircle(true);
        }

        Debug.Log("MovableTiles Count : " + movableTIles.Count);
    }

    //현재 선택한 Piece의 정보를 초기화합니다.
    void ClearPieceInfo()
    {
        ChessManager.chessManager.nowPiece.isClickPiece = false;

        //AvailableCircle을 active 상태를 다시 false로 변환
        for (int i = 0; i < ChessManager.chessManager.nowPiece.movableTIles.Count; i++)
        {
            ChessManager.chessManager.nowPiece.movableTIles[i].SetAvailableCircle(false);
            ChessManager.chessManager.nowPiece.movableTIles[i].isSelectedMovalleTile = false;
        }

        ChessManager.chessManager.nowPiece.movableTIles.Clear();

        ChessManager.chessManager.nowPiece = null;
    }
}
