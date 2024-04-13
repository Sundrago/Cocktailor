using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MemoryCardTabController : MonoBehaviour
{
    [SerializeField] private Animator cardOpenAnimator;
    [SerializeField] private Animator cardCloseAnimator;
    [SerializeField] private Text cardDescriptionText, cardDescriptionText2;
    [SerializeField] private RectTransform scaler;
    
    private bool isOpen = true;
    
    public void ToggleCardVisibility()
    {
        UpdateCardVisibility(!isOpen);
    }

    public void UpdateCardVisibility(bool shouldShowCard, bool skipAnimation = false)
    {
        this.isOpen = shouldShowCard;

        if (skipAnimation)
        {
            cardOpenAnimator.SetTrigger(shouldShowCard ? "idle" : "hidden");
            cardCloseAnimator.SetTrigger(shouldShowCard ? "hidden" : "idle");
            return;
        }
        cardOpenAnimator.SetTrigger(shouldShowCard ? "show" : "hide");
        cardCloseAnimator.SetTrigger(shouldShowCard ? "hide" : "show");
    }

    public void SetCardDescriptionText(string str)
    {
        cardDescriptionText.text = str;
    }
    
    public void SetCardDescriptionText(string str1, string str2)
    {
        cardDescriptionText.text = str1;
        cardDescriptionText2.text = str2;
    }
}
