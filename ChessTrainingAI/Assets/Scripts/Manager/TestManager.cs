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

    [Header("테스트를 위한 보드, 기물, 타일")]
    [SerializeField] GameObject testTilePrefab;
    [SerializeField] Transform testBoardParent;
    [SerializeField] Transform testPiecesParent;

    [Header("테스트에 사용할 오브젝트")]
    public TestTile[,] testTileList = new TestTile[8, 8];               // 테스트를 진행할 대체 타일
    public List<TestPiece> testWhitePieces = new List<TestPiece>();
    public List<TestPiece> testBlackPieces = new List<TestPiece>();

    [Header("테스트 이벤트")]
    public UnityEvent testReady;
    public UnityEvent testStart;

    [Header("체크메이트 확인 변수")]
    public bool isCheckmate = false;

    private void Start()
    {
        
    }

    // 테스트 보드에 타일 생성
    void SetTestBoard()
    {
        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                TestTile tile = Instantiate(testTilePrefab, testBoardParent).GetComponent<TestTile>();
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
        // 현재 기물의 현재 위치를 가져와서 옮김
    }

    // 이제 모든 이동을 확인해 킹을 공격하는지 확인한다. 
    void Test()
    {
        // 현재 턴이 검은색이면 흰색 기물을 이동시키고 검은색이 흰색 킹을 공격하는지 확인한다. 
        if(ChessManager.instance.nowTurnColor == GameColor.Black)
            for(int i = 0; i < testWhitePieces.Count; i++)
            {
                for (int moveCount = 0; moveCount < testWhitePieces[i].movableTIleList.Count; moveCount++)
                {
                    // 기물 이동
                    testWhitePieces[i].Move(testWhitePieces[i].movableTIleList[moveCount]);

                    // 체크 확인
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

    // 테스트 보드 초기화
    void ResetBoardInfo()
    {
        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                testTileList[col, row].ClearTileInfo();
            }
    }

    #region 체크 확인용 함수
    //// 턴을 종료하기 전 체크인지 확인하는 함수
    //public bool EvaluateIsCheck()
    //{
    //    if (nowTurnColor == GameColor.White)
    //    {
    //        // 1. 각 기물의 이동 타일, 공격 기물 설정
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
