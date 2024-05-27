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
    public GameColor pieceColor;
    public Vector2Int nowPos;
    public List<Tile> movableTIles = null;                      //현재 위치에서 이동 가능한 타일
    public List<Tile> attackTIles = null;                      //현재 위치에서 이동 가능한 타일

    public bool isClickPiece = false;                           //현재 이 기물이 선택되었는가?

    //이외 정보
    int pieceLayer = 1 << 8;

    public abstract void FindMovableMoveTiles();                //움직일 수 있는 타일 찾는 함수

    public abstract void SetAttackTile(bool isActive);                       //현재 공격 타일 설정 함수
                                                                             //isActive가 참이면 AttackTile = true;

    //Parameter :   isSkip = Piece 이동 모션을 스킵할 것인가?
    //              selectTile = ChessManager에서 마우스로 선택한 Tile
    public void Move(Tile selectTIle, bool isSkip = true)
    {
        //0. 이동 가능한 타일로 선택되어져 있는가?
        if (!selectTIle.isSelectedMovalleTile)
            return;

        SetAttackTile(false);
        //기존에 존재한 타일 정보 변경
        ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y].nowLocateColor = GameColor.Null;

        //현재 Piece 정보 변경
        this.transform.position = selectTIle.transform.position;
        nowPos = new Vector2Int((int)selectTIle.transform.position.x, (int)selectTIle.transform.position.y);

        //해당 타일에 적 piece가 존재할 경우 해당 기물 파괴
        if (selectTIle.nowLocateColor != GameColor.Null)
        {
            Collider[] findPiece = Physics.OverlapSphere(this.transform.position, 0.1f, pieceLayer);
            if (findPiece[0].name != this.name)
                Destroy(findPiece[0].gameObject);
        }

        //piece가 앞으로 위치할 Tile의 정보 변경
        selectTIle.nowLocateColor = pieceColor;
        //모두 끝낸 후 정보 초기화 + 공격타일 설정
        ClearPieceInfo();
        SetAttackTile(true);

        ChessManager.chessManager.EndTurn();
    }

    private void OnMouseDown()
    {
        //현재 턴이 아니면 클릭 방지
        if (ChessManager.chessManager.nowTurnColor != pieceColor)
            return;
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

    //해당 타일이 존재하는가? -> 인덱스 초과를 확인하기 위해 만든 함수
    public bool IsAvailableTIle(Vector2Int getTIleVector)
    {
        Tile nowTile = null;
        try
        {
            nowTile = ChessManager.chessManager.chessTileList[getTIleVector.x, getTIleVector.y];
        }
        catch
        {
            return false;
        }
        return true;
    }

    public void SetMovablePiecesSelected()
    {
        for(int i =0; i<movableTIles.Count;i++)
        {
            movableTIles[i].isSelectedMovalleTile = true;
        }
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
        SetPieceSpecialInfo();

        ChessManager.chessManager.nowPiece = null;
    }

    //폰 2칸 전진, 앙파상, 캐슬링, 프로모션 등을 컨트롤하기 위한 특수 함수
    void SetPieceSpecialInfo()
    {
        //1. 폰
        if (ChessManager.chessManager.nowPiece.pieceType == PieceType.WhitePawn || ChessManager.chessManager.nowPiece.pieceType == PieceType.WhitePawn)
        {
            //1. 처음 2칸 이동 해제
            if(ChessManager.chessManager.nowPiece.GetComponent<Pawn>().isFirstMove)
                ChessManager.chessManager.nowPiece.GetComponent<Pawn>().isFirstMove = false;
        }
    }

    public void DebugMovableTiles(List<Tile> getMovableTileList)
    {
        for (int i = 0; i < getMovableTileList.Count; i++)
            Debug.Log(getMovableTileList[i].transform.position);
    }

}
