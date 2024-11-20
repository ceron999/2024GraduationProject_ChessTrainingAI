using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndBtn : MonoBehaviour
{
    public Button exitBtn;


    void Start()
    {
        exitBtn.onClick.AddListener(ExitChessScene);
    }

    void ExitChessScene()
    {
        JsonManager.SaveNotationJson();
        SceneManager.LoadScene("StartScene");
    }
}
