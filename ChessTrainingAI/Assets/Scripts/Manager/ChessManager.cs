using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;

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

    public Camera mainCamera;          //��鿡 ���� ���� ���� �ٲٷ��� ������ ī�޶�

    #region Ÿ��, ������ ����
    [SerializeField]
    GameObject tilePrefab;
    [SerializeField]
    Transform chessBoardParent;
    [SerializeField]
    Transform testBoardParent;
    [SerializeField]
    public Tile[,] chessTileList = new Tile[8, 8];
    #endregion Ÿ��, ������ ����

    #region �⹰ ������
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
    #endregion �⹰ ������

    public GameColor playerColor = GameColor.Null;
    public GameColor nowTurnColor = GameColor.White;
    public Piece nowPiece;
    #region ŷ ������Ʈ
    Piece whiteKing;
    Piece blackKing;
    #endregion

    #region �� �̺�Ʈ
    public UnityEvent gameStart;
    public UnityEvent turnEnd;

    public UnityEvent check;
    public bool isCheck = false;
    public bool isCheckmate = false;
    #endregion �� �̺�Ʈ

    #region ���� ���� �̺�Ʈ
    public bool isClickAvailable = true;
    #endregion ���� ���� �̺�Ʈ

    void Start()
    {
        // ü����, �⹰ ��ġ��Ű��
        SetChessBoard();
        SetChessPiece();

        // �� �Լ� ����
        gameStart.AddListener(GameStart);
        turnEnd.AddListener(EndTurn);
        check.AddListener(Check);

        // ���� ����
        nowTurnColor = GameColor.White;
        gameStart?.Invoke();
    }

    #region ü�� Ÿ��, �⹰ ����
    void SetChessBoard()
    {
        bool colorChange = true;
        GameObject tile;

        for(int col = 0; col < 8; col++)
            for(int row = 0; row < 8; row++)
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
        GameObject piecePrefab = null;

        //������ �⹰ ����
        for (int i = 0; i < 8; i++)
        {
            if (getColor == GameColor.Black)
            {
                piecePrefab = Instantiate(blackPawnPrefab, blackPiecesParent);
                piecePrefab.GetComponent<Transform>().position = new Vector2(i, 6);
                piecePrefab.GetComponent<Piece>().nowPos = new Vector2Int(i, 6);
                piecePrefab.GetComponent<Piece>().pieceType = PieceType.Pawn;
                piecePrefab.GetComponent<Piece>().pieceColor = GameColor.Black;

                chessTileList[i, 6].locatedPiece = piecePrefab.GetComponent<Piece>();
            }
            else if (getColor == GameColor.White)
            {
                piecePrefab = Instantiate(whitePawnPrefab, whitePiecesParent);
                piecePrefab.GetComponent<Transform>().position = new Vector2(i, 1);
                piecePrefab.GetComponent<Piece>().nowPos = new Vector2Int(i, 1);
                piecePrefab.GetComponent<Piece>().pieceType = PieceType.Pawn;
                piecePrefab.GetComponent<Piece>().pieceColor = GameColor.White;

                chessTileList[i, 1].locatedPiece = piecePrefab.GetComponent<Piece>();
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
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.Knight;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.Knight;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.Black;
            piecePrefab2.GetComponent<Piece>().pieceColor = GameColor.Black;


            chessTileList[1, 7].locatedPiece = piecePrefab1.GetComponent<Piece>();
            chessTileList[6, 7].locatedPiece = piecePrefab2.GetComponent<Piece>();
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


            chessTileList[1, 0].locatedPiece = piecePrefab1.GetComponent<Piece>();
            chessTileList[6, 0].locatedPiece = piecePrefab2.GetComponent<Piece>();
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
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.Bishop;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.Bishop;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.Black;
            piecePrefab2.GetComponent<Piece>().pieceColor = GameColor.Black;


            chessTileList[2, 7].locatedPiece = piecePrefab1.GetComponent<Piece>();
            chessTileList[5, 7].locatedPiece = piecePrefab2.GetComponent<Piece>();
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

            chessTileList[2, 0].locatedPiece = piecePrefab1.GetComponent<Piece>();
            chessTileList[5, 0].locatedPiece = piecePrefab2.GetComponent<Piece>();
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
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.Rook;
            piecePrefab2.GetComponent<Piece>().pieceType = PieceType.Rook;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.Black;
            piecePrefab2.GetComponent<Piece>().pieceColor = GameColor.Black;

            King nowKing = GameObject.Find("BlackKing(Clone)").GetComponent<King>();
            nowKing.nowRooks[0] = piecePrefab1.GetComponent<Rook>();
            nowKing.nowRooks[1] = piecePrefab2.GetComponent<Rook>();

            chessTileList[0, 7].locatedPiece = piecePrefab1.GetComponent<Piece>();
            chessTileList[7, 7].locatedPiece = piecePrefab2.GetComponent<Piece>();
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

            chessTileList[0, 0].locatedPiece = piecePrefab1.GetComponent<Piece>();
            chessTileList[7, 0].locatedPiece = piecePrefab2.GetComponent<Piece>();
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
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.Queen;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.Black;

            chessTileList[3, 7].locatedPiece = piecePrefab1.GetComponent<Piece>();
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteQueenPrefab, whitePiecesParent);
            piecePrefab1.GetComponent<Transform>().position = new Vector2(3, 0);
            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(3, 0);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.Queen;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.White;

            chessTileList[3, 0].locatedPiece = piecePrefab1.GetComponent<Piece>();
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
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.King;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.Black;

            chessTileList[4, 7].locatedPiece = piecePrefab1.GetComponent<Piece>();
            blackKing = piecePrefab1.GetComponent<Piece>();
        }
        else if (getColor == GameColor.White)
        {
            piecePrefab1 = Instantiate(whiteKingPrefab, whitePiecesParent);

            piecePrefab1.GetComponent<Transform>().position = new Vector2(4, 0);
            piecePrefab1.GetComponent<Piece>().nowPos = new Vector2Int(4, 0);
            piecePrefab1.GetComponent<Piece>().pieceType = PieceType.King;
            piecePrefab1.GetComponent<Piece>().pieceColor = GameColor.White;

            chessTileList[4, 0].locatedPiece = piecePrefab1.GetComponent<Piece>();
            whiteKing = piecePrefab1.GetComponent<Piece>();
        }
        else
            Debug.Log("King ����� �߿� GetColor �Ű������� Null�� ǥ�õǾ����ϴ�.");

    }
    #endregion

    #region ���� ���� �� �� ���� �Լ�
    void GameStart()
    {
        // 1. �÷��̾� �� ���� �� ī�޶� �̵�
        SetPlayerColor();

        // 2. �⹰�� �̵� Ÿ�ϰ� ���� �⹰ ����
        SetTilesInfo();
        SetPiecesInfo();
    }

    //�� ����
    public void EndTurn()
    {
        Debug.Log("turnENd");
        // 1. �̵� ���� Ÿ�� ǥ�� ��� ����
        for (int i = 0; i < nowPiece.movableTIleList.Count; i++)
        {
            nowPiece.movableTIleList[i].SetAvailableCircle(false);
        }

        // 2. ��� �⹰�� Ÿ�� ���� �缳��
        nowPiece = null;
        SetTilesInfo();
        SetPiecesInfo();

        if (EvaluateIsCheck())
            check?.Invoke();

        SetKingInfo();

        // 3. ���� ���� ������ �� ����
        nowTurnColor = (nowTurnColor == GameColor.White) ? GameColor.Black : GameColor.White;
    }

    void Check()
    {
        Debug.Log("Check");
        isCheck = true;

        // 1. üũ����Ʈ���� Ȯ��
        if (IsCheckmate())
            return;

        // üũ �Ҹ� ����

        // ���� ���� üũ�� Ǯ������ Ȯ���ϵ��� ���� �ϳ��� �߰���
    }

    bool IsCheckmate()
    {
        // 1. ŷ�� ������ �� ������ üũ����Ʈ�� �Ұ����ϹǷ� �н�
        if (nowTurnColor == GameColor.White)
        {
            if (blackKing.movableTIleList.Count > 0)
                return false;
        }
        else
        {
            if (whiteKing.movableTIleList.Count > 0)
                return false;
        }

        // 2. ŷ ���� ���θ��� �� �ִ� �⹰�� �����ϴ°�?
        TestManager.Instance.testReady?.Invoke();

        return false;
    }

    // ���� �����ϱ� �� üũ���� Ȯ���ϴ� �Լ�
    public bool EvaluateIsCheck()
    {
        if (nowTurnColor == GameColor.White)
        {
            // 1. �� �⹰�� �̵� Ÿ��, ���� �⹰ ����
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

    //������ �����ϱ� �� �÷��̾��� ���� �̸� �����մϴ�.
    void SetPlayerColor()
    {
        float randomNum = UnityEngine.Random.Range(0, 10);
        if (randomNum < 5)
            playerColor = GameColor.White;
        else
            playerColor = GameColor.Black;

        if (playerColor == GameColor.White)
            return;

        // ī�޶� ����
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
    #endregion �� ���� �Լ�

    #region Ÿ�� �� �⹰ ���� �ʱ�ȭ �Լ�
    //��� Ÿ���� ���ݹ��� �ʴ� Ÿ���̶�� �����ϴ� �Լ�
    void SetTilesInfo()
    {
        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                chessTileList[col, row].ClearTileInfo();
            }
    }

   

    // ��� �⹰�� �̵� Ÿ��, ���� �⹰ �����ϴ� �Լ�
    void SetPiecesInfo()
    {
        // 1. �� �⹰�� �̵� Ÿ��, ���� �⹰ ����
        for (int i = 0; i < whitePiecesParent.childCount; i++)
        {
            // ŷ�� �������� �� �����ϰ� �ؾ��ϹǷ� �н�
            if (whitePiecesParent.GetChild(i).GetComponent<Piece>().pieceType == PieceType.King)
                continue;

            whitePiecesParent.GetChild(i).GetComponent<Piece>().movableTIleList.Clear();
            whitePiecesParent.GetChild(i).GetComponent<Piece>().attackPieceList.Clear();
            whitePiecesParent.GetChild(i).GetComponent<Piece>().EvaluateMove();        }

        for (int i = 0; i < blackPiecesParent.childCount; i++)
        {
            // ŷ�� �������� �� �����ϰ� �ؾ��ϹǷ� �н�
            if (blackPiecesParent.GetChild(i).GetComponent<Piece>().pieceType == PieceType.King)
                continue;

            blackPiecesParent.GetChild(i).GetComponent<Piece>().movableTIleList.Clear();
            blackPiecesParent.GetChild(i).GetComponent<Piece>().attackPieceList.Clear();
            blackPiecesParent.GetChild(i).GetComponent<Piece>().EvaluateMove();
        }
    }

    void SetKingInfo()
    {
        // ŷ�� �⹰ ������ �� ���� �� �ؾ���
        whiteKing.movableTIleList.Clear();
        whiteKing.attackPieceList.Clear();
        whiteKing.EvaluateMove();
        blackKing.movableTIleList.Clear();
        blackKing.attackPieceList.Clear();
        blackKing.EvaluateMove();
    }
    #endregion

    
}
