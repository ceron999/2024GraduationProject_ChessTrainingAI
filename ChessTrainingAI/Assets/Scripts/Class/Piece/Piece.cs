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
    #region �⹰ ����
    [Header("�⹰ ����")]
    public PieceType pieceType;
    public GameColor pieceColor;
    #endregion

    [Header("�⹰ �̵� ���� ����Ʈ")]
    public Vector2Int nowPos;
    public List<Tile> movableTIleList = null;                      //���� ��ġ���� �̵� ������ Ÿ��
    public List<Piece> attackPieceList = null;                      //���� ��ġ���� �̵� ������ Ÿ��

    public abstract void EvaluateMove();                //������ �� �ִ� Ÿ�� ã�� �Լ�

    /// <summary>
    /// �⹰ �̵��ϴ� �Լ�
    /// </summary>
    /// <param name="selectTIle"> ChessManager���� ���콺�� ������ Tile </param>
    /// <param name="isSkip"> Piece �̵� ����� ��ŵ�� ���ΰ�? </param>
    public void Move(Tile selectTIle, bool isSkip = true)
    {
        // 0. �޾ƿ� Ÿ���� ���� �̵� ���� Ÿ�� �� �ϳ��ΰ�?
        if (movableTIleList.Count == 0)
            return;

        for (int i = 0; i < movableTIleList.Count; i++)
        {
            // �̵� ������ Ÿ���̸� �ݺ��� ���� ����
            if (movableTIleList[i].tileName == selectTIle.tileName)
                break;

            // ������ Ÿ�ϱ��� �˻��ߴµ� �޾ƿ� Ÿ���� �ȳ����� ����
            if (i == movableTIleList.Count - 1)
                return;
        }

        // ���� Ÿ�� ���� �޾ƿ���
        Tile nowTIle = ChessManager.instance.chessTileList[nowPos.x, nowPos.y];

        // ���̳� ŷ, ���� ��� Ư�� ������ ����
        SetPieceSpecialInfo();

        // 1. ���� Piece ��ġ ����
        this.transform.position = selectTIle.transform.position;

        // 2. �ش� Ÿ�Ͽ� �� piece�� ������ ��� �ش� �⹰ �ı��ϰ� ���� �缳��
        if(selectTIle.locatedPiece != null)
        {
            Destroy(selectTIle.locatedPiece.gameObject);

            nowTIle.locatedPiece = null;
        }
        selectTIle.locatedPiece = this.GetComponent<Piece>();
        nowPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        //��� ���� �� �� ����
        ChessManager.instance.turnEnd?.Invoke();
    }

    private void OnMouseDown()
    {
        // ���� ���� �ƴϸ� Ŭ�� ����
        if (ChessManager.instance.nowTurnColor != pieceColor)
            return;

        // 1. ���� �⹰�� ���� �⹰ ����
        Piece pastPiece = ChessManager.instance.nowPiece;
        ChessManager.instance.nowPiece = this;

        // 2. ���� �⹰�� �� ���� + ���� �⹰�� �� ǥ��
        for (int i = 0; i < movableTIleList.Count; i++)
        {
            if(pastPiece != null)
                pastPiece.movableTIleList[i].SetAvailableCircle(false);
            movableTIleList[i].SetAvailableCircle(true);
        }
    }

    #region Ÿ�� �̵� ���� �� ���� �⹰ Ȯ�� �Լ�
    //�ش� Ÿ���� �����ϴ°�? -> �ε��� �ʰ��� Ȯ���ϱ� ���� ���� �Լ�
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

    #region Ư�� ���� Ȯ��

    //�� 2ĭ ����, ���Ļ�, ĳ����, ���θ�� ���� ��Ʈ���ϱ� ���� Ư�� �Լ�
    void SetPieceSpecialInfo()
    {
        //1. ��
        if (ChessManager.instance.nowPiece.pieceType == PieceType.Pawn)
        {
            //1. ó�� 2ĭ �̵� ����
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
