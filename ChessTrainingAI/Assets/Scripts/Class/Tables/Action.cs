using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Action
{
    public float[,] nowActionArr = new float[64, 64];
    public List<Vector2Int> availableActionList = new List<Vector2Int>();

    public float maxActionReward = 0;
    public float minActionReward = 0;

    public Action()
    {
        ClearAction0();
    }

    #region Action 데이터 설정 함수
    // 파라미터로 받은 action을 복사
    public void DeepCopyAction(Action getAction)
    {
        for (int i = 0; i < 64; i++)
            for (int j = 0; j < 64; j++)
                nowActionArr[i, j] = getAction.nowActionArr[i, j];
    }

    // state를 0으로 초기화
    public void ClearAction0()
    {
        availableActionList.Clear();
        for (int i = 0; i < 64; i++)
            for (int j = 0; j < 64; j++)
                nowActionArr[i, j] = 0;
    }

    public void SetNowAction()
    {
        Tile[,] getTiles = ChessManager.instance.chessTileList;

        List<Piece> nowPieces = new List<Piece>();

        ClearAction0();

        // 기물 정리
        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
            {
                if (getTiles[x, y].locatedPiece != null)
                    nowPieces.Add(getTiles[x, y].locatedPiece);
            }

        // 모든 기물 확인
        for (int i = 0; i < nowPieces.Count; i++)
        {
            // 기물의 이동 가능한 타일을 모두 action에 저장
            for (int movableTileCount = 0; movableTileCount < nowPieces[i].movableTIleList.Count; movableTileCount++)
            {
                int startPos = nowPieces[i].nowPos.y * 8 + nowPieces[i].nowPos.x;
                int targetPos = (int)nowPieces[i].movableTIleList[movableTileCount].tileName;

                nowActionArr[startPos, targetPos] = nowPieces[i].piecePoint;
                availableActionList.Add(new Vector2Int(startPos, targetPos));
            }
        }
    }
    public void ConvertTestBoard2NowAction()
    {
        TestTile[,] getTiles = TestManager.Instance.testTileList;

        List<TestPiece> nowPieces = new List<TestPiece>();
        ClearAction0();

        // 기물 정리
        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
            {
                if (getTiles[x, y].locatedPiece != null)
                    nowPieces.Add(getTiles[x, y].locatedPiece);
            }

        // 모든 기물 확인
        for (int i = 0; i < nowPieces.Count; i++)
        {
            // 기물의 이동 가능한 타일을 모두 action에 저장
            for (int movableTileCount = 0; movableTileCount < nowPieces[i].movableTIleList.Count; movableTileCount++)
            {
                int startPos = nowPieces[i].nowPos.y * 8 + nowPieces[i].nowPos.x;
                int targetPos = (int)nowPieces[i].movableTIleList[movableTileCount].tileName;

                nowActionArr[startPos, targetPos] = nowPieces[i].piecePoint;
                availableActionList.Add(new Vector2Int(startPos, targetPos));

            }
        }
    }

    // State를 받아 action table을 업데이트
    public void UpdateAction(State getState)
    {
        // 1. getState의 상태로 TestBoard를 설정
        TestManager.Instance.ConvertState2TestTileBoard(getState);
        
        // 2. TestBoard 상태에서 이동할 수 있는 타일을 탐색한다. 
        TestManager.Instance.EvaluateMovableTiles();

        // 3. 테스트한 각 기물의 movableTile을 nowAction으로 변환한다.
        ConvertTestBoard2NowAction();
    }
    #endregion

    // nowAction에서 가장 높은 reward를 가지는 action의 row와 col을 return함니다
    public List<Vector2Int> GetMaxAction(State getState)
    {
        List<Vector2Int> largestIndexList = new List<Vector2Int>();
        largestIndexList.Add(availableActionList[0]);

        Vector2Int currentIndex;
        for (int i=0; i< availableActionList.Count; i++)
        {
            //Debug.Log("검사 시작");
            currentIndex = availableActionList[i];
            if (getState.nowState[currentIndex.y % 8, currentIndex.y / 8] 
                        > getState.nowState[largestIndexList[0].y % 8, largestIndexList[0].y / 8])
            {
                //Debug.Log(availableActionList[i] + " 초기화 후 추가");
                maxActionReward = getState.nowState[currentIndex.y % 8, currentIndex.y / 8];
                largestIndexList.Clear();
                largestIndexList.Add(availableActionList[i]);
            }
            else if(getState.nowState[currentIndex.y % 8, currentIndex.y / 8] 
                        == getState.nowState[largestIndexList[0].y % 8, largestIndexList[0].y / 8])
            {
                //Debug.Log(availableActionList[i] + " 추가");
                largestIndexList.Add(availableActionList[i]);
            }
        }

        return largestIndexList;
    }

    // nowAction에서 가장 낮은 reward를 가지는 action의 row와 col을 return함니다
    public List<Vector2Int> GetMinAction(State getState)
    {
        List<Vector2Int> smallestIndexList = new List<Vector2Int>();
        smallestIndexList.Add(availableActionList[0]);

        Vector2Int currentIndex;
        for (int i = 0; i < availableActionList.Count; i++)
        {
            currentIndex = availableActionList[i];
            if (getState.nowState[currentIndex.y % 8, currentIndex.y / 8] 
                        < getState.nowState[smallestIndexList[0].y % 8, smallestIndexList[0].y / 8])
            {
                //Debug.Log(currentIndex + "최소값 확인중: " + getState.nowState[currentIndex.y % 8, currentIndex.y / 8]);
                minActionReward = getState.nowState[currentIndex.y % 8, currentIndex.y / 8];
                smallestIndexList.Clear();
                smallestIndexList.Add(availableActionList[i]);
            }
            else if (getState.nowState[currentIndex.y % 8, currentIndex.y / 8] 
                        == getState.nowState[smallestIndexList[0].y % 8, smallestIndexList[0].y / 8])
            {
                smallestIndexList.Add(availableActionList[i]);
            }
        }

        return smallestIndexList;
    }

    public void DebugActionText()
    {
        Debug.Log("액션");
        StringBuilder arr = new StringBuilder();
        for (int y = 0; y < 64; y++)
        {
            for (int z = 0; z < 64; z++)
            {
                if (MathF.Abs(nowActionArr[y, z]) > 0)
                {
                    if (nowActionArr[y, z] > 0)
                        arr.Append("White ");
                    else if (nowActionArr[y, z] < 0)
                        arr.Append("Black ");

                    switch (MathF.Abs(nowActionArr[y, z]))
                    {
                        case 1:
                            arr.Append("Pawn : "); break;
                        case 3:
                            arr.Append("Knight : "); break;
                        case 3.5f:
                            arr.Append("Bishop : "); break;
                        case 5:
                            arr.Append("Rook : "); break;
                        case 8:
                            arr.Append("Queen : "); break;
                        case 10000:
                            arr.Append("King : "); break;
                    }


                    arr.Append((TIleName)Enum.Parse(typeof(TIleName), y.ToString()) + " -> "
                        + (TIleName)Enum.Parse(typeof(TIleName), z.ToString()) + "\n");
                }
            }
        }
        Debug.Log(arr.ToString());
    }
}
