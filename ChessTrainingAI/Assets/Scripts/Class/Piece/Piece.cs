using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    Null, 
    Pawn, Knight, Bishop, Rook, Queen, King, //1~ 6
}

public abstract class Piece : MonoBehaviour
{
    #region 기물 정보
    [Header("기물 정보")]
    public PieceType pieceType;
    public GameColor pieceColor;
    #endregion

    [Header("기물 이동 정보 리스트")]
    public Vector2Int nowPos;
    public List<Tile> movableTIleList = null;                      //현재 위치에서 이동 가능한 타일
    public List<Piece> attackPieceList = null;                      //현재 위치에서 이동 가능한 타일

    public abstract void EvaluateMove();                //움직일 수 있는 타일 찾는 함수

    /// <summary>
    /// 기물 이동하는 함수
    /// </summary>
    /// <param name="selectTIle"> ChessManager에서 마우스로 선택한 Tile </param>
    /// <param name="isSkip"> Piece 이동 모션을 스킵할 것인가? </param>
    public void Move(Tile selectTIle, bool isSkip = true)
    {
        // 0. 받아온 타일이 현재 이동 가능 타일 중 하나인가?
        if (movableTIleList.Count == 0)
            return;

        for (int i = 0; i < movableTIleList.Count; i++)
        {
            // 이동 가능한 타일이면 반복문 깨고 나와
            if (movableTIleList[i].tileName == selectTIle.tileName)
                break;

            // 마지막 타일까지 검사했는데 받아온 타일이 안나오면 리턴
            if (i == movableTIleList.Count - 1)
                return;
        }

        // 기존 타일 정보 받아오기
        Tile nowTIle = ChessManager.instance.chessTileList[nowPos.x, nowPos.y];

        // 폰이나 킹, 룩의 경우 특수 움직임 설정
        SetPieceSpecialInfo();

        // 1. 현재 Piece 위치 변경
        this.transform.position = selectTIle.transform.position;

        // 2. 해당 타일에 적 piece가 존재할 경우 해당 기물 파괴하고 정보 재설정
        if(selectTIle.locatedPiece != null)
        {
            Destroy(selectTIle.locatedPiece.gameObject);

            nowTIle.locatedPiece = null;
        }
        selectTIle.locatedPiece = this.GetComponent<Piece>();
        nowPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        //모두 끝낸 후 턴 종료
        ChessManager.instance.turnEnd?.Invoke();
    }

    private void OnMouseDown()
    {
        // 현재 턴이 아니면 클릭 방지
        if (ChessManager.instance.nowTurnColor != pieceColor)
            return;

        // 1. 이전 기물과 현재 기물 정리
        Piece pastPiece = ChessManager.instance.nowPiece;
        ChessManager.instance.nowPiece = this;

        // 2. 이전 기물의 원 제거 + 현재 기물의 원 표시
        for (int i = 0; i < movableTIleList.Count; i++)
        {
            if(pastPiece != null)
                pastPiece.movableTIleList[i].SetAvailableCircle(false);
            movableTIleList[i].SetAvailableCircle(true);
        }
    }

    #region 타일 이동 가능 및 공격 기물 확인 함수
    //해당 타일이 존재하는가? -> 인덱스 초과를 확인하기 위해 만든 함수
    public bool IsAvailableTIle(Vector2Int getTIleVector)
    {
        Tile nowTile = null;
        try
        {
            nowTile = ChessManager.instance.chessTileList[getTIleVector.x, getTIleVector.y];
        }
        catch
        {
            return false;
        }
        return true;
    }
    #endregion

    #region 특수 정보 확인

    //폰 2칸 전진, 앙파상, 캐슬링, 프로모션 등을 컨트롤하기 위한 특수 함수
    void SetPieceSpecialInfo()
    {
        //1. 폰
        if (ChessManager.instance.nowPiece.pieceType == PieceType.Pawn)
        {
            //1. 처음 2칸 이동 해제
            if(ChessManager.instance.nowPiece.GetComponent<Pawn>().isFirstMove)
                ChessManager.instance.nowPiece.GetComponent<Pawn>().isFirstMove = false;
        }

        else if(ChessManager.instance.nowPiece.pieceType == PieceType.Rook)
        {
            if (ChessManager.instance.nowPiece.GetComponent<Rook>().isFirstMove)
                ChessManager.instance.nowPiece.GetComponent<Rook>().isFirstMove = false;
        }
    }
    #endregion
}
