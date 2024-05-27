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
    public List<Tile> movableTIles = null;                      //���� ��ġ���� �̵� ������ Ÿ��
    public List<Tile> attackTIles = null;                      //���� ��ġ���� �̵� ������ Ÿ��

    public bool isClickPiece = false;                           //���� �� �⹰�� ���õǾ��°�?

    //�̿� ����
    int pieceLayer = 1 << 8;

    public abstract void FindMovableMoveTiles();                //������ �� �ִ� Ÿ�� ã�� �Լ�

    public abstract void SetAttackTile(bool isActive);                       //���� ���� Ÿ�� ���� �Լ�
                                                                             //isActive�� ���̸� AttackTile = true;

    //Parameter :   isSkip = Piece �̵� ����� ��ŵ�� ���ΰ�?
    //              selectTile = ChessManager���� ���콺�� ������ Tile
    public void Move(Tile selectTIle, bool isSkip = true)
    {
        //0. �̵� ������ Ÿ�Ϸ� ���õǾ��� �ִ°�?
        if (!selectTIle.isSelectedMovalleTile)
            return;

        SetAttackTile(false);
        //������ ������ Ÿ�� ���� ����
        ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y].nowLocateColor = GameColor.Null;

        //���� Piece ���� ����
        this.transform.position = selectTIle.transform.position;
        nowPos = new Vector2Int((int)selectTIle.transform.position.x, (int)selectTIle.transform.position.y);

        //�ش� Ÿ�Ͽ� �� piece�� ������ ��� �ش� �⹰ �ı�
        if (selectTIle.nowLocateColor != GameColor.Null)
        {
            Collider[] findPiece = Physics.OverlapSphere(this.transform.position, 0.1f, pieceLayer);
            if (findPiece[0].name != this.name)
                Destroy(findPiece[0].gameObject);
        }

        //piece�� ������ ��ġ�� Tile�� ���� ����
        selectTIle.nowLocateColor = pieceColor;
        //��� ���� �� ���� �ʱ�ȭ + ����Ÿ�� ����
        ClearPieceInfo();
        SetAttackTile(true);

        ChessManager.chessManager.EndTurn();
    }

    private void OnMouseDown()
    {
        //���� ���� �ƴϸ� Ŭ�� ����
        if (ChessManager.chessManager.nowTurnColor != pieceColor)
            return;
        //1. ������ Piece�� �ִ� ���¿��� �ٸ� Piece�� �� ��
        //������ ������ Piece�� �̵� ���� Ÿ���� ǥ���� �͵��� �ʱ�ȭ
        if (ChessManager.chessManager.nowPiece != null)
        {
            ClearPieceInfo();
        }

        //ó�� Ŭ���ϸ� �ش� �⹰�� �����ߴٰ� ǥ���ϰ� �̵� ������ Ÿ���� ã��
        ChessManager.chessManager.nowPiece = this;
        isClickPiece = true;
        FindMovableMoveTiles();

        //ã�� Ÿ���� AvailableCircle�� Ȱ��ȭ�Ͽ� ������ �� �ִ� ��ġ�� Ȯ���Ѵ�.
        for (int i = 0; i < movableTIles.Count; i++)
        {
            movableTIles[i].SetAvailableCircle(true);
        }

        Debug.Log("MovableTiles Count : " + movableTIles.Count);
    }

    //�ش� Ÿ���� �����ϴ°�? -> �ε��� �ʰ��� Ȯ���ϱ� ���� ���� �Լ�
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

    //���� ������ Piece�� ������ �ʱ�ȭ�մϴ�.
    void ClearPieceInfo()
    {
        ChessManager.chessManager.nowPiece.isClickPiece = false;

        //AvailableCircle�� active ���¸� �ٽ� false�� ��ȯ
        for (int i = 0; i < ChessManager.chessManager.nowPiece.movableTIles.Count; i++)
        {
            ChessManager.chessManager.nowPiece.movableTIles[i].SetAvailableCircle(false);
            ChessManager.chessManager.nowPiece.movableTIles[i].isSelectedMovalleTile = false;
        }

        ChessManager.chessManager.nowPiece.movableTIles.Clear();
        SetPieceSpecialInfo();

        ChessManager.chessManager.nowPiece = null;
    }

    //�� 2ĭ ����, ���Ļ�, ĳ����, ���θ�� ���� ��Ʈ���ϱ� ���� Ư�� �Լ�
    void SetPieceSpecialInfo()
    {
        //1. ��
        if (ChessManager.chessManager.nowPiece.pieceType == PieceType.WhitePawn || ChessManager.chessManager.nowPiece.pieceType == PieceType.WhitePawn)
        {
            //1. ó�� 2ĭ �̵� ����
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
