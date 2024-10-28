using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class JsonManager : MonoBehaviour
{

    public static void SaveJson(float[,,] saveData)
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

        jsonText = JsonUtility.ToJson(saveData, true);

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
