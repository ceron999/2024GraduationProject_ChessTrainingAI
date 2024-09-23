using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPiece : MonoBehaviour
{
    #region �⹰ ����
    [Header("�⹰ ����")]
    public PieceType pieceType;
    public GameColor pieceColor;
    #endregion

    [Header("�⹰ �̵� ���� ����Ʈ")]
    public Vector2Int nowPos;
    public List<TestTile> movableTIleList = null;                      //���� ��ġ���� �̵� ������ Ÿ��
    public List<TestPiece> attackPieceList = null;                      //���� ��ġ���� �̵� ������ Ÿ��

    public void Move(TestTile getTile)
    {
        TestTile nowTile = TestManager.Instance.testTileList[nowPos.x, nowPos.y];

        nowTile.locatedPiece = null;

        // �ش� ��ġ�� �⹰ �����ϸ� �ش� �⹰ 
        if (getTile.locatedPiece != null)
        {

        }
        getTile.locatedPiece = this;
    }

    #region Ÿ�� �̵� ���� �� ���� �⹰ Ȯ�� �Լ�
    //�ش� Ÿ���� �����ϴ°�? -> �ε��� �ʰ��� Ȯ���ϱ� ���� ���� �Լ�
    protected bool IsAvailableTIle(Vector2Int getTIleVector)
    {
        if (getTIleVector.x < 0 || getTIleVector.x > 7)
            return false;
        else if (getTIleVector.y < 0 || getTIleVector.y > 7)
            return false;

        return true;
    }

    public bool IsAttackKing()
    {
        if (attackPieceList.Count == 0)
            return false;

        for (int i = 0; i < attackPieceList.Count; i++)
        {
            if (attackPieceList[i].pieceType == PieceType.King)
                return true;
        }
        return false;
    }
    #endregion


}
