using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public JsonNotationWrapper reviewNotaion;

    [Header("UI")]
    public GameObject notationUI;
    public GameObject notationPrefab;
    public Transform notationParent;

    public List<Notation> notationList;

    Notation nowNotation;

    private void Start()
    {
        if(GameManager.Instance.isReview)
        {
            reviewNotaion = JsonManager.LoadNotationJsonFile(GameManager.Instance.reviewNotationName);
            ReviewManager.Instance.nowReviewNotation = reviewNotaion;
            SetNotations();
        }
    }

    /// <summary>
    /// �⺸ ���
    /// </summary>
    /// <param name="getPiece"> �⹰�� ���� �޾ƿ��� ����</param>
    /// <param name="endTile"> Ÿ���� �̸� �޾ƿ��� ����</param>
    /// <param name="isTake"> �� �⹰�� óġ�Ͽ��°� Ȯ�ο�</param>
    public void WriteNotation(Piece getPiece, Tile startTile, Tile endTile, bool isTake)
    {
        if (nowNotation == null)
        {
            Notation newNotation = Instantiate(notationPrefab, notationParent).GetComponent<Notation>();
            nowNotation = newNotation;
            notationList.Add(nowNotation);
        }

        nowNotation.gameObject.SetActive(true);
        nowNotation.SetNotation(getPiece.pieceType, endTile.tileName, isTake);
        

        if(ChessManager.instance.nowTurnColor == GameColor.Black)
        {
            nowNotation.blackNotation.SetNotationPositions(startTile.tileName, endTile.tileName);
            nowNotation = null;
        }
        else if(ChessManager.instance.nowTurnColor == GameColor.White)
        {
            nowNotation.whiteNotation.SetNotationPositions(startTile.tileName, endTile.tileName);
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

    void SetNotations()
    {
        for(int i =0; i < reviewNotaion.list.Count; i++)
        {
            if(i%2 ==0)
            {
                Notation newNotation = Instantiate(notationPrefab, notationParent).GetComponent<Notation>();
                nowNotation = newNotation;
                nowNotation.gameObject.SetActive(true);
                notationList.Add(nowNotation);

                nowNotation.turnCountText.text = ((int)i / 2).ToString();
                nowNotation.whiteNotation.notation = reviewNotaion.list[i].notation;
                nowNotation.whiteNotation.startPos = reviewNotaion.list[i].startPos;
                nowNotation.whiteNotation.endPos = reviewNotaion.list[i].endPos;

                nowNotation.UpdateNotationPrefab();
            }
            else
            {
                nowNotation.blackNotation.notation = reviewNotaion.list[i].notation;
                nowNotation.blackNotation.startPos = reviewNotaion.list[i].startPos;
                nowNotation.blackNotation.endPos = reviewNotaion.list[i].endPos;

                nowNotation.UpdateNotationPrefab();
            }
        }
    }
}
