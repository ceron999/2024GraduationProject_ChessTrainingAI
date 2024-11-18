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

    [Header("테스트를 위한 보드, 기물, 타일")]
    [SerializeField] GameObject testTilePrefab;
    [SerializeField] Transform testBoardParent;

    [Header("테스트에 사용할 오브젝트")]
    public TestTile[,] testTileList = new TestTile[8, 8];               // 테스트를 진행할 대체 타일
    public List<TestPiece> testWhitePieces = new List<TestPiece>();
    public List<TestPiece> testBlackPieces = new List<TestPiece>();

    [Header("테스트 이벤트")]
    public UnityEvent testReady;
    public UnityEvent testStart;

    #region 테스트 보드 생성 및 초기화
    // 테스트 보드에 타일 생성
    void SetTestBoard()
    {
        bool colorChange = true;
        for (int col = 0; col < 8; col++)
            for (int row = 0; row < 8; row++)
            {
                TestTile tile = Instantiate(testTilePrefab, testBoardParent).GetComponent<TestTile>();
                tile.transform.localPosition = new Vector2(row, col);
                testTileList[row, col] = tile;

                // 타일 이름 설정
                testTileList[row, col].tileName = (TIleName)(row + col * 8);

                // 타일 색을 변경
                if (colorChange)
                    testTileList[row, col].GetComponent<SpriteRenderer>().color = new Color(115 / 255f, 144 / 255f, 83 / 255f);
                else
                    testTileList[row, col].GetComponent<SpriteRenderer>().color = new Color(235 / 255f, 236 / 255f, 208 / 255f);

                // 다음 Eow의 0번 Col의 색깔이 이전 Row의 7번 Col과 동일하므로 변화하지 않도록 고정하는 부분
                if (row != 7)
                    colorChange = !colorChange;
            }

        
    }
    
    // 테스트 보드에 현재 위치한 기물 정보 삽입
    public void SetTestBoardInfo()
    {
        // 현재 기물의 현재 위치를 가져와서 옮김
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

    // 이제 모든 이동을 확인해 킹을 공격하는지 확인한다. 
    void TestCheckMate()
    {
        // 0. 기물 저장
        TestPiece nowPiece = new TestPiece();       // 이동시킬 기물
        TestPiece savePiece = new TestPiece();      // 이동할 타일에 있던 기물

        Vector2Int savePosition = Vector2Int.zero;

        // 1-1. 현재 턴이 흰색일 경우
        if (ChessManager.instance.nowTurnColor == GameColor.White)
        {
            // 2. 흰색 기물을 이동시켜 여전히 체크인지 확인할 것
            for (int i = 0; i < testBlackPieces.Count; i++)
            {
                // 3. 현재 이동 기물 저장
                nowPiece = testBlackPieces[i];
                Debug.Log("현재 기물 : " + nowPiece.pieceColor + " " + nowPiece.pieceType + " (" + nowPiece.nowPos + ")");

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

                    Debug.Log("공격 위치 : " + nowPiece.movableTIleList[count].tileName);

                    // 3.기물을 이동했을 때 체크가 풀린다면 턴 종료
                    if (!EvaluateIsCheck(testWhitePieces))
                    {
                        ChessManager.instance.nowTurnColor = (ChessManager.instance.nowTurnColor == GameColor.White) ? GameColor.Black : GameColor.White;
                        return;
                    }

                    // 체크 아니니까 undo
                    savePosition = new Vector2Int((int)nowPiece.movableTIleList[count].transform.localPosition.x, (int)nowPiece.movableTIleList[count].transform.localPosition.y);
                    UndoPieceMove(nowPiece, nowPiece.nowPos, savePiece, savePosition); 
                }
            }
            ChessManager.instance.isCheckmate = true;
        }

        // 1-2. 현재 턴이 검은색일 경우
        else if (ChessManager.instance.nowTurnColor == GameColor.Black)
        {
            // 2. 검은색 기물을 이동시켜 여전히 체크인지 확인할 것
            for (int i = 0; i < testWhitePieces.Count; i++)
            {
                // 3. 현재 이동 기물 저장
                nowPiece = testWhitePieces[i];
                Debug.Log("현재 기물 : " + nowPiece.pieceColor + " " + nowPiece.pieceType + " (" + nowPiece.nowPos + ")");

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

                    Debug.Log("공격 위치 : " + nowPiece.movableTIleList[count].tileName);

                    // 3.기물을 이동했을 때 체크가 풀린다면 턴 종료
                    if (!EvaluateIsCheck(testBlackPieces))
                    {
                        ChessManager.instance.nowTurnColor = (ChessManager.instance.nowTurnColor == GameColor.White) ? GameColor.Black : GameColor.White;
                        return;
                    }

                    // 체크 아니니까 undo
                    savePosition = new Vector2Int((int)nowPiece.movableTIleList[count].transform.localPosition.x, (int)nowPiece.movableTIleList[count].transform.localPosition.y);
                    UndoPieceMove(nowPiece, nowPiece.nowPos, savePiece, savePosition);
                }
            }
            ChessManager.instance.isCheckmate = true;
        }
    }

    // 테스트 보드 초기화
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

    #region 체크 및 이동 확인용 함수
    // 턴을 종료하기 전 체크인지 확인하는 함수
    public bool EvaluateIsCheck(List<TestPiece> getList)
    {
        Vector2Int piecePos = new Vector2Int();
        for (int i = 0; i < getList.Count; i++)
            getList[i].SetAttackPieceList();

        for (int i=0; i<getList.Count; i++)
        {
            // 1. 이동에 의해 현재 기물이 죽었다면 넘어가기
            piecePos = getList[i].nowPos;
            if (testTileList[piecePos.x, piecePos.y].locatedPiece.pieceColor != getList[i].pieceColor)
                continue;

            // 2. 본인이 공격할 수 있는 기물에 킹이 있다면 참
            if (getList[i].IsAttackKing())
                return true;
        }
        return false;
    }
    
    // 현재 Board에서 이동 가능한 행동 계산
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

    #region 변환 함수
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
    /// State.nowState[x,y]의 값을 확인하고 해당 값에 맞는 기물로 설정
    /// </summary>
    /// <param name="nowStateNum"> State.nowState[x,y]의 값</param>
    TestPiece ConvertStateNum2Piece(float nowStateNum, Vector2Int getNowPos)
    {
        TestPiece piece = new TestPiece();
        switch (Mathf.Abs(nowStateNum))
        {
            case 1:
                // 백
                piece = new TestPawn();
                piece.pieceType = PieceType.P;
                piece.nowPos = getNowPos;
                if (nowStateNum > 0)
                {
                    piece.pieceColor = GameColor.White;
                    piece.piecePoint = 1;
                }

                // 흑
                else
                {
                    piece.pieceColor = GameColor.Black;
                    piece.piecePoint = -1;
                }

        return piece;
            case 3:
                // 백
                piece = new TestKnight();
                piece.pieceType = PieceType.N;
                piece.nowPos = getNowPos;
                if (nowStateNum > 0)
                { 
                    piece.pieceColor = GameColor.White; 
                    piece.piecePoint = 3;
                }

                // 흑
                else
                { 
                    piece.pieceColor = GameColor.Black;
                    piece.piecePoint = -3;
                }

                return piece;
            case 3.5f:
                // 백
                piece = new TestBishop();
                piece.pieceType = PieceType.B;
                piece.nowPos = getNowPos;
                if (nowStateNum > 0)
                {
                    piece.pieceColor = GameColor.White;
                    piece.piecePoint = 3.5f;
                }

                // 흑
                else
                {
                    piece.pieceColor = GameColor.Black; 
                    piece.piecePoint = -3.5f;
                }

                return piece;
            case 5:
                // 백
                piece = new TestRook();
                piece.pieceType = PieceType.R;
                piece.nowPos = getNowPos;
                if (nowStateNum > 0)
                { 
                    piece.pieceColor = GameColor.White; 
                    piece.piecePoint = 5;
                }

                // 흑
                else
                { 
                    piece.pieceColor = GameColor.Black;
                    piece.piecePoint = -5;
                }

                return piece;
            case 8:
                // 백
                piece = new TestQueen();
                piece.pieceType = PieceType.Q;
                piece.nowPos = getNowPos;
                if (nowStateNum > 0)
                {   
                    piece.pieceColor = GameColor.White;
                    piece.piecePoint = 8;
                }

                // 흑
                else
                { 
                    piece.pieceColor = GameColor.Black;
                    piece.piecePoint = -8;
                }
                return piece;
            case 10000:
                // 백
                piece = new TestKing();
                piece.pieceType = PieceType.K;
                piece.nowPos = getNowPos;
                if (nowStateNum > 0)
                {
                    piece.pieceColor = GameColor.White;
                    piece.piecePoint = 10000;
                }
                // 흑
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
        // 0. 확인 전 보드 초기화
        testBlackPieces.Clear();
        testWhitePieces.Clear();
        ClearBoardInfo();

        // 1. 보드판을 State에 맞게 재설정
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                if (getState.nowState[i, j] == 0)
                {
                    //여기엔 기물이 없으므로 넘어간다.
                    continue;
                }

                // tile,testPiece에 기물 데이터를 삽입한다.
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
