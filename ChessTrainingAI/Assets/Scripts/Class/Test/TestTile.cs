using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTile : MonoBehaviour
{
    public bool isWhiteAttack;
    public bool isWhiteBlockAttack;       // ����̳� ��, ���� ���θ��� ���� ����� ���
    public bool isBlackAttack;
    public bool isBlackBlockAttack;

    public TIleName tileName;

    public TestPiece locatedPiece;                    //���� ��ġ�� Piece


    // �� �ϸ��� ���� �ʱ�ȭ�Ҷ� �� �Լ�
    public void ClearTileInfo()
    {
        isWhiteAttack = false;
        isWhiteBlockAttack = false;
        isBlackAttack = false;
        isBlackBlockAttack = false;
    }
}
