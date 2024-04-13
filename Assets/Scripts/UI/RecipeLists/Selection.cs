using System;
using System.Collections;
using System.Collections.Generic;
using Cocktailor;
using Cocktailor.Utility;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    [SerializeField] public Text indexTxt, nameTxt;
    [SerializeField] public Image smallIco;
    [SerializeField] public Button button;

    public void Init(int cockTailIndex, int buttonIndex, Action<int> action)
    {
        indexTxt.text = buttonIndex.ToString();
        nameTxt.text = CocktailRecipeManger.GetCocktailRecipeByIndex(cockTailIndex).Name;
        var userState = PlayerData.GetUserState(cockTailIndex);
        smallIco.sprite = CocktailIconManager.Instance.GetSmallMarkIconAtIndexOf((int)userState);

        gameObject.SetActive(true);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(()=>action(cockTailIndex));
    }

    public void UpdateIcon(MemorizedState userState)
    {
        smallIco.sprite = CocktailIconManager.Instance.GetSmallMarkIconAtIndexOf((int)userState);
    }
}
