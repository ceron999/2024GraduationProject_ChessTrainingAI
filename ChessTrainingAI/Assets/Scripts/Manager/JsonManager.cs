using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class JsonManager : MonoBehaviour
{

    public static void SaveNotationJson()
    {
        string jsonText;

        string savePath = Application.dataPath;
        string appender = "/userData/";
        string nameString = "SaveData";
        string dotJson = ".json";

        StringBuilder builder = new StringBuilder(savePath);
        builder.Append(appender);
        if (!Directory.Exists(builder.ToString()))
        {
            //디렉토리가 없는경우 만들어준다
            Directory.CreateDirectory(builder.ToString());

        }
        builder.Append(nameString);
        builder.Append(dotJson);

        SaveNotation nowData = new SaveNotation();

        List<Notation> getNotaion = new List<Notation>();
        getNotaion = NotationManager.instance.notationList;

        for (int i = 0; i < getNotaion.Count; i++)
        {
            nowData.notaionList.Add(getNotaion[i].nowNotation[0]);

            try
            {
                nowData.notaionList.Add(getNotaion[i].nowNotation[1]);
            }
            catch
            {
                Debug.Log("End Notaion");
            }
        }
        jsonText = JsonUtility.ToJson(nowData, true);

        FileStream fileStream = new FileStream(builder.ToString(), FileMode.Create);
        byte[] bytes = Encoding.UTF8.GetBytes(jsonText);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
    }

    public static T ResourceDataLoad<T>(string name)
    {
        T gameData;
        string directory = "JsonData/";

        string appender1 = name;
        StringBuilder builder = new StringBuilder(directory);
        builder.Append(appender1);
        TextAsset jsonString = Resources.Load<TextAsset>(builder.ToString());
        
        gameData = JsonUtility.FromJson<T>(jsonString.ToString());

        return gameData;
    }
}
