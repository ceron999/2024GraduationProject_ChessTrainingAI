using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override void EvaluateMove()
    {
        EvaluateLeftMoveTiles();
        EvaluateRightMoveTiles();
        EvaluateUpMoveTiles();
        EvaluateDownMoveTiles();
        EvaluateLeftUpMoveTiles();
        EvaluateRightUpMoveTiles();
        EvaluateLeftDownMoveTiles();
        EvaluateRightDownMoveTiles();
    }

    #region 수직 이동
    void EvaluateLeftMoveTiles()
    {
        for (int i = 1; nowPos.x - i >= 0; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x - i, nowPos.y);

            // 0. 해당 타일이 존재하지 않으면 중단
            if (!IsAvailableTIle(targetVector))
                break;

            // 1. 해당 타일이 비어있으면 이동 타일로 추가
            if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece == null)
            {
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
            }
            else
            {
                // 2. 해당 타일의 기물의 색 == 선택한 기물의 색이면 넘어감
                if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 3. 해당 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                    break;
                }
            }
        }
    }

    void EvaluateRightMoveTiles()
    {
        for (int i = 1; nowPos.x + i <= 7; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x + i, nowPos.y);

            // 0. 해당 타일이 존재하지 않으면 중단
            if (!IsAvailableTIle(targetVector))
                break;

            // 1. 해당 타일이 비어있으면 이동 타일로 추가
            if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece == null)
            {
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
            }
            else
            {
                // 2. 해당 타일의 기물의 색 == 선택한 기물의 색이면 넘어감
                if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 3. 해당 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                    break;
                }
            }
        }
    }

    void EvaluateUpMoveTiles()
    {
        for (int i = 1; nowPos.y + i <= 7; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x, nowPos.y + i);

            // 0. 해당 타일이 존재하지 않으면 중단
            if (!IsAvailableTIle(targetVector))
                break;

            // 1. 해당 타일이 비어있으면 이동 타일로 추가
            if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece == null)
            {
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
            }
            else
            {
                // 2. 해당 타일의 기물의 색 == 선택한 기물의 색이면 넘어감
                if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 3. 해당 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                    break;
                }
            }
        }
    }

    void EvaluateDownMoveTiles()
    {
        for (int i = 1; nowPos.y - i >= 0; i++)
        {
            Vector2Int targetVector = new Vector2Int(nowPos.x, nowPos.y - i);

            // 0. 해당 타일이 존재하지 않으면 중단
            if (!IsAvailableTIle(targetVector))
                break;

            // 1. 해당 타일이 비어있으면 이동 타일로 추가
            if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece == null)
            {
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
            }
            else
            {
                // 2. 해당 타일의 기물의 색 == 선택한 기물의 색이면 넘어감
                if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 3. 해당 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                    break;
                }
            }
        }
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

            // 2. 타일의 기물이 없으면 이동 타일 추가
            if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece == null)
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
            else
            {
                // 3. 타일의 기물 색 == 선택한 기물의 색이면 넘어감
                if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 4. 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                    break;
                }
            }
        }
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

            // 2. 타일의 기물이 없으면 이동 타일 추가
            if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece == null)
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
            else
            {
                // 3. 타일의 기물 색 == 선택한 기물의 색이면 넘어감
                if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 4. 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                    break;
                }
            }
        }
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

            // 2. 타일의 기물이 없으면 이동 타일 추가
            if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece == null)
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
            else
            {
                // 3. 타일의 기물 색 == 선택한 기물의 색이면 넘어감
                if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 4. 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                    break;
                }
            }
        }
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

            // 2. 타일의 기물이 없으면 이동 타일 추가
            if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece == null)
                movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
            else
            {
                // 3. 타일의 기물 색 == 선택한 기물의 색이면 넘어감
                if (ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 4. 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[targetVector.x, targetVector.y]);
                    break;
                }
            }
        }
    }
    #endregion
}
