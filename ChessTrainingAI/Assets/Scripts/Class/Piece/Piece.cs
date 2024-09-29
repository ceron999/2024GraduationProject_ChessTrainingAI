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
        if (movableTIleList.Count == 0)
            return;

        // 0. �޾ƿ� Ÿ���� �̵� ���� Ÿ���ΰ�?
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

        // 1. Ư���� ������ �켱 �Ǵ�

        // ���̳� ŷ, ���� ��� Ư�� ������
        if (IsSpecialMove(selectTIle))
            return;

        // ���̳� ŷ, ���� ��� Ư�� ������ ���� ����
        SetPieceSpecialInfo();

        // 2. �Ϲ����� ������ �Ǵ�
        // 2-1. ���� Piece ��ġ ����
        this.transform.position = selectTIle.transform.position;
        nowPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        // 2-2. �ش� Ÿ�Ͽ� �� piece�� ������ ��� �ش� �⹰ �ı�
        if (selectTIle.locatedPiece != null)
        {
            Destroy(selectTIle.locatedPiece.gameObject);
        }

        // 2-3. Ÿ�� ���� �缳��
        nowTIle.locatedPiece = null;
        selectTIle.locatedPiece = this.GetComponent<Piece>();

        //��� ���� �� �� ����
        ChessManager.instance.turnEnd?.Invoke();
    }

    public void ShowMovableTiles()
    {
        // ���� ���� �ƴϸ� Ŭ�� ����
        if (ChessManager.instance.nowTurnColor != pieceColor)
            return;

        // 1. ���� �⹰�� ���� �⹰ ����
        if(ChessManager.instance.nowPiece != null)
        {
            Piece pastPiece = ChessManager.instance.nowPiece;
            pastPiece.SetAvailableCircle(false);
        }
        SetAvailableCircle(true);

        ChessManager.instance.nowPiece = this;
    }

    // �̵� ������ Ÿ������ ���̴� ���� ���̰����� �Ⱥ��̰����� �����ϴ� �Լ�
    void SetAvailableCircle(bool isActive)
    {
        if(movableTIleList.Count >0)
            for(int i =0; i< movableTIleList.Count; i++)
            {
                movableTIleList [i].SetAvailableCircle(isActive);
            }
    }

    #region Ÿ�� �̵� ���� �� ���� �⹰ Ȯ�� �Լ�
    //�ش� Ÿ���� �����ϴ°�? -> �ε��� �ʰ��� Ȯ���ϱ� ���� ���� �Լ�
    protected bool IsAvailableTIle(Vector2Int getTIleVector)
    {
        if (getTIleVector.x < 0 || getTIleVector.x > 7)
            return false;
        else if (getTIleVector.y < 0 || getTIleVector.y > 7)
            return false;

        return true;
    }

    public bool IsAttackKing()
    {
        if (attackPieceList.Count == 0)
            return false;

        for(int i =0; i<attackPieceList.Count; i++)
        {
            if (attackPieceList[i].pieceType == PieceType.King)
                return true;
        }
        return false;
    }
    #endregion

    #region Ư�� ����(ĳ����, ���Ļ�, ���θ��) Ȯ��

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

        else if (ChessManager.instance.nowPiece.pieceType == PieceType.King)
        {
            if (ChessManager.instance.nowPiece.GetComponent<King>().isFirstMove)
            {
                ChessManager.instance.nowPiece.GetComponent<King>().isFirstMove = false;
            }
        }
    }

    bool IsSpecialMove(Tile getTile)
    {
        if (pieceType == PieceType.King)
        {
            // 0. �̹� 1ȸ ���������� �Ұ���
            if (!GetComponent<King>().isFirstMove)
                return false;
            
            // 1. ĳ����
            if (GetComponent<King>().Castling(getTile))
            {
                ChessManager.instance.turnEnd?.Invoke();
                return true;
            }
            else return false;

        }
        else if (pieceType == PieceType.Pawn)
        {
            // 0. �̹� 1ȸ ���������� �Ұ���
            if (!GetComponent<Pawn>().isFirstMove)
                return false;

            // 2. �� ���θ�� or ���Ļ�
            if (GetComponent<Pawn>().Promotion(getTile))
            {
                return true;
            }
            else return false;
        }
        else
            return false;
    }

    public void SetIsColorAttack(Tile getTile)
    {
        if (pieceColor == GameColor.White)
        {
            getTile.isWhiteAttack = true;
        }
        else if(pieceColor == GameColor.Black)
        {
            getTile.isBlackAttack = true;
        }
    }

    public void SetIsColorBlockAttack(Tile getTile)
    {
        if (pieceColor == GameColor.White)
        {
            getTile.isWhiteBlockAttack = true;
        }
        else if (pieceColor == GameColor.Black)
        {
            getTile.isBlackBlockAttack = true;
        }
    }
    #endregion
}
