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

    public Tile[,] testTileList = new Tile[8, 8];               // 테스트를 진행할 대체 타일
    public List<Piece> testWhitePieces = new List<Piece>();
    public List<Piece> testBlackPieces = new List<Piece>();

    public UnityEvent readyTest;
    public UnityEvent resetTest;

    private void Start()
    {
        
    }

    // 테스트 보드에 타일 생성
    void SetTestBoard()
    {
        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                Tile tile = Instantiate(tilePrefab, testBoardParent).GetComponent<Tile>();
                tile.transform.localPosition = new Vector2(row, col);
                testTileList[row, col] = tile;

                // 타일 이름 설정
                testTileList[row, col].tileName = (TIleName)(row + col * 8);

                tile.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
            }
    }
    
    // 테스트 보드에 현재 위치한 기물 정보 삽입
    void SetTestBoardInfo()
    {
        Piece tempPiece;
        Piece getLocatedPiece;
        tempPiece = Instantiate(ChessManager.instance.chessTileList[0, 0].locatedPiece, testPiecesParent).GetComponent<Piece>();

        // 타일에 기물 정보 넣기

        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                if (ChessManager.instance.chessTileList[row, col].locatedPiece != null)
                {
                    // 가져올 기물 정보 받고 카피
                    getLocatedPiece = ChessManager.instance.chessTileList[row, col].locatedPiece;
                    tempPiece.PieceInfoCopy(getLocatedPiece);

                    // 타일에 정보 저장
                    testTileList[row,col].locatedPiece = tempPiece;
                    tempPiece.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                }
            }
    }

    // 테스트 보드 초기화
    void ResetTestInfo()
    {
        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                testTileList[col, row].ClearTileInfo();
            }
    }

    #region 테스트 함수
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
        // 원래 기물 돌려놓기
    }
    #endregion
}
