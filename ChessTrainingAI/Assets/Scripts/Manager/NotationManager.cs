using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotationManager : MonoBehaviour
{
    #region SIngleton
    // �⺸ ��¿� �Ŵ���
    public static NotationManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }    
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    [Header("UI")]
    public GameObject notationUI;
    public GameObject notationPrefab;
    public Transform notationParent;

    public List<Notation> notationList;

    Notation nowNotation;

    /// <summary>
    /// �⺸ ���
    /// </summary>
    /// <param name="getPiece"> �⹰�� ���� �޾ƿ��� ����</param>
    /// <param name="getTile"> Ÿ���� �̸� �޾ƿ��� ����</param>
    /// <param name="isTake"> �� �⹰�� óġ�Ͽ��°� Ȯ�ο�</param>
    public void WriteNotation(Piece getPiece, Tile getTile, bool isTake)
    {
        if (nowNotation == null)
        {
            Notation newNotation = Instantiate(notationPrefab, notationParent).GetComponent<Notation>();
            nowNotation = newNotation;
            notationList.Add(nowNotation);
        }

        nowNotation.gameObject.SetActive(true);
        nowNotation.SetNotation(getPiece.pieceType, getTile.tileName, isTake);

        if(ChessManager.instance.nowTurnColor == GameColor.Black)
        {
            nowNotation = null;
        }
    }

    // �ش� Notation fixText �߰��� ������ �� �ֵ��� ����
    public void AddNotation(string fixText)
    {
        notationList[notationList.Count - 1].AddNotation(fixText);
    }

    public void FixNotation(string fixText)
    {
        notationList[notationList.Count - 1].FixNotation(fixText);
    }
}
