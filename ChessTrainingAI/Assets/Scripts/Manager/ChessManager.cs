using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameColor
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

    public GameColor playerColor = GameColor.Null;
    public GameColor nowTurnColor = GameColor.White;
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
        SetPlayerTurn();
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
                //Ÿ���� �����ϰ� chessTileList�� ����
                tile = Instantiate(tilePrefab, chessBoardParent);
                chessTileList[row, col] = tile.GetComponent<Tile>();
                chessTileList[row, col].gameObject.GetComponent<Transform>().position = new Vector2(row, col);

                //Ÿ�� ���� ����
                if (colorChange)
                    chessTileList[row, col].GetComponent<SpriteRenderer>().color = new Color(115 / 255f, 144 / 255f, 83 / 255f);
                else
                    chessTileList[row, col].GetComponent<SpriteRenderer>().color = new Color(235 / 255f, 236 / 255f, 208 / 255f);

                //���� Eow�� 0�� Col�� ������ ���� Row�� 7�� Col�� �����ϹǷ� ��ȭ���� �ʵ��� �����ϴ� �κ�
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
        GameObject piecePrefab = null;

        //������ �⹰ ����
        for (int i = 0; i < 8; i++)
        {
            if (getColor == GameColor.Black)
            {
                piecePrefab = Instantiate(blackPawnPrefab, blackPiecesParent);
                piecePrefab.GetComponent<Transform>().position = new Vector2(i, 6);
                piecePrefab.GetComponent<Piece>().nowPos = new Vector2Int(i, 6);
                piecePrefab.GetComponent<Piece>().pieceType = PieceType.BlackPawn;
                piecePrefab.GetComponent<Piece>().pieceColor = GameColor.Black;

                chessTileList[i, 6].nowLocateColor = getColor;
            }
            else if (getColor == GameColor.White)
            {
                piecePrefab = Instantiate(whitePawnPrefab, whitePiecesParent);
                piecePrefab.GetComponent<Transform>().position = new Vector2(i, 1);
                piecePrefab.GetComponent<Piece>().nowPos = new Vector2Int(i, 1);
                piecePrefab.GetComponent<Piece>().pieceType = PieceType.WhitePawn;
                piecePrefab.GetComponent<Piece>().pieceColor = GameColor.White;

                chessTileList[i, 1].nowLocateColor = getColor;
            }
            else
                Debug.Log("Pawn ����� �߿� GetColor �Ű������� Null�� ǥ�õǾ����ϴ�.");
        }
    }

    void SetKnight(GameColor getColor)
    {
        GameObject piecePrefab1 = null;
        GameObject piecePrefab2 = null;


        if (getColor == GameColor.Black)
        {
            piecePrefab1 = Instantiate(blackKnightPrefab, blackPiecesParent);
            piecePrefab2 = Instantiate(blackKnightPrefab, blackPiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(1, 7);
            piecePrefab2.GetComponent<Transform>().position = new Vector2(6, 7);

            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(1, 7);
            piecePrefab2.GetComponent<Piece>().nowPos = new Vector2Int(6, 7);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.BlackKnight;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.BlackKnight;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.Black;
            piecePrefab2.GetComponent<Piece>().pieceColor = GameColor.Black;


            chessTileList[1, 7].nowLocateColor = getColor;
            chessTileList[6, 7].nowLocateColor = getColor;
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteKnightPrefab, whitePiecesParent);
            piecePrefab2 = Instantiate(whiteKnightPrefab, whitePiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(1, 0);
            piecePrefab2.GetComponent<Transform>().position = new Vector2(6, 0);

            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(1, 0);
            piecePrefab2.GetComponent<Piece>().nowPos = new Vector2Int(6, 0);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.WhiteKnight;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.WhiteKnight;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.White;
            piecePrefab2.GetComponent<Piece>().pieceColor = GameColor.White;


            chessTileList[1, 0].nowLocateColor = getColor;
            chessTileList[6, 0].nowLocateColor = getColor;
        }
        else
            Debug.Log("Knight ����� �߿� GetColor �Ű������� Null�� ǥ�õǾ����ϴ�.");

    }

    void SetBishop(GameColor getColor)
    {
        GameObject piecePrefab1 = null;
        GameObject piecePrefab2 = null;


        if (getColor == GameColor.Black)
        {
            piecePrefab1 = Instantiate(blackBishopPrefab, blackPiecesParent);
            piecePrefab2 = Instantiate(blackBishopPrefab, blackPiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(2, 7);
            piecePrefab2.GetComponent<Transform>().position = new Vector2(5, 7);

            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(2, 7);
            piecePrefab2.GetComponent<Piece>().nowPos = new Vector2Int(5, 7);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.BlackBishop;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.BlackBishop;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.Black;
            piecePrefab2.GetComponent<Piece>().pieceColor = GameColor.Black;


            chessTileList[2, 7].nowLocateColor = getColor;
            chessTileList[5, 7].nowLocateColor = getColor;
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteBishopPrefab, whitePiecesParent);
            piecePrefab2 = Instantiate(whiteBishopPrefab, whitePiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(2, 0);
            piecePrefab2.GetComponent<Transform>().position = new Vector2(5, 0);

            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(2, 0);
            piecePrefab2.GetComponent<Piece>().nowPos = new Vector2Int(5, 0);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.WhiteBishop;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.WhiteBishop;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.White;
            piecePrefab2.GetComponent<Piece>().pieceColor = GameColor.White;

            chessTileList[2, 0].nowLocateColor = getColor;
            chessTileList[5, 0].nowLocateColor = getColor;
        }
        else
            Debug.Log("Bishop ����� �߿� GetColor �Ű������� Null�� ǥ�õǾ����ϴ�.");

    }

    void SetRook(GameColor getColor)
    {
        GameObject piecePrefab1 = null;
        GameObject piecePrefab2 = null;


        if (getColor == GameColor.Black)
        {
            piecePrefab1 = Instantiate(blackRookPrefab, blackPiecesParent);
            piecePrefab2 = Instantiate(blackRookPrefab, blackPiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(0, 7);
            piecePrefab2.GetComponent<Transform>().position = new Vector2(7, 7);

            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(0, 7);
            piecePrefab2.GetComponent<Piece>().nowPos = new Vector2Int(7, 7);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.BlackRook;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.BlackRook;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.Black;
            piecePrefab2.GetComponent<Piece>().pieceColor = GameColor.Black;

            King nowKing = GameObject.Find("BlackKing(Clone)").GetComponent<King>();
            nowKing.nowRooks[0] = piecePrefab1.GetComponent<Rook>();
            nowKing.nowRooks[1] = piecePrefab2.GetComponent<Rook>();

            chessTileList[0, 7].nowLocateColor = getColor;
            chessTileList[7, 7].nowLocateColor = getColor;
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteRookPrefab, whitePiecesParent);
            piecePrefab2 = Instantiate(whiteRookPrefab, whitePiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(0, 0);
            piecePrefab2.GetComponent<Transform>().position = new Vector2(7, 0);

            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(0, 0);
            piecePrefab2.GetComponent<Piece>().nowPos = new Vector2Int(7, 0);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.WhiteRook;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.WhiteRook;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.White;
            piecePrefab2.GetComponent<Piece>().pieceColor = GameColor.White;

            King nowKing = GameObject.Find("WhiteKing(Clone)").GetComponent<King>();
            nowKing.nowRooks[0] = piecePrefab1.GetComponent<Rook>();
            nowKing.nowRooks[1] = piecePrefab2.GetComponent<Rook>();

            chessTileList[0, 0].nowLocateColor = getColor;
            chessTileList[7, 0].nowLocateColor = getColor;
        }
        else
            Debug.Log("Rook ����� �߿� GetColor �Ű������� Null�� ǥ�õǾ����ϴ�.");

    }

    void SetQueen(GameColor getColor)
    {
        GameObject piecePrefab1 = null;


        if (getColor == GameColor.Black)
        {
            piecePrefab1 = Instantiate(blackQueenPrefab, blackPiecesParent);
            piecePrefab1.GetComponent<Transform>().position = new Vector2(3, 7);
            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(3, 7);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.BlackQueen;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.Black;

            chessTileList[3, 7].nowLocateColor = getColor;
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteQueenPrefab, whitePiecesParent);
            piecePrefab1.GetComponent<Transform>().position = new Vector2(3, 0);
            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(3, 0);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.WhiteQueen;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.White;

            chessTileList[3, 0].nowLocateColor = getColor;
        }
        else
            Debug.Log("Queen ����� �߿� GetColor �Ű������� Null�� ǥ�õǾ����ϴ�.");

    }

    void SetKing(GameColor getColor)
    {
        GameObject piecePrefab1 = null;


        if (getColor == GameColor.Black)
        {
            piecePrefab1 = Instantiate(blackKingPrefab, blackPiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(4, 7);
            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(4, 7);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.BlackKing;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.Black;

            chessTileList[4, 7].nowLocateColor = getColor;
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteKingPrefab, whitePiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(4, 0);
            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(4, 0);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.WhiteKing;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.White;

            chessTileList[4, 0].nowLocateColor = getColor;
        }
        else
            Debug.Log("King ����� �߿� GetColor �Ű������� Null�� ǥ�õǾ����ϴ�.");

    }
    #endregion

    //�÷��̾ �������� �İ����� ���ϰ� �׿� �°� ü���� ����
    void SetPlayerTurn()
    {
        SetPlayerColor();

        if (playerColor == GameColor.White)
            return;

        mainCamera.transform.rotation = Quaternion.Euler(0, 0, 180);
        for (int i = 0; i < 16; i++)
        {
            whitePiecesParent.GetChild(i).transform.rotation = Quaternion.Euler(180, 0, 0);
            blackPiecesParent.GetChild(i).transform.rotation = Quaternion.Euler(180, 0, 0);

            whitePiecesParent.GetChild(i).GetComponent<Piece>().SetAttackTile();
            blackPiecesParent.GetChild(i).GetComponent<Piece>().SetAttackTile();
        }
    }

    //������ �����ϱ� �� �÷��̾��� ���� �̸� �����մϴ�.
    void SetPlayerColor()
    {
        float randomNum = Random.Range(0, 10);
        if (randomNum < 5)
            playerColor = GameColor.White;
        else
            playerColor = GameColor.Black;
    }

    void MovePiece()
    {
        //Piece�� ���õ��� �ʾ����� �ƹ� �ϵ� �Ͼ�� ����
        if (nowPiece == null)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(rayPos, Vector2.zero);

            nowPiece.Move(hit.collider.GetComponent<Tile>());
        }
    }

    //�� ����
    public void EndTurn()
    {
        if (nowTurnColor == GameColor.White)
            nowTurnColor = GameColor.Black;
        else
            nowTurnColor = GameColor.White;

        //SetAttackTiles();
    }

    void SetAttackTiles()
    {
        Piece nowPiece = null;

        for(int i =0; i< blackPiecesParent.childCount; i++)
        {
            nowPiece = blackPiecesParent.GetChild(i).GetComponent<Piece>();
            nowPiece.SetAttackTile();
        }

        for (int i = 0; i < whitePiecesParent.childCount; i++)
        {
            nowPiece = whitePiecesParent.GetChild(i).GetComponent<Piece>();
            nowPiece.SetAttackTile();
        }
    }
}
