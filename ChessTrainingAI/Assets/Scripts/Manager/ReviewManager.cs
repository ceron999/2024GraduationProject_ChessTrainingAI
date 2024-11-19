using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewManager : MonoBehaviour
{
    #region 타일, 보드판 정보
    [SerializeField]
    GameObject tilePrefab;
    [SerializeField]
    Transform chessBoardParent;
    [SerializeField]
    public Tile[,] chessTileList = new Tile[8, 8];
    #endregion 타일, 보드판 정보

    #region 기물 프리팹
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
    #endregion 기물 프리팹

    private void Start()
    {
        SetChessBoard();
        SetChessPiece();
    }

    #region 보드판 생성 및 기물 생성
    void SetChessBoard()
    {
        bool colorChange = true;
        GameObject tile;

        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                // 타일을 생성하고 chessTileList에 삽입
                tile = Instantiate(tilePrefab, chessBoardParent);
                chessTileList[row, col] = tile.GetComponent<Tile>();
                chessTileList[row, col].gameObject.GetComponent<Transform>().position = new Vector2(row, col);

                // 타일 이름 설정
                chessTileList[row, col].tileName = (TIleName)(row + col * 8);

                // 타일 색을 변경
                if (colorChange)
                    chessTileList[row, col].GetComponent<SpriteRenderer>().color = new Color(115 / 255f, 144 / 255f, 83 / 255f);
                else
                    chessTileList[row, col].GetComponent<SpriteRenderer>().color = new Color(235 / 255f, 236 / 255f, 208 / 255f);

                // 다음 Eow의 0번 Col의 색깔이 이전 Row의 7번 Col과 동일하므로 변화하지 않도록 고정하는 부분
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

        //검은색 기물 지정
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
                Debug.Log("Pawn 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");
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
            Debug.Log("Knight 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

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
            Debug.Log("Bishop 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

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
            Debug.Log("Rook 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

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
            Debug.Log("Queen 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

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
            Debug.Log("King 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

    }
    #endregion
}
