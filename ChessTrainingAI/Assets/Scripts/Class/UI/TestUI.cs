using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    public void Save()
    {
        JsonManager.SaveNotationJson();
    }
}
