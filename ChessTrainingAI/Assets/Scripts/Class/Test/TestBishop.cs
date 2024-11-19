using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class TestBishop : TestPiece
{
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


       while(nowDir < direction.Count)
        {
            targetVector = nowPos + direction[nowDir] * count;

            // 1. 해당하는 타일이 존재하지 않으면 넘어감
            if (!IsAvailableTIle(targetVector))
            {
                if (nowDir < 4)
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
            else
            {
                InsertAttackPieces(targetVector);
                count++;
            }
        }
    }

    public override void SetMovableTileList()
    {
        base.SetMovableTileList();

        EvaluateLeftUpMoveTiles();
        EvaluateLeftDownMoveTiles();
        EvaluateRightUpMoveTiles();
        EvaluateRightDownMoveTiles();
    }

    void InsertAttackPieces(Vector2Int getVector)
    {
        TestTile nowTIle = TestManager.Instance.testTileList[getVector.x, getVector.y];

        // 2. 타일의 기물이 없으면 이동 타일 추가
        if (nowTIle.locatedPiece != null)
        {
            if(nowTIle.locatedPiece.pieceColor != pieceColor)
                attackPieceList.Add(nowTIle.locatedPiece);
            else if (nowTIle.locatedPiece.pieceColor == pieceColor)
            {
                if (nowDir < 4)
                {
                    count = 0;
                    nowDir++;
                }
            }
        }
        
    }

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

            EvaluateTiles(targetVector);
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

            EvaluateTiles(targetVector);
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

            EvaluateTiles(targetVector);
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

            EvaluateTiles(targetVector);
        }

        isBlock = false;
        isEvaluateSkip = false;
    }

    /// <summary>
    /// 평가 함수에서 반복적으로 나타나는 부분을 뽑아서 함수로 정의함.
    /// 해당 타일을 무시, 이동 가능 타일, 공격 기물 설정하는 함수
    /// </summary>
    /// <param name="getVector"> 목표 타일 벡터 </param>
    void EvaluateTiles(Vector2Int getVector)
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
                //attackPieceList.Add(nowTIle.locatedPiece);
                movableTIleList.Add(nowTIle);
                SetIsColorAttack(nowTIle);

                if (nowTIle.locatedPiece.pieceType == PieceType.K)
                {
                    isBlock = true;
                    SetIsColorBlockAttack(nowTIle);
                }
                else isEvaluateSkip = true;

                return;
            }

        }
    }
    #endregion
}