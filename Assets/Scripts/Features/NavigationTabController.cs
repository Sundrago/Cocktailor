using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Features.Quiz;
using Features.Quize;
using Features.RecipeViewer;
using UnityEngine.Serialization;

public enum TabOptions { RecipeCard, MemoryCard, Test, Search, ShowAll, Setting }

public class NavigationTabController : MonoBehaviour
{
    [Header("Managers and Controllers")]
    [SerializeField] private RecipeViewerManager recipeViewerManager;
    [SerializeField] private QuizManager quizManager;
    [SerializeField] private SfxManager sfxManager;
    
    [Header("UI Elements")]
    [SerializeField] private GameObject MainPanel, SearchPanel, TestPanel, MoaPanel, SettingsPanel;
    [SerializeField] private GameObject[] tabButtons = new GameObject[6];
    [SerializeField] private Transform tabSelectedIndicator;

    private void Start()
    {
        SwitchToTab((int)TabOptions.RecipeCard);
    }

    public void SwitchToTab(int index)
    {
        TabOptions tabOptions = (TabOptions)index;
        
        if (recipeViewerManager.isInQuizMode)
        {
            quizManager.PromptUserForQuizExit();
            return;
        }

        SetTargetTab(index);
        sfxManager.PlaySfx(3);

        recipeViewerManager.ToggleTabVisibility(tabOptions == TabOptions.RecipeCard);
        MainPanel.SetActive(tabOptions == TabOptions.RecipeCard || tabOptions == TabOptions.MemoryCard);
        SearchPanel.SetActive(tabOptions == TabOptions.Search);
        TestPanel.SetActive(tabOptions == TabOptions.Test);
        MoaPanel.SetActive(tabOptions == TabOptions.ShowAll);
        SettingsPanel.SetActive(tabOptions == TabOptions.Setting);
    }
    
    private void SetTargetTab(int target)
    {
        if (DOTween.IsTweening(tabSelectedIndicator)) DOTween.Kill(tabSelectedIndicator);

        Vector3 targetPos = new Vector3(tabSelectedIndicator.position.x, tabButtons[target].transform.position.y, 0);
        tabSelectedIndicator.DOMoveY(targetPos.y, 0.7f)
            .SetEase(Ease.OutExpo);
    }
}
