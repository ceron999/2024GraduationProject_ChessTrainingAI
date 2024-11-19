using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AudeLeLuel.RetroOSUIPack
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<TabButton> tabButtons;
        [SerializeField] private List<GameObject> tabPanels;

        private void Start()
        {
            if (tabButtons.Count != tabPanels.Count)
            {
                Debug.Log("Tabs Button & Panels don't match !");
                return;
            }

            for (int i = 0; i < tabButtons.Count; i++)
            {
                tabButtons[i].OnTabButtonClicked += SelectTab;
            }

            SelectTab(tabButtons[0]);
        }

        void SelectTab(TabButton tabButton)
        {
            int index = tabButtons.IndexOf(tabButton);

            for (int i = 0; i < tabPanels.Count; i++)
            {
                tabPanels[i].SetActive(index == i);
                if (index == i)
                {
                    tabButtons[i].SelectTabButton();
                }
                else
                {
                    tabButtons[i].DeselectTabButton();
                }
            }
        }
    }
}

