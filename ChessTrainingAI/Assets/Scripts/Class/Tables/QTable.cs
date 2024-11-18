using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class QTable
{
    float[,,,] Q_Table = new float[60, 6, 64, 64];     // [turn, 기물 타입, 시작 타일, 도착 타일] 

    public QTable()
    {
        for (int x = 0; x < 60; x++)
            for (int y = 0; y < 6; y++)
                for (int z = 0; z < 64; z++)
                    for (int w = 0; w < 64; w++)
                        Q_Table[x, y, z, w] = 0;
    }


    public void LoadQ_Table()
    {
        // 데이터 파일을 받아서 저장한다.
    }

    public void UpdateQ_Table()
    {
        //for (int x = 0; x < 64; x++)
        //{
        //    for (int y = 0; y < 64; y++)
        //    {
        //        int nowPiece = 0;
        //        if (state.nowAction.nowActionArr[x, y] != 0)
        //        {
        //            switch (MathF.Abs(state.nowAction.nowActionArr[x, y]))
        //            {
        //                case 1:
        //                    nowPiece = 0; break;
        //                case 3:
        //                    nowPiece = 1; break;
        //                case 3.5f:
        //                    nowPiece = 2; break;
        //                case 5:
        //                    nowPiece = 3; break;
        //                case 8:
        //                    nowPiece = 4; break;
        //                case 10000:
        //                    nowPiece = 5; break;
        //            }
        //        }
        //        //여기서 테이블 값에 보상값을 추가한다
        //        Q_Table[nowStateCount, nowPiece, x, y] += 0;
        //    }
        //}
    }


    public void DebugQ_TableArr()
    {
        StringBuilder arr = new StringBuilder();
        arr.Append("Q_Table 디버깅\n\n");

        for (int x = 0; x < 10; x++)
        {
            arr.Append("현재 턴 수: " + x + "\n");
            for (int y = 0; y < 6; y++)
            {
                arr.Append("현재 기물 : ");
                switch (y)
                {
                    case 0:
                        arr.Append("Pawn\n"); break;
                    case 1:
                        arr.Append("Knight\n"); break;
                    case 2:
                        arr.Append("Bishop\n"); break;
                    case 3:
                        arr.Append("Rook\n"); break;
                    case 4:
                        arr.Append("Queen\n"); break;
                    case 5:
                        arr.Append("King\n"); break;
                }
                for (int z = 0; z < 64; z++)
                {
                    for (int w = 0; w < 64; w++)
                    {
                        arr.Append(Q_Table[x, y, z, w]);
                        if (w < 63)
                        {
                            arr.Append(", ");
                        }
                        else
                        {
                            arr.Append("]\n");
                        }
                    }
                }
            }
        }

        Debug.Log(arr.ToString());
    }

    public void DebugQ_TableNotation()
    {
        StringBuilder arr = new StringBuilder();
        arr.Append("Q_Table 디버깅\n\n");

        for (int x = 0; x < 10; x++)
        {
            arr.Append("현재 턴 수: " + x + "\n");
            for (int y = 0; y < 6; y++)
            {
                for (int z = 0; z < 64; z++)
                {
                    for (int w = 0; w < 64; w++)
                    {
                        switch (MathF.Abs(Q_Table[x, y, z, w]))
                        {
                            case 1:
                                arr.Append("Pawn : ");
                                arr.Append((TIleName)Enum.Parse(typeof(TIleName), z.ToString()) + " -> "
                                        + (TIleName)Enum.Parse(typeof(TIleName), w.ToString()) + "\n");
                                break;
                            case 3:
                                arr.Append("Knight : ");
                                arr.Append((TIleName)Enum.Parse(typeof(TIleName), z.ToString()) + " -> "
                                        + (TIleName)Enum.Parse(typeof(TIleName), w.ToString()) + "\n");
                                break;
                            case 3.5f:
                                arr.Append("Bishop : ");
                                arr.Append((TIleName)Enum.Parse(typeof(TIleName), z.ToString()) + " -> "
                                        + (TIleName)Enum.Parse(typeof(TIleName), w.ToString()) + "\n");
                                break;
                            case 5:
                                arr.Append("Rook : ");
                                arr.Append((TIleName)Enum.Parse(typeof(TIleName), z.ToString()) + " -> "
                                        + (TIleName)Enum.Parse(typeof(TIleName), w.ToString()) + "\n");
                                break;
                            case 8:
                                arr.Append("Queen : ");
                                arr.Append((TIleName)Enum.Parse(typeof(TIleName), z.ToString()) + " -> "
                                        + (TIleName)Enum.Parse(typeof(TIleName), w.ToString()) + "\n");
                                break;
                            case 10000:
                                arr.Append("King : ");
                                arr.Append((TIleName)Enum.Parse(typeof(TIleName), z.ToString()) + " -> "
                                        + (TIleName)Enum.Parse(typeof(TIleName), w.ToString()) + "\n");
                                break;
                            default:
                                continue;
                        }


                    }
                }
            }
        }

        Debug.Log(arr.ToString());
    }
}
