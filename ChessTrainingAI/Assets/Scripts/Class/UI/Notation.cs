using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class Notation : MonoBehaviour
{
    public string[] nowNotation = new string[2];
    public TextMeshProUGUI turnCountText;
    public GameObject whiteNotationBtn;
    public GameObject blackNotationBtn;
    public TextMeshProUGUI whiteNotationText;
    public TextMeshProUGUI blackNotationText;

    private void OnEnable()
    {
        turnCountText.gameObject.SetActive(false);
        blackNotationBtn.gameObject.SetActive(false);
        whiteNotationBtn.gameObject.SetActive(false);
    }

    public void SetNotation(PieceType getPieceType, TIleName getTileName, bool isTake)
    {
        turnCountText.gameObject.SetActive(true);
        turnCountText.text = ChessManager.instance.nowTurn.ToString();
        StringBuilder stringBuilder = new StringBuilder();

        // �⹰ �̸� �ֱ�(Pawn ����)
        if(getPieceType != PieceType.P)
        {
            stringBuilder.Append(getPieceType.ToString());
        }
        // �⹰�� óġ�Ͽ��ٸ� x�߰�
        if (isTake)
        {
            stringBuilder.Append("x");
        }
        stringBuilder.Append(getTileName.ToString());



        if (ChessManager.instance.nowTurnColor == GameColor.White)
        {
            whiteNotationBtn.gameObject.SetActive(true);
            whiteNotationText.text = stringBuilder.ToString();
            nowNotation[0] = stringBuilder.ToString();
        }
        else if(ChessManager.instance.nowTurnColor == GameColor.Black)
        {
            blackNotationBtn.gameObject.SetActive(true);
            blackNotationText.text = stringBuilder.ToString();
            nowNotation[1] = stringBuilder.ToString();
        }
    }

    public void AddNotation(string getText)
    {
        if (ChessManager.instance.nowTurnColor == GameColor.White)
        {
            whiteNotationBtn.gameObject.SetActive(true);
            whiteNotationText.text += getText;
            nowNotation[0] = whiteNotationText.text;
        }
        else if (ChessManager.instance.nowTurnColor == GameColor.Black)
        {
            blackNotationBtn.gameObject.SetActive(true);
            blackNotationText.text += getText;
            nowNotation[1] = blackNotationText.text;
        }
    }

    public void FixNotation(string getText)
    {
        if (ChessManager.instance.nowTurnColor == GameColor.White)
        {
            whiteNotationBtn.gameObject.SetActive(true);
            whiteNotationText.text = getText;
            nowNotation[0] = whiteNotationText.text;
        }
        else if (ChessManager.instance.nowTurnColor == GameColor.Black)
        {
            blackNotationBtn.gameObject.SetActive(true);
            blackNotationText.text = getText;
            nowNotation[1] = blackNotationText.text;
        }
    }
}