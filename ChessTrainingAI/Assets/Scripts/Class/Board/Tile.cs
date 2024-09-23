using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public enum TIleName
{
    a1, b1, c1, d1, e1, f1, g1, h1,
    a2, b2, c2, d2, e2, f2, g2, h2,
    a3, b3, c3, d3, e3, f3, g3, h3,
    a4, b4, c4, d4, e4, f4, g4, h4,
    a5, b5, c5, d5, e5, f5, g5, h5,
    a6, b6, c6, d6, e6, f6, g6, h6,
    a7, b7, c7, d7, e7, f7, g7, h7,
    a8, b8, c8, d8, e8, f8, g8, h8,


    Null = 1000
}

public class Tile : MonoBehaviour
{
    [SerializeField]
    GameObject availableCircle;
    public bool isWhiteAttack;
    public bool isWhiteBlockAttack;       // ����̳� ��, ���� ���θ��� ���� ����� ���
    public bool isBlackAttack;
    public bool isBlackBlockAttack;

    public TIleName tileName;

    public Piece locatedPiece;                    //���� ��ġ�� Piece

    //���� �̵� ������ Ÿ�� ��ġ�� ���� �����ֱ� ���Ͽ� ����
    public void SetAvailableCircle(bool isActive)
    {
        availableCircle.SetActive(isActive);
    }


    // �� �ϸ��� ���� �ʱ�ȭ�Ҷ� �� �Լ�
    public void ClearTileInfo()
    {
        isWhiteAttack = false;
        isWhiteBlockAttack = false;
        isBlackAttack = false;
        isBlackBlockAttack = false;
    }

    private void OnMouseDown()
    {
        Debug.Log(tileName.ToString());
        // ������ �⹰ �̵� Ÿ�� �����ֱ�
        if (locatedPiece != null)
        {
            locatedPiece.ShowMovableTiles();
        }

        if (ChessManager.instance.nowPiece != null && availableCircle.activeSelf == true)
            ChessManager.instance.nowPiece.Move(this.GetComponent<Tile>());
    }
}
