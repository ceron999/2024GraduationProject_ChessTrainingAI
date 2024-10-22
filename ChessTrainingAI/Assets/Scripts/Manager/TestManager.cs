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
        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                TestTile tile = Instantiate(testTilePrefab, testBoardParent).GetComponent<TestTile>();
                tile.transform.localPosition = new Vector2(row, col);
                testTileList[row, col] = tile;

                // Ÿ�� �̸� ����
                testTileList[row, col].tileName = (TIleName)(row + col * 8);

                tile.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
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

    #region üũ Ȯ�ο� �Լ�
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
    #endregion
}
