using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameColor
{ 
    Null, White, Black
}

public class ChessManager : MonoBehaviour
{
    public static ChessManager chessManager;

    [SerializeField]
    Camera mainCamera;          //흑백에 따라 보는 관점 바꾸려고 가져온 카메라

    #region 타일, 보드판 정보
    [SerializeField]
    GameObject tilePrefab;
    [SerializeField]
    Transform chessBoardParent;
    [SerializeField]
    public Tile[,] chessTileList = new Tile[8,8];
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

    public GameColor playerColor = GameColor.Null;
    public GameColor nowTurnColor = GameColor.White;
    RaycastHit2D hit;
    public Piece nowPiece;

    #region 턴 이벤트
    UnityEvent turnStart;
    UnityEvent turnEnd;
    #endregion 턴 이벤트

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
        // 체스판, 기물 위치시키기
        SetChessBoard();
        SetChessPiece();
        SetPlayerColor();

        // 턴 함수 설정
        turnStart.AddListener(StartTurn);
        turnEnd.AddListener(EndTurn);

        // 게임 시작
        nowTurnColor = GameColor.White;
        turnStart?.Invoke();
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
        GameObject piecePrefab = null;

        //검은색 기물 지정
        for (int i = 0; i < 8; i++)
        {
            if (getColor == GameColor.Black)
            {
                piecePrefab = Instantiate(blackPawnPrefab, blackPiecesParent);
                piecePrefab.GetComponent<Transform>().position = new Vector2(i, 6);
                piecePrefab.GetComponent<Piece>().nowPos = new Vector2Int(i, 6);
                piecePrefab.GetComponent<Piece>().pieceType = PieceType.Pawn;
                piecePrefab.GetComponent<Piece>().pieceColor = GameColor.Black;

                chessTileList[i, 6].nowLocateColor = getColor;
            }
            else if (getColor == GameColor.White)
            {
                piecePrefab = Instantiate(whitePawnPrefab, whitePiecesParent);
                piecePrefab.GetComponent<Transform>().position = new Vector2(i, 1);
                piecePrefab.GetComponent<Piece>().nowPos = new Vector2Int(i, 1);
                piecePrefab.GetComponent<Piece>().pieceType = PieceType.Pawn;
                piecePrefab.GetComponent<Piece>().pieceColor = GameColor.White;

                chessTileList[i, 1].nowLocateColor = getColor;
            }
            else
                Debug.Log("Pawn 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");
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
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.Knight;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.Knight;
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
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.Knight;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.Knight;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.White;
            piecePrefab2.GetComponent<Piece>().pieceColor = GameColor.White;


            chessTileList[1, 0].nowLocateColor = getColor;
            chessTileList[6, 0].nowLocateColor = getColor;
        }
        else
            Debug.Log("Knight 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

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
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.Bishop;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.Bishop;
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
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.Bishop;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.Bishop;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.White;
            piecePrefab2.GetComponent<Piece>().pieceColor = GameColor.White;

            chessTileList[2, 0].nowLocateColor = getColor;
            chessTileList[5, 0].nowLocateColor = getColor;
        }
        else
            Debug.Log("Bishop 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

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
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.Rook;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.Rook;
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
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.Rook;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.Rook;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.White;
            piecePrefab2.GetComponent<Piece>().pieceColor = GameColor.White;

            King nowKing = GameObject.Find("WhiteKing(Clone)").GetComponent<King>();
            nowKing.nowRooks[0] = piecePrefab1.GetComponent<Rook>();
            nowKing.nowRooks[1] = piecePrefab2.GetComponent<Rook>();

            chessTileList[0, 0].nowLocateColor = getColor;
            chessTileList[7, 0].nowLocateColor = getColor;
        }
        else
            Debug.Log("Rook 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

    }

    void SetQueen(GameColor getColor)
    {
        GameObject piecePrefab1 = null;


        if (getColor == GameColor.Black)
        {
            piecePrefab1 = Instantiate(blackQueenPrefab, blackPiecesParent);
            piecePrefab1.GetComponent<Transform>().position = new Vector2(3, 7);
            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(3, 7);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.Queen;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.Black;

            chessTileList[3, 7].nowLocateColor = getColor;
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteQueenPrefab, whitePiecesParent);
            piecePrefab1.GetComponent<Transform>().position = new Vector2(3, 0);
            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(3, 0);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.Queen;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.White;

            chessTileList[3, 0].nowLocateColor = getColor;
        }
        else
            Debug.Log("Queen 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

    }

    void SetKing(GameColor getColor)
    {
        GameObject piecePrefab1 = null;


        if (getColor == GameColor.Black)
        {
            piecePrefab1 = Instantiate(blackKingPrefab, blackPiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(4, 7);
            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(4, 7);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.King;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.Black;

            chessTileList[4, 7].nowLocateColor = getColor;
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteKingPrefab, whitePiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(4, 0);
            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(4, 0);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.King;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.White;

            chessTileList[4, 0].nowLocateColor = getColor;
        }
        else
            Debug.Log("King 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

    }
    #endregion

    #region 턴 관련 함수
    public void StartTurn()
    {
        // 1. 
        // 2. 
    }

    //턴 종료
    public void EndTurn()
    {
        // 1. 공격 타일 설정
        SetAttackTiles();

        // 2. 이제 턴을 진행할 색 지정
        nowTurnColor = (nowTurnColor == GameColor.White) ? GameColor.Black : GameColor.White;
    }
    #endregion 턴 관련 함수

    //게임을 진행하기 전 플레이어의 색을 미리 지정합니다.
    void SetPlayerColor()
    {
        float randomNum = UnityEngine.Random.Range(0, 10);
        if (randomNum < 5)
            playerColor = GameColor.White;
        else
            playerColor = GameColor.Black;

        if (playerColor == GameColor.White)
            return;

        // 카메라 설정
        mainCamera.transform.rotation = Quaternion.Euler(0, 0, 180);
        for (int i = 0; i < 16; i++)
        {
            whitePiecesParent.GetChild(i).transform.rotation = Quaternion.Euler(180, 0, 0);
            blackPiecesParent.GetChild(i).transform.rotation = Quaternion.Euler(180, 0, 0);

            whitePiecesParent.GetChild(i).GetComponent<Piece>().SetAttackTile();
            blackPiecesParent.GetChild(i).GetComponent<Piece>().SetAttackTile();
        }
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
