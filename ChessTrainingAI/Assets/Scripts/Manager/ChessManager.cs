using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.AI;
using TMPro;

public enum GameColor
{ 
    Null, White, Black
}

public class ChessManager : MonoBehaviour
{
    #region Singleton
    public static ChessManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    #region UI
    public GameObject promotionUI;
    public GameObject gameEndUI;
    public TextMeshProUGUI gameEndText;
    #endregion
    public Camera mainCamera;          //흑백에 따라 보는 관점 바꾸려고 가져온 카메라

    #region 타일, 보드판 정보
    [SerializeField]
    GameObject tilePrefab;
    [SerializeField]
    Transform chessBoardParent;
    [SerializeField]
    Transform testBoardParent;
    [SerializeField]
    public Tile[,] chessTileList = new Tile[8, 8];
    #endregion 타일, 보드판 정보

    #region 기물 프리팹
    [SerializeField]
    Transform whitePiecesParent;
    [SerializeField]
    Transform blackPiecesParent;
    [SerializeField]
    Transform testPiecesParent;
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
    public Piece nowPiece;
    #region 킹 오브젝트
    Piece whiteKing;
    Piece blackKing;
    #endregion

    #region 턴 이벤트
    public int nowTurn = 0;
    public UnityEvent gameStart;
    public UnityEvent turnEnd;
    public UnityEvent aiTurn;

    public UnityEvent check;
    public bool isCheck = false;
    public bool isCheckmate = false;
    #endregion 턴 이벤트

    #region 오류 방지 이벤트
    public bool isClickAvailable = true;
    #endregion 오류 방지 이벤트

    void Start()
    {
        // 체스판, 기물 위치시키기
        SetChessBoard();
        SetChessPiece();

        // 턴 함수 설정
        gameStart.AddListener(GameStart);
        turnEnd.AddListener(EndTurn);
        check.AddListener(Check);
        aiTurn.AddListener(StartAITurn);

        // 게임 시작
        nowTurnColor = GameColor.White;
        gameStart?.Invoke();
    }

