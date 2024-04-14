using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Cocktailor
{
    public class SecondIngredientSelector : RecipeSelectorBase
    {
        [SerializeField] private List<Button> buttons;
        [SerializeField] private List<Text> buttonTexts;

        [Button]
        public void InitAndOpen(string[] strings, int currentSelectionIdx, Action<int, string> onButtonClick)
        {
            selectedIndex = currentSelectionIdx;
            UpdateIconImages();

            for (var i = 0; i < buttons.Count; i++) buttons[i].gameObject.SetActive(i < strings.Length);

            for (var i = 0; i < strings.Length; i++)
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