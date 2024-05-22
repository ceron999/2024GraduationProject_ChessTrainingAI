using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    GameObject availableCircle;
    public PlayerColor nowLocateColor;                    //���� ��ġ�� Piece;

    public bool isSelectedMovalleTile = false;      //MovableTiles�� ���� �� �ߺ����� ��Ÿ���� �ʱ� ���� �߰�

    //���� �̵� ������ Ÿ�� ��ġ�� ���� �����ֱ� ���Ͽ� ����
    public void SetAvailableCircle(bool isActive)
    {
        availableCircle.SetActive(isActive);
    }
}
