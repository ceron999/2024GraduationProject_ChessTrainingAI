using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace AudeLeLuel.RetroOSUIPack
{

    public class TabButton : MonoBehaviour
    {
        [SerializeField] public Button button;
        [SerializeField] private Image targetGraphic;
        [SerializeField] private Sprite activeTabSprite;
        [SerializeField] private Sprite inactiveTabSprite;


        public event Action<TabButton> OnTabButtonClicked;

        private void Start()
        {
            button.onClick.AddListener(() => {
                OnTabButtonClicked?.Invoke(this); 
            });
        }

        public void SelectTabButton()
        {
            targetGraphic.sprite = activeTabSprite;
        }

        public void DeselectTabButton()
        {
            targetGraphic.sprite = inactiveTabSprite;
        }
    }

}