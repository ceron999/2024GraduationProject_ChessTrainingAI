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
    public List<Tile> movableTIles = null;                         //���� ��ġ���� �̵� ������ Ÿ��
    
    public bool isClickPiece = false;                       //���� �� �⹰�� ���õǾ��°�?

    public abstract List<Tile> FindMovableMoveTiles();      //������ �� �ִ� Ÿ�� ã�� �Լ�

    //Parameter :   isSkip = Piece �̵� ����� ��ŵ�� ���ΰ�?
    //              selectTile = ChessManager���� ���콺�� ������ Tile
    public void Move(Tile selectTIle, bool isSkip = true)
    {
        //0. �̵� ������ Ÿ�Ϸ� ���õǾ��� �ִ°�?
        if (!selectTIle.isSelectedMovalleTile)
            return;

        //1. Piece�� �ش� Ÿ�Ͽ� �������� ���� ��� �׳� �̵�
        if (selectTIle.nowLocateColor == PlayerColor.Null)
        {
            //������ ������ Ÿ�� ���� ����
            ChessManager.chessManager.chessTileList[nowPos.x, nowPos.y].nowLocateColor = PlayerColor.Null;
            
            //piece�� ������ ��ġ�� Tile�� ���� ����
            if((int)pieceType < 7)
                selectTIle.nowLocateColor = PlayerColor.White;
            else
                selectTIle.nowLocateColor = PlayerColor.Black;

            //���� Piece ���� ����
            this.transform.position = selectTIle.transform.position;
            nowPos = new Vector2Int((int)selectTIle.transform.position.x, (int)selectTIle.transform.position.y);

            //��� ���� �� ���� �ʱ�ȭ
            ClearPieceInfo();
        }
        
        //2. Piece�� �ش� Ÿ�Ͽ� ������ ���
        //2-1. ���� ���
        //2-2. �Ʊ��� ���
    }

    private void OnMouseDown()
    {
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

        ChessManager.chessManager.nowPiece = null;
    }
}
