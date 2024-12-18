using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public Button playingComputerBtn;
    public Button reviewChessBtn;
    public Button exitBtn;

    public GameObject inventory;

    private void Awake()
    {
        playingComputerBtn.onClick.AddListener(PlayComputer);
        reviewChessBtn.onClick.AddListener(ReviewGame);
        exitBtn.onClick.AddListener(Exit);
    }

    void PlayComputer()
    {
        SceneManager.LoadScene("ChessScene");
    }

    void ReviewGame()
    {
        inventory.SetActive(true);
    }


    void Exit()
    {
        Application.Quit();
    }

}
