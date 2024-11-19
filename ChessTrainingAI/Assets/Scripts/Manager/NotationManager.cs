using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotationManager : MonoBehaviour
{
    #region SIngleton
    // 기보 출력용 매니저
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
    /// 기보 출력
    /// </summary>
    /// <param name="getPiece"> 기물의 종류 받아오기 전용</param>
    /// <param name="endTile"> 타일의 이름 받아오기 전용</param>
    /// <param name="isTake"> 적 기물을 처치하였는가 확인용</param>
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

    // 해당 Notation fixText 추가해 변경할 수 있도록 수정
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
