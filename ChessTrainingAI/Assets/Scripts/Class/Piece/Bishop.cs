using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override void EvaluateMove()
    {
        EvaluateLeftUpMoveTiles();
        EvaluateRightUpMoveTiles();
        EvaluateLeftDownMoveTiles();
        EvaluateRightDownMoveTiles();
    }

    #region 대각선 이동
    //(-1.+1)
    void EvaluateLeftUpMoveTiles()
    {
        Vector2Int evaluateVector = nowPos;
        for (int i = 1; ; i++)
        {
            evaluateVector = new Vector2Int(nowPos.x - i, nowPos.y + i);

            // 1. 해당하는 타일이 존재하지 않으면 넘어감
            if (!IsAvailableTIle(evaluateVector))
                break;

            // 2. 타일의 기물이 없으면 이동 타일 추가
            if (ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece == null)
                movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y]);
            else
            {
                // 3. 타일의 기물 색 == 선택한 기물의 색이면 넘어감
                if (ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 4. 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y]);
                    break;
                }
            }
        }
    }

    //(+1.+1)
    void EvaluateRightUpMoveTiles()
    {
        Vector2Int evaluateVector = nowPos;
        for (int i = 1; ; i++)
        {
            evaluateVector = new Vector2Int(nowPos.x + i, nowPos.y + i);

            // 1. 해당하는 타일이 존재하지 않으면 넘어감
            if (!IsAvailableTIle(evaluateVector))
                break;

            // 2. 타일의 기물이 없으면 이동 타일 추가
            if (ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece == null)
                movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y]);
            else
            {
                // 3. 타일의 기물 색 == 선택한 기물의 색이면 넘어감
                if (ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 4. 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y]);
                    break;
                }
            }
        }
    }

    //(-1.-1)
    void EvaluateLeftDownMoveTiles()
    {
        Vector2Int evaluateVector = nowPos;
        for (int i = 1; ; i++)
        {
            evaluateVector = new Vector2Int(nowPos.x - i, nowPos.y - i);

            // 1. 해당하는 타일이 존재하지 않으면 넘어감
            if (!IsAvailableTIle(evaluateVector))
                break;

            // 2. 타일의 기물이 없으면 이동 타일 추가
            if (ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece == null)
                movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y]);
            else
            {
                // 3. 타일의 기물 색 == 선택한 기물의 색이면 넘어감
                if (ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 4. 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y]);
                    break;
                }
            }
        }
    }

    //(+1.-1)
    void EvaluateRightDownMoveTiles()
    {
        Vector2Int evaluateVector = nowPos;
        for (int i = 1; ; i++)
        {
            evaluateVector = new Vector2Int(nowPos.x + i, nowPos.y - i);

            // 1. 해당하는 타일이 존재하지 않으면 넘어감
            if (!IsAvailableTIle(evaluateVector))
                break;

            // 2. 타일의 기물이 없으면 이동 타일 추가
            if (ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece == null)
                movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y]);
            else
            {
                // 3. 타일의 기물 색 == 선택한 기물의 색이면 넘어감
                if (ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece.pieceColor == pieceColor)
                    break;

                // 4. 타일의 기물 색 != 선택한 기물의 색이면 공격 기물 추가, 이동 타일 추가
                else
                {
                    attackPieceList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y].locatedPiece);
                    movableTIleList.Add(ChessManager.instance.chessTileList[evaluateVector.x, evaluateVector.y]);
                    break;
                }
            }
        }
    }
    #endregion
}
