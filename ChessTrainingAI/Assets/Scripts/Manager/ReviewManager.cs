using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewManager : MonoBehaviour
{
    #region Ÿ��, ������ ����
    [SerializeField]
    GameObject tilePrefab;
    [SerializeField]
    Transform chessBoardParent;
    [SerializeField]
    public Tile[,] chessTileList = new Tile[8, 8];
    #endregion Ÿ��, ������ ����

    #region �⹰ ������
    [SerializeField]
    Transform whitePiecesParent;
    [SerializeField]
    Transform blackPiecesParent;
    [SerializeField]
    GameObject whitePawnPrefab;
    [SerializeField]
    GameObject whiteKnightPrefab;
    [SerializeField]
    GameObject whiteBishopPrefab;
    [SerializeField]
    GameObject whiteRookPrefab;
    [SerializeField]
    GameObject whiteQueenPrefab;
    [SerializeField]
    GameObject whiteKingPrefab;
    [SerializeField]
    GameObject blackPawnPrefab;
    [SerializeField]
    GameObject blackKnightPrefab;
    [SerializeField]
    GameObject blackBishopPrefab;
    [SerializeField]
    GameObject blackRookPrefab;
    [SerializeField]
    GameObject blackQueenPrefab;
    [SerializeField]
    GameObject blackKingPrefab;
    #endregion �⹰ ������

    private void Start()
    {
        SetChessBoard();
        SetChessPiece();
    }

    #region ������ ���� �� �⹰ ����
    void SetChessBoard()
    {
        bool colorChange = true;
        GameObject tile;

        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                // Ÿ���� �����ϰ� chessTileList�� ����
                tile = Instantiate(tilePrefab, chessBoardParent);
                chessTileList[row, col] = tile.GetComponent<Tile>();
                chessTileList[row, col].gameObject.GetComponent<Transform>().position = new Vector2(row, col);

                // Ÿ�� �̸� ����
                chessTileList[row, col].tileName = (TIleName)(row + col * 8);

                // Ÿ�� ���� ����
                if (colorChange)
                    chessTileList[row, col].GetComponent<SpriteRenderer>().color = new Color(115 / 255f, 144 / 255f, 83 / 255f);
                else
                    chessTileList[row, col].GetComponent<SpriteRenderer>().color = new Color(235 / 255f, 236 / 255f, 208 / 255f);

                // ���� Eow�� 0�� Col�� ������ ���� Row�� 7�� Col�� �����ϹǷ� ��ȭ���� �ʵ��� �����ϴ� �κ�
                if (row != 7)
                    colorChange = !colorChange;
            }
    }

    void SetChessPiece()
    {
        SetPawn(GameColor.White);
        SetKnight(GameColor.White);
        SetBishop(GameColor.White);
        SetQueen(GameColor.White);
        SetKing(GameColor.White);
        SetRook(GameColor.White);

        SetPawn(GameColor.Black);
        SetKnight(GameColor.Black);
        SetBishop(GameColor.Black);
        SetQueen(GameColor.Black);
        SetKing(GameColor.Black);
        SetRook(GameColor.Black);
    }

    void SetPawn(GameColor getColor)
    {
        Piece piecePrefab = null;

        //������ �⹰ ����
        for (int i = 0; i < 8; i++)
        {
            if (getColor == GameColor.Black)
            {
                piecePrefab = Instantiate(blackPawnPrefab, blackPiecesParent).GetComponent<Piece>();
                piecePrefab.transform.position = new Vector2(i, 6);
            }
            else if (getColor == GameColor.White)
            {
                piecePrefab = Instantiate(whitePawnPrefab, whitePiecesParent).GetComponent<Piece>();
                piecePrefab.transform.position = new Vector2(i, 1);
            }
            else
                Debug.Log("Pawn ����� �߿� GetColor �Ű������� Null�� ǥ�õǾ����ϴ�.");
        }
    }

    void SetKnight(GameColor getColor)
    {
        Piece piecePrefab1 = null;
        Piece piecePrefab2 = null;


        if (getColor == GameColor.Black)
        {
            piecePrefab1 = Instantiate(blackKnightPrefab, blackPiecesParent).GetComponent<Piece>();
            piecePrefab2 = Instantiate(blackKnightPrefab, blackPiecesParent).GetComponent<Piece>();

            piecePrefab1.transform.position = new Vector2(1, 7);
            piecePrefab2.transform.position = new Vector2(6, 7);


            chessTileList[1, 7].locatedPiece = piecePrefab1;
            chessTileList[6, 7].locatedPiece = piecePrefab2;
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteKnightPrefab, whitePiecesParent).GetComponent<Piece>();
            piecePrefab2 = Instantiate(whiteKnightPrefab, whitePiecesParent).GetComponent<Piece>();

            piecePrefab1.transform.position = new Vector2(1, 0);
            piecePrefab2.transform.position = new Vector2(6, 0);
        }
        else
            Debug.Log("Knight ����� �߿� GetColor �Ű������� Null�� ǥ�õǾ����ϴ�.");

    }

    void SetBishop(GameColor getColor)
    {
        Piece piecePrefab1 = null;
        Piece piecePrefab2 = null;


        if (getColor == GameColor.Black)
        {
            piecePrefab1 = Instantiate(blackBishopPrefab, blackPiecesParent).GetComponent<Piece>();
            piecePrefab2 = Instantiate(blackBishopPrefab, blackPiecesParent).GetComponent<Piece>();

            piecePrefab1.transform.position = new Vector2(2, 7);
            piecePrefab2.transform.position = new Vector2(5, 7);
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteBishopPrefab, whitePiecesParent).GetComponent<Piece>();
            piecePrefab2 = Instantiate(whiteBishopPrefab, whitePiecesParent).GetComponent<Piece>();

            piecePrefab1.transform.position = new Vector2(2, 0);
            piecePrefab2.transform.position = new Vector2(5, 0);
        }
        else
            Debug.Log("Bishop ����� �߿� GetColor �Ű������� Null�� ǥ�õǾ����ϴ�.");

    }

    void SetRook(GameColor getColor)
    {
        Piece piecePrefab1 = null;
        Piece piecePrefab2 = null;


        if (getColor == GameColor.Black)
        {
            piecePrefab1 = Instantiate(blackRookPrefab, blackPiecesParent).GetComponent<Piece>();
            piecePrefab2 = Instantiate(blackRookPrefab, blackPiecesParent).GetComponent<Piece>();

            piecePrefab1.transform.position = new Vector2(0, 7);
            piecePrefab2.transform.position = new Vector2(7, 7);
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteRookPrefab, whitePiecesParent).GetComponent<Piece>();
            piecePrefab2 = Instantiate(whiteRookPrefab, whitePiecesParent).GetComponent<Piece>();

            piecePrefab1.transform.position = new Vector2(0, 0);
            piecePrefab2.transform.position = new Vector2(7, 0);
        }
        else
            Debug.Log("Rook ����� �߿� GetColor �Ű������� Null�� ǥ�õǾ����ϴ�.");

    }

    void SetQueen(GameColor getColor)
    {
        Piece piecePrefab1 = null;


        if (getColor == GameColor.Black)
        {
            piecePrefab1 = Instantiate(blackQueenPrefab, blackPiecesParent).GetComponent<Piece>();
            piecePrefab1.transform.position = new Vector2(3, 7);
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteQueenPrefab, whitePiecesParent).GetComponent<Piece>();
            piecePrefab1.transform.position = new Vector2(3, 0);
        }
        else
            Debug.Log("Queen ����� �߿� GetColor �Ű������� Null�� ǥ�õǾ����ϴ�.");

    }

    void SetKing(GameColor getColor)
    {
        Piece piecePrefab1 = null;

        if (getColor == GameColor.Black)
        {
            piecePrefab1 = Instantiate(blackKingPrefab, blackPiecesParent).GetComponent<Piece>();

            piecePrefab1.transform.position = new Vector2(4, 7);
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteKingPrefab, whitePiecesParent).GetComponent<Piece>();

            piecePrefab1.transform.position = new Vector2(4, 0);
        }
        else
            Debug.Log("King ����� �߿� GetColor �Ű������� Null�� ǥ�õǾ����ϴ�.");

    }
    #endregion
}
