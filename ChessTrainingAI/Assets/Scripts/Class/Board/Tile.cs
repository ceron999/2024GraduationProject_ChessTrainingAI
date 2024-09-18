using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public enum TIleName
{
    a1, a2, a3, a4, a5, a6, a7, a8, 
    b1, b2, b3, b4, b5, b6, b7, b8,
    c1, c2, c3, c4, c5, c6, c7, c8,
    d1, d2, d3, d4, d5, d6, d7, d8,
    e1, e2, e3, e4, e5, e6, e7, e8,
    f1, f2, f3, f4, f5, f6, f7, f8,
    g1, g2, g3, g4, g5, g6, g7, g8,
    h1, h2, h3, h4, h5, h6, h7, h8,
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
        // �̵��� �⹰�� ������ �н�
        if (ChessManager.instance.nowPiece == null)
            return;

        // Ŭ���� Ÿ���� �⹰ == ������ �⹰�̸� Ŭ�� �ѹ��� piece�� tile �� Ŭ�� �Լ��� ������ ���̹Ƿ� ����
        if (locatedPiece != null && ChessManager.instance.nowPiece.nowPos == locatedPiece.nowPos)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            ChessManager.instance.nowPiece.Move(this.GetComponent<Tile>());
        }
    }
}
