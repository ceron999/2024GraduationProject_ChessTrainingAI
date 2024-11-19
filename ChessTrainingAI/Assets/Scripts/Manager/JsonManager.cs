using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

[Serializable]
public class JsonNotationWrapper
{
    public List<NotationInfo> list;

    public JsonNotationWrapper()
    {
        list = new List<NotationInfo>();
    }
}

public class JsonManager : MonoBehaviour
{

    public static void SaveNotationJson()
    {
        string savePath = Application.dataPath;
        string folderName = "/userData/";
        string nameString = "Game";
        string dotJson = ".json";

        //1. 폴더 개수 세기
        DirectoryInfo di = new DirectoryInfo(savePath + folderName);
        FileInfo[] fiArr = di.GetFiles("*.json");
        string gameCountString = fiArr.Length.ToString();

        // 2. 폴더 생성 및 
        StringBuilder builder = new StringBuilder(savePath);
        builder.Append(folderName);
        if (!Directory.Exists(builder.ToString()))
        {
            //디렉토리가 없는경우 만들어준다
            Directory.CreateDirectory(builder.ToString());

        }
        builder.Append(nameString);
        builder.Append(gameCountString);
        builder.Append(dotJson);

        JsonNotationWrapper nowData = new JsonNotationWrapper();

        List<Notation> getNotaion = new List<Notation>();
        getNotaion = NotationManager.instance.notationList;

        for (int i = 0; i < getNotaion.Count; i++)
        {
            nowData.list.Add(getNotaion[i].whiteNotation);

            try
            {
                nowData.list.Add(getNotaion[i].blackNotation);
            }
            catch
            {
                Debug.Log("End Notaion");
            }
        }

        string jsonText;
        jsonText = JsonUtility.ToJson(nowData, true);
        
        FileStream fileStream = new FileStream(builder.ToString(), FileMode.Create);
        byte[] bytes = Encoding.UTF8.GetBytes(jsonText);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
    }

    public static JsonNotationWrapper LoadNotationJsonFile(string name)
    {
        JsonNotationWrapper gameData;
        string savePath = Application.dataPath;
        string folderName = "/userData/";

        StringBuilder builder = new StringBuilder(savePath);
        builder.Append(folderName);

        DirectoryInfo directoryInfo = new DirectoryInfo(builder.ToString());
        FileInfo[] fileInfos = directoryInfo.GetFiles(name + ".json");
        Debug.Log(fileInfos[0].FullName);
        string jsonData = File.ReadAllText(fileInfos[0].FullName);
        gameData = JsonUtility.FromJson<JsonNotationWrapper>(jsonData);

        return gameData;
    }
}
