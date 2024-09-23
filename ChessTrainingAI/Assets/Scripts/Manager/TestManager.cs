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
    }
    #endregion

    [Header("�׽�Ʈ�� ���� ����, �⹰, Ÿ��")]
    [SerializeField] GameObject testTilePrefab;
    [SerializeField] Transform testBoardParent;
    [SerializeField] Transform testPiecesParent;

    [Header("�׽�Ʈ�� ����� ������Ʈ")]
    public TestTile[,] testTileList = new TestTile[8, 8];               // �׽�Ʈ�� ������ ��ü Ÿ��
    public List<TestPiece> testWhitePieces = new List<TestPiece>();
    public List<TestPiece> testBlackPieces = new List<TestPiece>();

    [Header("�׽�Ʈ �̺�Ʈ")]
    public UnityEvent testReady;
    public UnityEvent testStart;

    [Header("üũ����Ʈ Ȯ�� ����")]
    public bool isCheckmate = false;

    private void Start()
    {
        
    }

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
    void SetTestBoardInfo()
    {
        // ���� �⹰�� ���� ��ġ�� �����ͼ� �ű�
    }

    // ���� ��� �̵��� Ȯ���� ŷ�� �����ϴ��� Ȯ���Ѵ�. 
    void Test()
    {
        // ���� ���� �������̸� ��� �⹰�� �̵���Ű�� �������� ��� ŷ�� �����ϴ��� Ȯ���Ѵ�. 
        if(ChessManager.instance.nowTurnColor == GameColor.Black)
            for(int i = 0; i < testWhitePieces.Count; i++)
            {
                for (int moveCount = 0; moveCount < testWhitePieces[i].movableTIleList.Count; moveCount++)
                {
                    // �⹰ �̵�
                    testWhitePieces[i].Move(testWhitePieces[i].movableTIleList[moveCount]);

                    // üũ Ȯ��
                }
            }

        else if (ChessManager.instance.nowTurnColor == GameColor.White)
            for (int i = 0; i < testBlackPieces.Count; i++)
            {
                for (int moveCount = 0; moveCount < testBlackPieces[i].movableTIleList.Count; moveCount++)
                {
                    testBlackPieces[i].Move(testBlackPieces[i].movableTIleList[moveCount]);
                }
            }
    }

    // �׽�Ʈ ���� �ʱ�ȭ
    void ResetBoardInfo()
    {
        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                testTileList[col, row].ClearTileInfo();
            }
    }

    #region üũ Ȯ�ο� �Լ�
    //// ���� �����ϱ� �� üũ���� Ȯ���ϴ� �Լ�
    //public bool EvaluateIsCheck()
    //{
    //    if (nowTurnColor == GameColor.White)
    //    {
    //        // 1. �� �⹰�� �̵� Ÿ��, ���� �⹰ ����
    //        for (int i = 0; i < whitePiecesParent.childCount; i++)
    //        {
    //            if (whitePiecesParent.GetChild(i).GetComponent<Piece>().IsAttackKing())
    //                return true;
    //        }
    //    }
    //    else if (nowTurnColor == GameColor.Black)
    //    {
    //        for (int i = 0; i < blackPiecesParent.childCount; i++)
    //        {
    //            if (blackPiecesParent.GetChild(i).GetComponent<Piece>().IsAttackKing())
    //                return true;
    //        }
    //    }

    //    return false;
    //}
    #endregion
}
