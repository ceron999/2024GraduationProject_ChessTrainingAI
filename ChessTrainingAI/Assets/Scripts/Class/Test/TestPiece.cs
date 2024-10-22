using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestPiece
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

    public TestPiece()
    {
        pieceType = PieceType.Null;
        pieceColor = GameColor.Null;
        nowPos = Vector2Int.zero;
        movableTIleList = new List<TestTile>();
        attackPieceList = new List<TestPiece>();
    }

    public virtual void SetAttackPieceList()
    {

    }

    // Piece -> testPiece�� ������ ��ȯ�ϴ� �Լ�
    public void SetPieceInfo(Piece getPiece)
    {
        pieceType = getPiece.pieceType;
        pieceColor = getPiece.pieceColor;
        nowPos = getPiece.nowPos;

        movableTIleList.Clear();
        for (int i = 0; i < getPiece.movableTIleList.Count; i++)
        {
            movableTIleList.Add(TestManager.Instance.ConvertTile2TestTile(getPiece.movableTIleList[i]));
        }

        attackPieceList.Clear();
        for (int i = 0; i < getPiece.attackPieceList.Count; i++)
        {
            attackPieceList.Add(TestManager.Instance.ConvertPiece2TestPiece(getPiece.attackPieceList[i]));
        }
    }

    public void Move(TestTile getTile)
    {
        TestTile nowTile = TestManager.Instance.testTileList[nowPos.x, nowPos.y];

        nowTile.locatedPiece = null;
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
            if (attackPieceList[i].pieceType == PieceType.K)
                return true;
        }
        return false;
    }
    #endregion


}
