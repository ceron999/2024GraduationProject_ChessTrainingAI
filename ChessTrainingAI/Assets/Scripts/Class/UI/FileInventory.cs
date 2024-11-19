using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FileInventory : MonoBehaviour
{
    public GameObject filePrefab;
    public Transform fileParent;

    public Button exitBtn;

    private void Awake()
    {
        exitBtn.onClick.AddListener(ExitInventory);
    }

    private void Start()
    {
        // 1. ����� ���� �ҷ�����
        InstantiateFiles();

        // 2. �ҷ��� ����ŭ 
    }

    void InstantiateFiles()
    {
        string savePath = Application.dataPath;
        string folderName = "/userData/";

        //1. ���� ���� ����
        DirectoryInfo di = new DirectoryInfo(savePath + folderName);
        FileInfo[] fiArr = di.GetFiles("*.json");

        GameObject temp;
        for (int i = 0; i < fiArr.Length; i++)
        {
            temp = Instantiate(filePrefab, fileParent);
            temp.SetActive(true);

            if(temp.TryGetComponent<GameFile>(out GameFile gf))
            {
                string nowFileName = "Game" + i;
                gf.UpdateFIleName(nowFileName);
            }
        }
    }

    void ExitInventory()
    {
        this.gameObject.SetActive(false);
    }
}
