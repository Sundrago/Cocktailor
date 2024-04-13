using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Quiz
{
    public class SecondIngredientSelector : IngredientSelectorUI
    {
        [SerializeField] private List<Button> buttons;
        [SerializeField] private List<Text> buttonTexts;
        // [SerializeField] private List<Image> buttonImages;

        // [Sirenix.OdinInspector.Button]
        // private void GetAllItems()
        // {
        //     buttons = transform.GetComponentsInChildren<Button>().ToList();
        //     buttonTexts = transform.GetComponentsInChildren<Text>().ToList();
        //     buttonImages = transform.GetComponentsInChildren<Image>().ToList();
        // }

        [Sirenix.OdinInspector.Button]
        public void InitAndOpen(string[] strings, int currentSelectionIdx, Action<int, string> onButtonClick)
        {
            selectedIndex = currentSelectionIdx;
            UpdateIconImages();

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(i < strings.Length);
            }

            for (int i = 0; i < strings.Length; i++)
            {
                buttonTexts[i].text = strings[i];
                buttons[i].onClick.RemoveAllListeners();
                var i1 = i;
                buttons[i].onClick.AddListener(() => onButtonClick(i1, strings[i1]));
            }
            
            OpenPanel();
        }

    }
}