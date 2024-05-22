using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerColor
{ 
    Null, White, Black
}

public class ChessManager : MonoBehaviour
{
    public static ChessManager chessManager;

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    GameObject tilePrefab;
    [SerializeField]
    Transform chessBoardParent;
    [SerializeField]
    public Tile[,] chessTileList = new Tile[8,8];

    //Pieces Prefabs
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

    public PlayerColor playerColor = PlayerColor.Null;
    RaycastHit2D hit;
    public Piece nowPiece;

    void Awake()
    {
        if (chessManager == null)
        {
            chessManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetChessBoard();
        SetChessPiece();
    }

    private void Update()
    {
        MovePiece();
    }

    #region Set Chess Board
    void SetChessBoard()
    {
        bool colorChange = true;
        GameObject tile;

        for(int col = 0; col < 8; col++)
            for(int row = 0; row < 8; row++)
            {
                //타일을 생성하고 chessTileList에 삽입
                tile = Instantiate(tilePrefab, chessBoardParent);
                chessTileList[row, col] = tile.GetComponent<Tile>();
                chessTileList[row, col].gameObject.GetComponent<Transform>().position = new Vector2(row, col);

                //타일 색을 변경
                if (colorChange)
                    chessTileList[row, col].GetComponent<SpriteRenderer>().color = new Color(115 / 255f, 144 / 255f, 83 / 255f);
                else
                    chessTileList[row, col].GetComponent<SpriteRenderer>().color = new Color(235 / 255f, 236 / 255f, 208 / 255f);

                //다음 Eow의 0번 Col의 색깔이 이전 Row의 7번 Col과 동일하므로 변화하지 않도록 고정하는 부분
                if (row != 7)
                    colorChange = !colorChange;
            }
    }

    void SetChessPiece()
    {
        SetPawn(PlayerColor.White);
        SetKnight(PlayerColor.White);
        SetBishop(PlayerColor.White);
        SetRook(PlayerColor.White);
        SetQueen(PlayerColor.White);
        SetKing(PlayerColor.White);

        SetPawn(PlayerColor.Black);
        SetKnight(PlayerColor.Black);
        SetBishop(PlayerColor.Black);
        SetRook(PlayerColor.Black);
        SetQueen(PlayerColor.Black);
        SetKing(PlayerColor.Black);
    }

    void SetPawn(PlayerColor getColor)
    {
        GameObject piecePrefab = null;

        //검은색 기물 지정
        for (int i = 0; i < 8; i++)
        {
            if (getColor == PlayerColor.Black)
            {
                piecePrefab = Instantiate(blackPawnPrefab, blackPiecesParent);
                piecePrefab.GetComponent<Transform>().position = new Vector2(i, 6);
                piecePrefab.GetComponent<Piece>().nowPos = new Vector2Int(i, 6);
                piecePrefab.GetComponent<Piece>().pieceType = PieceType.BlackPawn;

                chessTileList[i, 6].nowLocateColor = getColor;
            }
            else if (getColor == PlayerColor.White)
            {
                piecePrefab = Instantiate(whitePawnPrefab, whitePiecesParent);
                piecePrefab.GetComponent<Transform>().position = new Vector2(i, 1);
                piecePrefab.GetComponent<Piece>().nowPos = new Vector2Int(i, 1);
                piecePrefab.GetComponent<Piece>().pieceType = PieceType.WhitePawn;

                chessTileList[i, 1].nowLocateColor = getColor;
            }
            else
                Debug.Log("Pawn 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");
        }
    }

    void SetKnight(PlayerColor getColor)
    {
        GameObject piecePrefab1 = null;
        GameObject piecePrefab2 = null;


        if (getColor == PlayerColor.Black)
        {
            piecePrefab1 = Instantiate(blackKnightPrefab, blackPiecesParent);
            piecePrefab2 = Instantiate(blackKnightPrefab, blackPiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(1, 7);
            piecePrefab2.GetComponent<Transform>().position = new Vector2(6, 7);

            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(1, 7);
            piecePrefab2.GetComponent<Piece>().nowPos = new Vector2Int(6, 7);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.BlackKnight;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.BlackKnight;


            chessTileList[1, 7].nowLocateColor = getColor;
            chessTileList[6, 7].nowLocateColor = getColor;
        }
        else if (getColor == PlayerColor.White)
        {
            piecePrefab1 = Instantiate(whiteKnightPrefab, whitePiecesParent);
            piecePrefab2 = Instantiate(whiteKnightPrefab, whitePiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(1, 0);
            piecePrefab2.GetComponent<Transform>().position = new Vector2(6, 0);

            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(1, 0);
            piecePrefab2.GetComponent<Piece>().nowPos = new Vector2Int(6, 0);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.WhiteKnight;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.WhiteKnight;


            chessTileList[1, 0].nowLocateColor = getColor;
            chessTileList[6, 0].nowLocateColor = getColor;
        }
        else
            Debug.Log("Knight 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

    }

    void SetBishop(PlayerColor getColor)
    {
        GameObject piecePrefab1 = null;
        GameObject piecePrefab2 = null;


        if (getColor == PlayerColor.Black)
        {
            piecePrefab1 = Instantiate(blackBishopPrefab, blackPiecesParent);
            piecePrefab2 = Instantiate(blackBishopPrefab, blackPiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(2, 7);
            piecePrefab2.GetComponent<Transform>().position = new Vector2(5, 7);

            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(2, 7);
            piecePrefab2.GetComponent<Piece>().nowPos = new Vector2Int(5, 7);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.BlackBishop;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.BlackBishop;


            chessTileList[2, 7].nowLocateColor = getColor;
            chessTileList[5, 7].nowLocateColor = getColor;
        }
        else if (getColor == PlayerColor.White)
        {
            piecePrefab1 = Instantiate(whiteBishopPrefab, whitePiecesParent);
            piecePrefab2 = Instantiate(whiteBishopPrefab, whitePiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(2, 0);
            piecePrefab2.GetComponent<Transform>().position = new Vector2(5, 0);

            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(2, 0);
            piecePrefab2.GetComponent<Piece>().nowPos = new Vector2Int(5, 0);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.WhiteBishop;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.WhiteBishop;

            chessTileList[2, 0].nowLocateColor = getColor;
            chessTileList[5, 0].nowLocateColor = getColor;
        }
        else
            Debug.Log("Bishop 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

    }

    void SetRook(PlayerColor getColor)
    {
        GameObject piecePrefab1 = null;
        GameObject piecePrefab2 = null;


        if (getColor == PlayerColor.Black)
        {
            piecePrefab1 = Instantiate(blackRookPrefab, blackPiecesParent);
            piecePrefab2 = Instantiate(blackRookPrefab, blackPiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(0, 7);
            piecePrefab2.GetComponent<Transform>().position = new Vector2(7, 7);

            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(0, 7);
            piecePrefab2.GetComponent<Piece>().nowPos = new Vector2Int(7, 7);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.BlackRook;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.BlackRook;

            chessTileList[0, 7].nowLocateColor = getColor;
            chessTileList[7, 7].nowLocateColor = getColor;
        }
        else if (getColor == PlayerColor.White)
        {
            piecePrefab1 = Instantiate(whiteRookPrefab, whitePiecesParent);
            piecePrefab2 = Instantiate(whiteRookPrefab, whitePiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(0, 0);
            piecePrefab2.GetComponent<Transform>().position = new Vector2(7, 0);

            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(0, 0);
            piecePrefab2.GetComponent<Piece>().nowPos = new Vector2Int(7, 0);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.WhilteRook;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.WhilteRook;

            chessTileList[0, 0].nowLocateColor = getColor;
            chessTileList[7, 0].nowLocateColor = getColor;
        }
        else
            Debug.Log("Rook 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

    }

    void SetQueen(PlayerColor getColor)
    {
        GameObject piecePrefab1 = null;


        if (getColor == PlayerColor.Black)
        {
            piecePrefab1 = Instantiate(blackQueenPrefab, blackPiecesParent);
            piecePrefab1.GetComponent<Transform>().position = new Vector2(3, 7);
            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(3, 7);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.BlackQueen;

            chessTileList[3, 7].nowLocateColor = getColor;
        }
        else if (getColor == PlayerColor.White)
        {
            piecePrefab1 = Instantiate(whiteQueenPrefab, whitePiecesParent);
            piecePrefab1.GetComponent<Transform>().position = new Vector2(3, 0);
            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(3, 0);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.WhiteQueen;

            chessTileList[3, 0].nowLocateColor = getColor;
        }
        else
            Debug.Log("Queen 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

    }

    void SetKing(PlayerColor getColor)
    {
        GameObject piecePrefab1 = null;


        if (getColor == PlayerColor.Black)
        {
            piecePrefab1 = Instantiate(blackKingPrefab, blackPiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(4, 7);
            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(4, 7);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.BlackKing;

            chessTileList[4, 7].nowLocateColor = getColor;
        }
        else if (getColor == PlayerColor.White)
        {
            piecePrefab1 = Instantiate(whiteKingPrefab, whitePiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(4, 0);
            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(4, 0);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.WhiteKing;

            chessTileList[4, 0].nowLocateColor = getColor;
        }
        else
            Debug.Log("King 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

    }
    #endregion

    //게임을 진행하기 전 플레이어의 색을 미리 지정합니다.
    void SetPlayerColor()
    {
        float randomNum = Random.Range(0, 10);
        if (randomNum < 5)
            playerColor = PlayerColor.White;
        else
            playerColor = PlayerColor.Black;
    }

    void MovePiece()
    {
        //Piece가 선택되지 않았으면 아무 일도 일어나지 않음
        if (nowPiece == null)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(rayPos, Vector2.zero);

            nowPiece.Move(hit.collider.GetComponent<Tile>());
        }
    }
}
