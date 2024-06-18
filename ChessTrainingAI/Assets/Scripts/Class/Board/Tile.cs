using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    GameObject availableCircle;
    public GameColor nowLocateColor;                    //���� ��ġ�� Piece;
    public bool isAttackedTile = false;                 //���ݴ��ϴ� Ÿ����(ŷ ������ ���� ����

    public bool isSelectedMovalleTile = false;      //MovableTiles�� ���� �� �ߺ����� ��Ÿ���� �ʱ� ���� �߰�

    //���� �̵� ������ Ÿ�� ��ġ�� ���� �����ֱ� ���Ͽ� ����
    public void SetAvailableCircle(bool isActive)
    {
        availableCircle.SetActive(isActive);
    }

    void EvaluateAttackTile()
    {

    }
}
