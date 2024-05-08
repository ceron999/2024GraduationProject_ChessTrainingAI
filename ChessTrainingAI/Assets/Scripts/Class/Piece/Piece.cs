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

    //���߿� �� �Լ� �����ϰ� 
    public void Move()
    {
        //���� ������ �⹰�� �̵��� �����մϴ�. 
         if(isClickPiece)
        {
            ///if(�̵� �Ұ����� �� �����ϸ�)
            ///     isClickPiece = false;
        }
    }

    private void OnMouseDown()
    {
        //�̹� ������ ���¿��� �� �� �� ������ ���� ���� ����ϰ� �̵� ������ Ÿ�� ����Ʈ �ʱ�ȭ
        if (isClickPiece)
        {
            isClickPiece = false;
            movableTIles.Clear();
        }

        //ó�� Ŭ���ϸ� �ش� �⹰�� �����ߴٰ� ǥ���ϰ� �̵� ������ Ÿ���� ã��
        isClickPiece = true;
        Debug.Log("MovableTiles Count : " + movableTIles.Count);
        FindMovableMoveTiles();
    }
}
