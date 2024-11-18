using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class TestQueen : TestPiece
{
    bool isSearch = true;
    int nowDir = 0;
    int count = 0;

    bool isEvaluateSkip = false;
    bool isBlock = false;

    public override void SetAttackPieceList()
    {
        base.SetAttackPieceList();
        Vector2Int targetVector = nowPos;
        List<Vector2Int> direction = new List<Vector2Int>();
        direction.Add(new Vector2Int(-1, +1));
        direction.Add(new Vector2Int(+1, +1));
        direction.Add(new Vector2Int(-1, -1));
        direction.Add(new Vector2Int(+1, -1)); 
        direction.Add(new Vector2Int(-1, 0));
        direction.Add(new Vector2Int(+1, 0));
        direction.Add(new Vector2Int(0, -1));
        direction.Add(new Vector2Int(0, +1));


        while (isSearch)
        {
            targetVector = nowPos + direction[nowDir] * count;

            // 1. 해당하는 타일이 존재하지 않으면 넘어감
            if (!IsAvailableTIle(targetVector))
            {
                if (nowDir < 8)
                {
                    count = 0;
                    nowDir++;

                    if (nowDir > 1000)
                    {
                        Debug.Log("오류!");
                        break;
                    }
                }
                else
                    break;
            }

            isSearch = true;
            InsertAttackPieces(targetVector);
        }
    }

    public override void SetMovableTileList()
    {
        base.SetMovableTileList();

        EvaluateUpMoveTiles();
        EvaluateRightMoveTiles();
        EvaluateLeftMoveTiles();
        EvaluateDownMoveTiles();
        EvaluateLeftDownMoveTiles();
        EvaluateLeftUpMoveTiles();
        EvaluateRightDownMoveTiles();
        EvaluateRightUpMoveTiles();
    }

    void InsertAttackPieces(Vector2Int getVector)
    {
        TestTile nowTIle = TestManager.Instance.testTileList[getVector.x, getVector.y];

        // 2. 타일의 기물이 없으면 이동 타일 추가
        if (nowTIle.locatedPiece != null)
        {
            if (nowTIle.locatedPiece.pieceColor != pieceColor)
                attackPieceList.Add(nowTIle.locatedPiece);
            else if (nowTIle.locatedPiece.pieceColor == pieceColor)
            {
                if (nowDir < 4)
                {
                    count = 0;
                    nowDir++;
                }
                else
                    isSearch = false;
            }
        }

    }


    #region 수직 이동
    void EvaluateLeftMoveTiles()
    {
        for (int i = 1; nowPos.x - i >= 0; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x - i, nowPos.y);

            // 1. 해당하는 타일이 존재하지 않으면 넘어감
            if (!IsAvailableTIle(targetVector))
                break;

            // 2. 평가를 중단하고 싶으므로 중단
            if (isEvaluateSkip)
                break;

            if (isBlock)
            {
                SetIsColorBlockAttack(TestManager.Instance.testTileList[targetVector.x, targetVector.y]);
                continue;
            }

            EvaluateCrossTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
    }

    void EvaluateRightMoveTiles()
    {
        for (int i = 1; nowPos.x + i <= 7; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x + i, nowPos.y);

            // 1. 해당하는 타일이 존재하지 않으면 넘어감
            if (!IsAvailableTIle(targetVector))
                break;

            // 2. 평가를 중단하고 싶으므로 중단
            if (isEvaluateSkip)
                break;

            if (isBlock)
            {
                SetIsColorBlockAttack(TestManager.Instance.testTileList[targetVector.x, targetVector.y]);
                continue;
            }

            EvaluateCrossTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
    }

    void EvaluateUpMoveTiles()
    {
        for (int i = 1; nowPos.y + i <= 7; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x, nowPos.y + i);

            // 1. 해당하는 타일이 존재하지 않으면 넘어감
            if (!IsAvailableTIle(targetVector))
                break;

            // 2. 평가를 중단하고 싶으므로 중단
            if (isEvaluateSkip)
                break;

            if (isBlock)
            {
                SetIsColorBlockAttack(TestManager.Instance.testTileList[targetVector.x, targetVector.y]);
                continue;
            }

            EvaluateCrossTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
    }

    void EvaluateDownMoveTiles()
    {
        for (int i = 1; nowPos.y - i >= 0; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x, nowPos.y - i);

            // 1. 해당하는 타일이 존재하지 않으면 넘어감
            if (!IsAvailableTIle(targetVector))
                break;

            // 2. 평가를 중단하고 싶으므로 중단
            if (isEvaluateSkip)
                break;

            if (isBlock)
            {
                SetIsColorBlockAttack(TestManager.Instance.testTileList[targetVector.x, targetVector.y]);
                continue;
            }

            EvaluateCrossTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
    }
    #endregion

    #region 대각선 이동
    //(-1.+1)
    void EvaluateLeftUpMoveTiles()
    {
        Vector2Int targetVector = nowPos;

        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x - i, nowPos.y + i);

            // 1. 해당하는 타일이 존재하지 않으면 넘어감
            if (!IsAvailableTIle(targetVector))
                break;

            // 2. 평가를 중단하고 싶으므로 중단
            if (isEvaluateSkip)
                break;

            if (isBlock)
            {
                SetIsColorBlockAttack(TestManager.Instance.testTileList[targetVector.x, targetVector.y]);
                continue;
            }

            EvaluateDiagonalTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
    }

    //(+1.+1)
    void EvaluateRightUpMoveTiles()
    {
        Vector2Int targetVector = nowPos;

        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x + i, nowPos.y + i);

            // 1. 해당하는 타일이 존재하지 않으면 넘어감
            if (!IsAvailableTIle(targetVector))
                break;

            // 2. 평가를 중단하고 싶으므로 중단
            if (isEvaluateSkip)
                break;

            if (isBlock)
            {
                SetIsColorBlockAttack(TestManager.Instance.testTileList[targetVector.x, targetVector.y]);
                continue;
            }

            EvaluateDiagonalTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
    }

    //(-1.-1)
    void EvaluateLeftDownMoveTiles()
    {
        Vector2Int targetVector = nowPos;

        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x - i, nowPos.y - i);

            // 1. 해당하는 타일이 존재하지 않으면 넘어감
            if (!IsAvailableTIle(targetVector))
                break;

            // 2. 평가를 중단하고 싶으므로 중단
            if (isEvaluateSkip)
                break;

            if (isBlock)
            {
                SetIsColorBlockAttack(TestManager.Instance.testTileList[targetVector.x, targetVector.y]);
                continue;
            }

            EvaluateDiagonalTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
    }

    //(+1.-1)
    void EvaluateRightDownMoveTiles()
    {
        Vector2Int targetVector = nowPos;

        for (int i = 1; ; i++)
        {
            targetVector = new Vector2Int(nowPos.x + i, nowPos.y - i);

            // 1. 해당하는 타일이 존재하지 않으면 넘어감
            if (!IsAvailableTIle(targetVector))
                break;

            // 2. 평가를 중단하고 싶으므로 중단
            if (isEvaluateSkip)
                break;

            if (isBlock)
            {
                SetIsColorBlockAttack(TestManager.Instance.testTileList[targetVector.x, targetVector.y]);
                continue;
            }

            EvaluateDiagonalTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
    }
    #endregion

    void EvaluateCrossTiles(Vector2Int getVector)
    {
        TestTile nowTIle = TestManager.Instance.testTileList[getVector.x, getVector.y];

        // 1. 해당 타일이 비어있으면 이동 타일로 추가
        if (nowTIle.locatedPiece == null)
        {
            movableTIleList.Add(nowTIle);
            SetIsColorAttack(nowTIle);
        }
        else
        {
            // 2. 해당 타일의 기물의 색 == 선택한 기물의 색이면 넘어감
            if (nowTIle.locatedPiece.pieceColor == pieceColor)
            {
                isEvaluateSkip = true;
                return;
            }

            // 3. 해당 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
            else
            {
                attackPieceList.Add(nowTIle.locatedPiece);
                movableTIleList.Add(nowTIle);
                SetIsColorAttack(nowTIle);

                if (nowTIle.locatedPiece.pieceType == PieceType.K)
                {
                    isBlock = true;
                    SetIsColorBlockAttack(nowTIle);
                }
                else
                    isEvaluateSkip = true;

                return;
            }
        }
    }

    void EvaluateDiagonalTiles(Vector2Int getVector)
    {
        TestTile nowTIle = TestManager.Instance.testTileList[getVector.x, getVector.y];

        // 2. 타일의 기물이 없으면 이동 타일 추가
        if (nowTIle.locatedPiece == null)
        {
            movableTIleList.Add(nowTIle);
            SetIsColorAttack(nowTIle);
        }
        else
        {
            // 3. 타일의 기물 색 == 선택한 기물의 색이면 넘어감
            if (nowTIle.locatedPiece.pieceColor == pieceColor)
            {
                isEvaluateSkip = true;
                SetIsColorAttack(nowTIle);
                return;
            }

            // 4. 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
            else
            {
                attackPieceList.Add(nowTIle.locatedPiece);
                movableTIleList.Add(nowTIle);
                SetIsColorAttack(nowTIle);

                if (nowTIle.locatedPiece.pieceType == PieceType.K)
                {
                    isBlock = true;
                    SetIsColorBlockAttack(nowTIle);
                }
                else
                    isEvaluateSkip = true;

                return;
            }
        }
    }
}
