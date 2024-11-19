using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameFile : MonoBehaviour
{
    public TextMeshProUGUI fileName;
    public Button onclickEvent;

    private void Awake()
    {
        onclickEvent.onClick.AddListener(ClickFile);
    }

    public void UpdateFIleName(string name)
    {
        fileName.text = name;
    }

    void ClickFile()
    {
        string nowFileName = fileName.text;

        GameManager.Instance.reviewNotationName = nowFileName;
        GameManager.Instance.isReview = true;

        SceneManager.LoadScene("ReviewChessScene");
    }
}
