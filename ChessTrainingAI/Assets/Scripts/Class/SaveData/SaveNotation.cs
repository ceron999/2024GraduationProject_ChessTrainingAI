using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveNotation
{
    public List<NotationInfo> notaionList;

    public SaveNotation()
    {
        notaionList = new List<NotationInfo>();
    }
}

[System.Serializable]
public class NotationInfo
{
    public string notation;
    public string startPos;
    public string endPos;

    public NotationInfo()
    {
        notation = string.Empty;
        startPos = string.Empty;
        endPos = string.Empty;
    }

    public void SetNotationInfo(string getNotation, TIleName getStartPos, TIleName getEndPos)
    {
        notation = getNotation;
        startPos = getStartPos.ToString();
        endPos = getEndPos.ToString();
    }

    public void SetNotation(string getNotation)
    {
        notation = getNotation;
    }

    public void SetNotationPositions(TIleName getStartPos, TIleName getEndPos)
    {
        startPos = getStartPos.ToString();
        endPos = getEndPos.ToString();
    }
}
