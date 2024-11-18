using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestManager : MonoBehaviour
{
    #region Singleton
    public static TestManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        SetTestBoard();
        testReady.AddListener(SetTestBoardInfo);
        testStart.AddListener(TestCheckMate);
    }
    #endregion

    [Header("�׽�Ʈ�� ���� ����, �⹰, Ÿ��")]
    [SerializeField] GameObject testTilePrefab;
    [SerializeField] Transform testBoardParent;

    [Header("�׽�Ʈ�� ����� ������Ʈ")]
    public TestTile[,] testTileList = new TestTile[8, 8];               // �׽�Ʈ�� ������ ��ü Ÿ��
    public List<TestPiece> testWhitePieces = new List<TestPiece>();
    public List<TestPiece> testBlackPieces = new List<TestPiece>();

    [Header("�׽�Ʈ �̺�Ʈ")]
    public UnityEvent testReady;
    public UnityEvent testStart;

    #region �׽�Ʈ ���� ���� �� �ʱ�ȭ
    // �׽�Ʈ ���忡 Ÿ�� ����
    void SetTestBoard()
    {
        bool colorChange = true;
        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                TestTile tile = Instantiate(testTilePrefab, testBoardParent).GetComponent<TestTile>();
                tile.transform.localPosition = new Vector2(row, col);
                testTileList[row, col] = tile;

                // Ÿ�� �̸� ����
                testTileList[row, col].tileName = (TIleName)(row + col * 8);

                // Ÿ�� ���� ����
                if (colorChange)
                    testTileList[row, col].GetComponent<SpriteRenderer>().color = new Color(115 / 255f, 144 / 255f, 83 / 255f);
                else
                    testTileList[row, col].GetComponent<SpriteRenderer>().color = new Color(235 / 255f, 236 / 255f, 208 / 255f);

                // ���� Eow�� 0�� Col�� ������ ���� Row�� 7�� Col�� �����ϹǷ� ��ȭ���� �ʵ��� �����ϴ� �κ�
                if (row != 7)
                    colorChange = !colorChange;
            }

        
    }
    
    // �׽�Ʈ ���忡 ���� ��ġ�� �⹰ ���� ����
    public void SetTestBoardInfo()
    {
        // ���� �⹰�� ���� ��ġ�� �����ͼ� �ű�
        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                testTileList[row, col].SetTileInfo(ChessManager.instance.chessTileList[row, col]);
                
                if(testTileList[row, col].locatedPiece != null)
                {
                    if(testTileList[row, col].locatedPiece.pieceColor == GameColor.White)
                        testWhitePieces.Add(testTileList[row, col].locatedPiece);
                    else if(testTileList[row, col].locatedPiece.pieceColor == GameColor.Black)
                        testBlackPieces.Add(testTileList[row, col].locatedPiece);
                }
            }

        testStart?.Invoke();
    }
    #endregion

    // ���� ��� �̵��� Ȯ���� ŷ�� �����ϴ��� Ȯ���Ѵ�. 
    void TestCheckMate()
    {
        // 0. �⹰ ����
        TestPiece nowPiece = new TestPiece();       // �̵���ų �⹰
        TestPiece savePiece = new TestPiece();      // �̵��� Ÿ�Ͽ� �ִ� �⹰

        Vector2Int savePosition = Vector2Int.zero;

        // 1-1. ���� ���� ����� ���
        if (ChessManager.instance.nowTurnColor == GameColor.White)
        {
            // 2. ��� �⹰�� �̵����� ������ üũ���� Ȯ���� ��
            for (int i = 0; i < testBlackPieces.Count; i++)
            {
                // 3. ���� �̵� �⹰ ����
                nowPiece = testBlackPieces[i];
                Debug.Log("���� �⹰ : " + nowPiece.pieceColor + " " + nowPiece.pieceType + " (" + nowPiece.nowPos + ")");

                for (int count = 0; count < nowPiece.movableTIleList.Count; count++)
                {
                    if (nowPiece.movableTIleList[count].locatedPiece != null)
                    {
                        savePiece = nowPiece.movableTIleList[count].locatedPiece;
                        savePosition = savePiece.nowPos;
                    }
                    else
                        savePiece = null;
                    nowPiece.Move(nowPiece.movableTIleList[count]);

                    Debug.Log("���� ��ġ : " + nowPiece.movableTIleList[count].tileName);

                    // 3.�⹰�� �̵����� �� üũ�� Ǯ���ٸ� �� ����
                    if (!EvaluateIsCheck(testWhitePieces))
                    {
                        ChessManager.instance.nowTurnColor = (ChessManager.instance.nowTurnColor == GameColor.White) ? GameColor.Black : GameColor.White;
                        return;
                    }

                    // üũ �ƴϴϱ� undo
                    savePosition = new Vector2Int((int)nowPiece.movableTIleList[count].transform.localPosition.x, (int)nowPiece.movableTIleList[count].transform.localPosition.y);
                    UndoPieceMove(nowPiece, nowPiece.nowPos, savePiece, savePosition); 
                }
            }
            ChessManager.instance.isCheckmate = true;
        }

        // 1-2. ���� ���� �������� ���
        else if (ChessManager.instance.nowTurnColor == GameColor.Black)
        {
            // 2. ������ �⹰�� �̵����� ������ üũ���� Ȯ���� ��
            for (int i = 0; i < testWhitePieces.Count; i++)
            {
                // 3. ���� �̵� �⹰ ����
                nowPiece = testWhitePieces[i];
                Debug.Log("���� �⹰ : " + nowPiece.pieceColor + " " + nowPiece.pieceType + " (" + nowPiece.nowPos + ")");

                for (int count = 0; count < nowPiece.movableTIleList.Count; count++)
                {
                    if (nowPiece.movableTIleList[count].locatedPiece != null)
                    {
                        savePiece = nowPiece.movableTIleList[count].locatedPiece;
                        savePosition = savePiece.nowPos;
                    }
                    else
                        savePiece = null;
                    nowPiece.Move(nowPiece.movableTIleList[count]);

                    Debug.Log("���� ��ġ : " + nowPiece.movableTIleList[count].tileName);

                    // 3.�⹰�� �̵����� �� üũ�� Ǯ���ٸ� �� ����
                    if (!EvaluateIsCheck(testBlackPieces))
                    {
                        ChessManager.instance.nowTurnColor = (ChessManager.instance.nowTurnColor == GameColor.White) ? GameColor.Black : GameColor.White;
                        return;
                    }

                    // üũ �ƴϴϱ� undo
                    savePosition = new Vector2Int((int)nowPiece.movableTIleList[count].transform.localPosition.x, (int)nowPiece.movableTIleList[count].transform.localPosition.y);
                    UndoPieceMove(nowPiece, nowPiece.nowPos, savePiece, savePosition);
                }
            }
            ChessManager.instance.isCheckmate = true;
        }
    }

    // �׽�Ʈ ���� �ʱ�ȭ
    void ClearBoardInfo()
    {
        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                testTileList[col, row].ClearTileInfo();
            }
    }

    void UndoPieceMove(TestPiece nowPiece, Vector2Int basePosition, TestPiece savePiece, Vector2Int savePostion)
    {
        testTileList[basePosition.x, basePosition.y].locatedPiece = nowPiece;

        if(savePiece != null)
            testTileList[savePostion.x, savePostion.y].locatedPiece = savePiece;
        else 
            testTileList[savePostion.x, savePostion.y].locatedPiece = null;
    }

    #region üũ �� �̵� Ȯ�ο� �Լ�
    // ���� �����ϱ� �� üũ���� Ȯ���ϴ� �Լ�
    public bool EvaluateIsCheck(List<TestPiece> getList)
    {
        Vector2Int piecePos = new Vector2Int();
        for (int i = 0; i < getList.Count; i++)
            getList[i].SetAttackPieceList();

        for (int i=0; i<getList.Count; i++)
        {
            // 1. �̵��� ���� ���� �⹰�� �׾��ٸ� �Ѿ��
            piecePos = getList[i].nowPos;
            if (testTileList[piecePos.x, piecePos.y].locatedPiece.pieceColor != getList[i].pieceColor)
                continue;

            // 2. ������ ������ �� �ִ� �⹰�� ŷ�� �ִٸ� ��
            if (getList[i].IsAttackKing())
                return true;
        }
        return false;
    }
    
    // ���� Board���� �̵� ������ �ൿ ���
    public void EvaluateMovableTiles()
    {
        TestPiece whiteKing = new TestKing();
        TestPiece blackKing = new TestKing();
        for (int i = 0; i < testBlackPieces.Count; i++)
        {
            if (testBlackPieces[i].pieceType == PieceType.K)
            {
                blackKing = testBlackPieces[i];
                continue;
            }

            testBlackPieces[i].SetMovableTileList();
        }

        for (int i = 0;i < testWhitePieces.Count; i++)
        {
            if (testWhitePieces[i].pieceType == PieceType.K)
            {
                whiteKing = testWhitePieces[i];
                continue;
            }

            testWhitePieces[i].SetMovableTileList();
        }

        whiteKing.SetMovableTileList();
        blackKing.SetMovableTileList();
    }

    #endregion

    #region ��ȯ �Լ�
    public TestTile ConvertTile2TestTile(Tile getTile)
    {
        int tileName = (int)getTile.tileName;
        int row = tileName % 8;
        int col = tileName / 8;

        TestTile temp = testTileList[row, col];

        return temp;
    }

    public TestPiece ConvertPiece2TestPiece(Piece getPiece)
    {

        TestPiece temp;
        switch (getPiece.pieceType)
        {
            case PieceType.P:
                temp = new TestPawn();
                break;
            case PieceType.N:
                temp = new TestKnight();
                break;
            case PieceType.B:
                temp = new TestBishop();
                break;
            case PieceType.R:
                temp = new TestRook();
                break;
            case PieceType.Q:
                temp = new TestQueen();
                break;
            case PieceType.K:
                temp = new TestKing();
                break;
            default:
                temp = new TestPiece();
                break;
        }

        temp.pieceColor = getPiece.pieceColor;
        temp.pieceType = getPiece.pieceType;
        temp.nowPos = getPiece.nowPos;

        return temp;
    }

    /// <summary>
    /// State.nowState[x,y]�� ���� Ȯ���ϰ� �ش� ���� �´� �⹰�� ����
    /// </summary>
    /// <param name="nowStateNum"> State.nowState[x,y]�� ��</param>
    TestPiece ConvertStateNum2Piece(float nowStateNum, Vector2Int getNowPos)
    {
        TestPiece piece = new TestPiece();
        switch (Mathf.Abs(nowStateNum))
        {
            case 1:
                // ��
                piece = new TestPawn();
                piece.pieceType = PieceType.P;
                piece.nowPos = getNowPos;
                if (nowStateNum > 0)
                {
                    piece.pieceColor = GameColor.White;
                    piece.piecePoint = 1;
                }

                // ��
                else
                {
                    piece.pieceColor = GameColor.Black;
                    piece.piecePoint = -1;
                }

        return piece;
            case 3:
                // ��
                piece = new TestKnight();
                piece.pieceType = PieceType.N;
                piece.nowPos = getNowPos;
                if (nowStateNum > 0)
                { 
                    piece.pieceColor = GameColor.White; 
                    piece.piecePoint = 3;
                }

                // ��
                else
                { 
                    piece.pieceColor = GameColor.Black;
                    piece.piecePoint = -3;
                }

                return piece;
            case 3.5f:
                // ��
                piece = new TestBishop();
                piece.pieceType = PieceType.B;
                piece.nowPos = getNowPos;
                if (nowStateNum > 0)
                {
                    piece.pieceColor = GameColor.White;
                    piece.piecePoint = 3.5f;
                }

                // ��
                else
                {
                    piece.pieceColor = GameColor.Black; 
                    piece.piecePoint = -3.5f;
                }

                return piece;
            case 5:
                // ��
                piece = new TestRook();
                piece.pieceType = PieceType.R;
                piece.nowPos = getNowPos;
                if (nowStateNum > 0)
                { 
                    piece.pieceColor = GameColor.White; 
                    piece.piecePoint = 5;
                }

                // ��
                else
                { 
                    piece.pieceColor = GameColor.Black;
                    piece.piecePoint = -5;
                }

                return piece;
            case 8:
                // ��
                piece = new TestQueen();
                piece.pieceType = PieceType.Q;
                piece.nowPos = getNowPos;
                if (nowStateNum > 0)
                {   
                    piece.pieceColor = GameColor.White;
                    piece.piecePoint = 8;
                }

                // ��
                else
                { 
                    piece.pieceColor = GameColor.Black;
                    piece.piecePoint = -8;
                }
                return piece;
            case 10000:
                // ��
                piece = new TestKing();
                piece.pieceType = PieceType.K;
                piece.nowPos = getNowPos;
                if (nowStateNum > 0)
                {
                    piece.pieceColor = GameColor.White;
                    piece.piecePoint = 10000;
                }
                // ��
                else
                {
                    piece.pieceColor = GameColor.Black;
                    piece.piecePoint = -10000;
                }
                return piece;
            default:
                return null;
        }
    }

    public void ConvertState2TestTileBoard(State getState)
    {
        // 0. Ȯ�� �� ���� �ʱ�ȭ
        testBlackPieces.Clear();
        testWhitePieces.Clear();
        ClearBoardInfo();

        // 1. �������� State�� �°� �缳��
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                if (getState.nowState[i, j] == 0)
                {
                    //���⿣ �⹰�� �����Ƿ� �Ѿ��.
                    continue;
                }

                // tile,testPiece�� �⹰ �����͸� �����Ѵ�.
                Vector2Int nowPos = new Vector2Int(i, j);
                testTileList[i, j].locatedPiece = ConvertStateNum2Piece(getState.nowState[i, j], nowPos);

                if (testTileList[i, j].locatedPiece.pieceColor == GameColor.White)
                    testWhitePieces.Add(testTileList[i, j].locatedPiece);
                else if (testTileList[i, j].locatedPiece.pieceColor == GameColor.Black)
                    testBlackPieces.Add(testTileList[i, j].locatedPiece);
            }

    }
    #endregion
}
