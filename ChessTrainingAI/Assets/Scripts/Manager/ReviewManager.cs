using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Header("UI")]
    // 리뷰씬에서 사용하는 기보 움직임 버튼
    public Button exitBtn;
    public Button prevBtn;
    public Button nextBtn;

    public JsonNotationWrapper nowReviewNotation;
    int nowState = 0;

    public static ReviewManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        exitBtn.onClick.AddListener(ExitReview);
        prevBtn.onClick.AddListener(SetPrevState);
        nextBtn.onClick.AddListener(SetNextState);
    }

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
                piecePrefab.nowPos = new Vector2Int(i, 6);

                chessTileList[i, 6].locatedPiece = piecePrefab;
            }
            else if (getColor == GameColor.White)
            {
                piecePrefab = Instantiate(whitePawnPrefab, whitePiecesParent).GetComponent<Piece>();
                piecePrefab.transform.position = new Vector2(i, 1);
                piecePrefab.nowPos = new Vector2Int(i, 1);

                chessTileList[i, 1].locatedPiece = piecePrefab;
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

            piecePrefab1.nowPos = new Vector2Int(1, 7);
            piecePrefab2.nowPos = new Vector2Int(6, 7);


            chessTileList[1, 7].locatedPiece = piecePrefab1;
            chessTileList[6, 7].locatedPiece = piecePrefab2;
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteKnightPrefab, whitePiecesParent).GetComponent<Piece>();
            piecePrefab2 = Instantiate(whiteKnightPrefab, whitePiecesParent).GetComponent<Piece>();

            piecePrefab1.transform.position = new Vector2(1, 0);
            piecePrefab2.transform.position = new Vector2(6, 0);

            piecePrefab1.nowPos = new Vector2Int(1, 0);
            piecePrefab2.nowPos = new Vector2Int(6, 0);

            chessTileList[1, 0].locatedPiece = piecePrefab1;
            chessTileList[6, 0].locatedPiece = piecePrefab2;
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

            piecePrefab1.nowPos = new Vector2Int(2, 7);
            piecePrefab2.nowPos = new Vector2Int(5, 7);

            chessTileList[2, 7].locatedPiece = piecePrefab1;
            chessTileList[5, 7].locatedPiece = piecePrefab2;
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteBishopPrefab, whitePiecesParent).GetComponent<Piece>();
            piecePrefab2 = Instantiate(whiteBishopPrefab, whitePiecesParent).GetComponent<Piece>();

            piecePrefab1.transform.position = new Vector2(2, 0);
            piecePrefab2.transform.position = new Vector2(5, 0);

            piecePrefab1.nowPos = new Vector2Int(2, 0);
            piecePrefab2.nowPos = new Vector2Int(5, 0);

            chessTileList[2, 0].locatedPiece = piecePrefab1;
            chessTileList[5, 0].locatedPiece = piecePrefab2;
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

            piecePrefab1.nowPos = new Vector2Int(0, 7);
            piecePrefab2.nowPos = new Vector2Int(7, 7);

            King nowKing = GameObject.Find("BlackKing(Clone)").GetComponent<King>();
            nowKing.nowRooks[0] = piecePrefab1.GetComponent<Rook>();
            nowKing.nowRooks[1] = piecePrefab2.GetComponent<Rook>();

            chessTileList[0, 7].locatedPiece = piecePrefab1;
            chessTileList[7, 7].locatedPiece = piecePrefab2;
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteRookPrefab, whitePiecesParent).GetComponent<Piece>();
            piecePrefab2 = Instantiate(whiteRookPrefab, whitePiecesParent).GetComponent<Piece>();

            piecePrefab1.transform.position = new Vector2(0, 0);
            piecePrefab2.transform.position = new Vector2(7, 0);

            piecePrefab1.nowPos = new Vector2Int(0, 0);
            piecePrefab2.nowPos = new Vector2Int(7, 0);

            King nowKing = GameObject.Find("WhiteKing(Clone)").GetComponent<King>();
            nowKing.nowRooks[0] = piecePrefab1.GetComponent<Rook>();
            nowKing.nowRooks[1] = piecePrefab2.GetComponent<Rook>();

            chessTileList[0, 0].locatedPiece = piecePrefab1;
            chessTileList[7, 0].locatedPiece = piecePrefab2;
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
            piecePrefab1.nowPos = new Vector2Int(3, 7);

            chessTileList[3, 7].locatedPiece = piecePrefab1;
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteQueenPrefab, whitePiecesParent).GetComponent<Piece>();
            piecePrefab1.transform.position = new Vector2(3, 0);
            piecePrefab1.nowPos = new Vector2Int(3, 0);

            chessTileList[3, 0].locatedPiece = piecePrefab1;
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
            piecePrefab1.nowPos = new Vector2Int(4, 7);

            chessTileList[4, 7].locatedPiece = piecePrefab1;
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteKingPrefab, whitePiecesParent).GetComponent<Piece>();

            piecePrefab1.transform.position = new Vector2(4, 0);
            piecePrefab1.nowPos = new Vector2Int(4, 0);

            chessTileList[4, 0].locatedPiece = piecePrefab1;
        }
        else
            Debug.Log("King 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

    }
    #endregion

    #region 기보 순서 따라가기 함수
    void SetPrevState()
    {
        if(nowState == 0)
        {
            return;
        }


    }

    public void SetNextState()
    {
        Debug.Log(11);
        if(nowState >= nowReviewNotation.list.Count)
        {
            return;
        }

        MovePiece(nowReviewNotation.list[nowState]);
        nowState++;
    }

    void MovePiece(NotationInfo getInfo)
    {
        TIleName startTileName = (TIleName)Enum.Parse(typeof(TIleName), getInfo.startPos.ToString());
        TIleName endTileName = (TIleName)Enum.Parse(typeof(TIleName), getInfo.endPos.ToString());

        Vector2Int startPos = new Vector2Int((int)startTileName % 8, (int)startTileName / 8);
        Vector2Int endPos = new Vector2Int((int)endTileName % 8, (int)endTileName / 8);
        Debug.Log(startTileName + " " + endTileName);
        Debug.Log(startPos + " " + endPos);
        if(chessTileList[endPos.x, endPos.y].locatedPiece != null)
            Destroy(chessTileList[endPos.x, endPos.y].locatedPiece.gameObject);
        chessTileList[endPos.x, endPos.y].locatedPiece = chessTileList[startPos.x, startPos.y].locatedPiece;
        chessTileList[startPos.x, startPos.y].locatedPiece.transform.position = new Vector3(endPos.x, endPos.y, 0);
        chessTileList[startPos.x, startPos.y].locatedPiece = null;
    }

    #endregion
    public void ExitReview()
    {
        SceneManager.LoadScene("StartScene");
    }
}
