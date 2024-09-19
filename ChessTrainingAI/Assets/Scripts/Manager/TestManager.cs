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
        readyTest.AddListener(SetTestBoardInfo);
        resetTest.AddListener(ResetTestInfo);
    }
    #endregion
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Transform testBoardParent;
    [SerializeField] Transform testPiecesParent;

    public Tile[,] testTileList = new Tile[8, 8];               // �׽�Ʈ�� ������ ��ü Ÿ��
    public List<Piece> testWhitePieces = new List<Piece>();
    public List<Piece> testBlackPieces = new List<Piece>();

    public UnityEvent readyTest;
    public UnityEvent resetTest;

    private void Start()
    {
        
    }

    // �׽�Ʈ ���忡 Ÿ�� ����
    void SetTestBoard()
    {
        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                Tile tile = Instantiate(tilePrefab, testBoardParent).GetComponent<Tile>();
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
        Piece tempPiece;
        Piece getLocatedPiece;
        tempPiece = Instantiate(ChessManager.instance.chessTileList[0, 0].locatedPiece, testPiecesParent).GetComponent<Piece>();

        // Ÿ�Ͽ� �⹰ ���� �ֱ�

        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                if (ChessManager.instance.chessTileList[row, col].locatedPiece != null)
                {
                    // ������ �⹰ ���� �ް� ī��
                    getLocatedPiece = ChessManager.instance.chessTileList[row, col].locatedPiece;
                    tempPiece.PieceInfoCopy(getLocatedPiece);

                    // Ÿ�Ͽ� ���� ����
                    testTileList[row,col].locatedPiece = tempPiece;
                    tempPiece.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                }
            }
    }

    // �׽�Ʈ ���� �ʱ�ȭ
    void ResetTestInfo()
    {
        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                testTileList[col, row].ClearTileInfo();
            }
    }

    #region �׽�Ʈ �Լ�
    void TestMovePiece(GameColor getNowTurnColor)
    {
        if(getNowTurnColor == GameColor.White)
        {
            
        }

        else if(getNowTurnColor == GameColor.Black)
        {
            
        }
    }

    void RollBackTestMove()
    {
        // ���� �⹰ ��������
    }
    #endregion
}