    #region 체스 타일, 기물 설정
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
        //SetBishop(GameColor.Black);
        SetQueen(GameColor.Black);
        SetKing(GameColor.Black);
        SetRook(GameColor.Black);
    }

    void SetPiece(Piece getPiece, GameColor getColor ,Vector2Int getVector)
    {
        getPiece.transform.position = new Vector2(getVector.x, getVector.y);
        getPiece.nowPos = getVector;
        getPiece.pieceColor = getColor;

        chessTileList[getVector.x, getVector.y].locatedPiece = getPiece;

        Destroy(nowPiece.gameObject);
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
                piecePrefab.pieceType = PieceType.P;
                piecePrefab.pieceColor = GameColor.Black;
                piecePrefab.piecePoint = -1;

                chessTileList[i, 6].locatedPiece = piecePrefab;
            }
            else if (getColor == GameColor.White)
            {
                piecePrefab = Instantiate(whitePawnPrefab, whitePiecesParent).GetComponent<Piece>();
                piecePrefab.transform.position = new Vector2(i, 1);
                piecePrefab.nowPos = new Vector2Int(i, 1);
                piecePrefab.pieceType = PieceType.P;
                piecePrefab.pieceColor = GameColor.White;
                piecePrefab.piecePoint = 1;

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
            piecePrefab1.pieceType = PieceType.N;
            piecePrefab2.pieceType = PieceType.N;
            piecePrefab1.pieceColor = GameColor.Black;
            piecePrefab2.pieceColor = GameColor.Black;
            piecePrefab1.piecePoint = -3;
            piecePrefab2.piecePoint = -3;


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
            piecePrefab1.pieceType = PieceType.N;
            piecePrefab2.pieceType = PieceType.N;
            piecePrefab1.pieceColor = GameColor.White;
            piecePrefab2.pieceColor = GameColor.White;
            piecePrefab1.piecePoint = 3;
            piecePrefab2.piecePoint = 3;

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
            piecePrefab1.pieceType = PieceType.B;
            piecePrefab2.pieceType = PieceType.B;
            piecePrefab1.pieceColor = GameColor.Black;
            piecePrefab2.pieceColor = GameColor.Black;
            piecePrefab1.piecePoint = -3.5f;
            piecePrefab2.piecePoint = -3.5f;

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
            piecePrefab1.pieceType = PieceType.B;
            piecePrefab2.pieceType = PieceType.B;
            piecePrefab1.pieceColor = GameColor.White;
            piecePrefab2.pieceColor = GameColor.White;
            piecePrefab1.piecePoint = 3.5f;
            piecePrefab2.piecePoint = 3.5f;

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
            piecePrefab1.pieceType = PieceType.R;
            piecePrefab2.pieceType = PieceType.R;
            piecePrefab1.pieceColor = GameColor.Black;
            piecePrefab2.pieceColor = GameColor.Black;
            piecePrefab1.piecePoint = -5;
            piecePrefab2.piecePoint = -5;

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
            piecePrefab1.pieceType = PieceType.R;
            piecePrefab2.pieceType = PieceType.R;
            piecePrefab1.pieceColor = GameColor.White;
            piecePrefab2.pieceColor = GameColor.White;
            piecePrefab1.piecePoint = 5;
            piecePrefab2.piecePoint = 5;

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
            piecePrefab1.pieceType = PieceType.Q;
            piecePrefab1.pieceColor = GameColor.Black; 
            piecePrefab1.piecePoint = -8;

            chessTileList[3, 7].locatedPiece = piecePrefab1;
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteQueenPrefab, whitePiecesParent).GetComponent<Piece>();
            piecePrefab1.transform.position = new Vector2(3, 0);
            piecePrefab1.nowPos = new Vector2Int(3, 0);
            piecePrefab1.pieceType = PieceType.Q;
            piecePrefab1.pieceColor = GameColor.White;
            piecePrefab1.piecePoint = 8;

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
            piecePrefab1.pieceType = PieceType.K;
            piecePrefab1.pieceColor = GameColor.Black;
            piecePrefab1.piecePoint = -10000;

            chessTileList[4, 7].locatedPiece = piecePrefab1;
            blackKing = piecePrefab1;
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteKingPrefab, whitePiecesParent).GetComponent<Piece>();

            piecePrefab1.transform.position = new Vector2(4, 0);
            piecePrefab1.nowPos = new Vector2Int(4, 0);
            piecePrefab1.pieceType = PieceType.K;
            piecePrefab1.pieceColor = GameColor.White;
            piecePrefab1.piecePoint = 10000;

            chessTileList[4, 0].locatedPiece = piecePrefab1;
            whiteKing = piecePrefab1;
        }
        else
            Debug.Log("King 만드는 중에 GetColor 매개변수가 Null로 표시되었습니다.");

    }
    #endregion

    #region 게임 시작 및 턴 종료 함수
    void GameStart()
    {
        // 1. 플레이어 색 선정 후 카메라 이동
        SetPlayerColor();

        // 2. 기물의 이동 타일과 공격 기물 설정
        SetTilesInfo();
        SetPiecesInfo();
        SetKingInfo();
    }
    public void StartAITurn()
    {
        ChessAIManager.Instance.SetState();
        ChessAIManager.Instance.MovePiece();
    }

    //턴 종료
    public void EndTurn()
    {
        //Debug.Log("turnEnd");
        // 1. 이동 가능 타일 표시 기능 끄기
        for (int i = 0; i < nowPiece.movableTIleList.Count; i++)
        {
            nowPiece.movableTIleList[i].SetAvailableCircle(false);
        }

        // 2. 모든 기물과 타일 정보 재설정
        nowPiece = null;
        SetTilesInfo();
        SetPiecesInfo();

        if (EvaluateIsCheck())
        {
            isCheck = true;
            
            SetKingInfo();
            check?.Invoke();
            return;
        }

        SetKingInfo();

        // 3. 이제 턴을 진행할 색 지정
        nowTurnColor = (nowTurnColor == GameColor.White) ? GameColor.Black : GameColor.White;

        // 4. 현재 턴 증가
        if (nowTurnColor == GameColor.White)
        {
            nowTurn++;
        }

        if(nowTurnColor != playerColor)
            aiTurn?.Invoke();
    }

    void Check()
    {
        Debug.Log("Check");

        // 체크 소리 진행

        // 1. 체크메이트인지 확인
        if (IsCheckmate())
        {
            // 현재 색깔 승리
            NotationManager.instance.AddNotation("#");
            gameEndUI.SetActive(true);
            gameEndText.text = nowTurnColor + " Win!";
            return;
        }
        else
        {
            // 체크 아니므로 상대 턴 시작
            Debug.Log("체크메니트 아님");
            NotationManager.instance.AddNotation("+");
            nowTurnColor = (nowTurnColor == GameColor.White) ? GameColor.Black : GameColor.White;

            if (nowTurnColor != playerColor)
                aiTurn?.Invoke();
        }
    }

    bool IsCheckmate()
    {
        // 1. 킹이 움직일 수 있으면 체크메이트가 불가능하므로 패스
        if (nowTurnColor == GameColor.White)
        {
            if (blackKing.movableTIleList.Count > 0)
                return false;
        }
        else if(nowTurnColor == GameColor.Black)
        {
            if (whiteKing.movableTIleList.Count > 0)
                return false;
        }
        
        // 2. 킹 앞을 가로막을 수 있는 기물이 존재하는가?
        TestManager.Instance.testReady?.Invoke();

        if (isCheckmate)
            return true;

        return false;
    }

    // 턴을 종료하기 전 체크인지 확인하는 함수
    public bool EvaluateIsCheck()
    {
        if (nowTurnColor == GameColor.White)
        {
            // 1. 각 기물의 이동 타일, 공격 기물 설정
            for (int i = 0; i < whitePiecesParent.childCount; i++)
            {
                if (whitePiecesParent.GetChild(i).GetComponent<Piece>().IsAttackKing())
                    return true;
            }
        }
        else if (nowTurnColor == GameColor.Black)
        {
            for (int i = 0; i < blackPiecesParent.childCount; i++)
            {
                if (blackPiecesParent.GetChild(i).GetComponent<Piece>().IsAttackKing())
                    return true;
            }
        }

        return false;
    }

    //게임을 진행하기 전 플레이어의 색을 미리 지정합니다.
    void SetPlayerColor()
    {
        float randomNum = UnityEngine.Random.Range(0, 4);
        if (randomNum < 5)
        {
            playerColor = GameColor.White;
            for (int i = 0; i < whitePiecesParent.childCount; i++)
            {
                whitePiecesParent.GetChild(i).GetComponent<Piece>().isMovable = true;
            }
        }
        else
        {
            playerColor = GameColor.Black;
            //for (int i = 0; i < blackPiecesParent.childCount; i++)
            //{
            //    whitePiecesParent.GetChild(i).GetComponent<Piece>().isMovable = true;
            //    blackPiecesParent.GetChild(i).GetComponent<Piece>().isMovable = true;
            //}
        }

        if (playerColor == GameColor.White)
            return;

        // 카메라 설정
        mainCamera.transform.rotation = Quaternion.Euler(0, 0, 180);
        for (int i = 0; i < whitePiecesParent.childCount; i++)
        {
            whitePiecesParent.GetChild(i).transform.rotation = Quaternion.Euler(180, 0, 0);
        }
        for (int i = 0; i < blackPiecesParent.childCount; i++)
        {
            blackPiecesParent.GetChild(i).transform.rotation = Quaternion.Euler(180, 0, 0);
        }
    }
    #endregion 턴 관련 함수

    #region 타일 및 기물 정보 초기화 함수
    //모든 타일이 공격받지 않는 타일이라고 설정하는 함수
    void SetTilesInfo()
    {
        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                chessTileList[col, row].ClearTileInfo();
            }
    }

   

    // 모든 기물의 이동 타일, 공격 기물 설정하는 함수
    void SetPiecesInfo()
    {
        // 1. 각 기물의 이동 타일, 공격 기물 설정
        for (int i = 0; i < whitePiecesParent.childCount; i++)
        {
            // 킹은 마지막에 다 설정하고 해야하므로 패스
            if (whitePiecesParent.GetChild(i).GetComponent<Piece>().pieceType == PieceType.K)
                continue;

            whitePiecesParent.GetChild(i).GetComponent<Piece>().movableTIleList.Clear();
            whitePiecesParent.GetChild(i).GetComponent<Piece>().attackPieceList.Clear();
            whitePiecesParent.GetChild(i).GetComponent<Piece>().EvaluateMove();        }

        for (int i = 0; i < blackPiecesParent.childCount; i++)
        {
            // 킹은 마지막에 다 설정하고 해야하므로 패스
            if (blackPiecesParent.GetChild(i).GetComponent<Piece>().pieceType == PieceType.K)
                continue;

            blackPiecesParent.GetChild(i).GetComponent<Piece>().movableTIleList.Clear();
            blackPiecesParent.GetChild(i).GetComponent<Piece>().attackPieceList.Clear();
            blackPiecesParent.GetChild(i).GetComponent<Piece>().EvaluateMove();
        }
    }

    void SetKingInfo()
    {
        // 킹은 기물 정리가 다 끝난 뒤 해야함
        whiteKing.movableTIleList.Clear();
        whiteKing.attackPieceList.Clear();
        whiteKing.EvaluateMove();
        blackKing.movableTIleList.Clear();
        blackKing.attackPieceList.Clear();
        blackKing.EvaluateMove();
    }
    #endregion

    #region 프로모션
    public void Promotion(int type)
    {
        GameColor getColor = nowPiece.pieceColor;
        Vector2Int getPos = nowPiece.nowPos;

        switch (type)
        {
            case 2:
                if(getColor == GameColor.White)
                {
                    Piece piece = Instantiate(whiteKnightPrefab, whitePiecesParent).GetComponent<Piece>();
                    piece.pieceType = PieceType.N;
                    SetPiece(piece, getColor, getPos);
                }
                else
                {
                    Piece piece = Instantiate(blackKnightPrefab, blackPiecesParent).GetComponent<Piece>();
                    piece.pieceType = PieceType.N;
                    SetPiece(piece, getColor, getPos);
                }
                break;
            case 3:
                if (getColor == GameColor.White)
                {
                    Piece piece = Instantiate(whiteBishopPrefab, whitePiecesParent).GetComponent<Piece>();
                    piece.pieceType = PieceType.B;
                    SetPiece(piece, getColor, getPos);
                }
                else
                {
                    Piece piece = Instantiate(blackBishopPrefab, blackPiecesParent).GetComponent<Piece>();
                    piece.pieceType = PieceType.B;
                    SetPiece(piece, getColor, getPos);
                }
                break;
            case 4:
                if (getColor == GameColor.White)
                {
                    Piece piece = Instantiate(whiteRookPrefab, whitePiecesParent).GetComponent<Piece>();
                    piece.pieceType = PieceType.R;
                    SetPiece(piece, getColor, getPos);
                }
                else
                {
                    Piece piece = Instantiate(blackRookPrefab, blackPiecesParent).GetComponent<Piece>();
                    piece.pieceType = PieceType.N;
                    SetPiece(piece, getColor, getPos);
                }
                break;
            case 5:
                if (getColor == GameColor.White)
                {
                    Piece piece = Instantiate(whiteQueenPrefab, whitePiecesParent).GetComponent<Piece>();
                    piece.pieceType = PieceType.Q;
                    SetPiece(piece, getColor, getPos);
                }
                else
                {
                    Piece piece = Instantiate(blackQueenPrefab, blackPiecesParent).GetComponent<Piece>();
                    piece.pieceType = PieceType.Q;
                    SetPiece(piece, getColor, getPos);
                }
                break;
        }

        promotionUI.SetActive(false);
        turnEnd?.Invoke();
    }
    #endregion
}
