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

    /// <summary>
    /// Ÿ���� ������ ������ �׽�Ʈ Ÿ�Ͽ� �����մϴ�
    /// </summary>
    /// <param name="getTile"> �����ϱ� ���ϴ� Ÿ�� </param>
    public void SetTileInfo(Tile getTile)
    {
        isWhiteAttack = getTile.isWhiteAttack;
        isBlackAttack = getTile.isBlackAttack;
        isBlackBlockAttack = getTile.isBlackBlockAttack;

        tileName = getTile.tileName;

        if (getTile.locatedPiece != null)
        {
            locatedPiece.SetPieceInfo(getTile.locatedPiece);
        }
    }

    // �� �ϸ��� ���� �ʱ�ȭ�Ҷ� �� �Լ�
    public void ClearTileInfo()
    {
        locatedPiece = null;
        isWhiteAttack = false;
        isWhiteBlockAttack = false;
        isBlackAttack = false;
        isBlackBlockAttack = false;
    }

    // �� �ϸ��� ���� �ʱ�ȭ�Ҷ� �� �Լ�
    public void ResetTileInfo()
    {
        isWhiteAttack = false;
        isWhiteBlockAttack = false;
        isBlackAttack = false;
        isBlackBlockAttack = false;

        locatedPiece = null;
    }
}
