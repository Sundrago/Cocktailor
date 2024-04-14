using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public enum TabOptions
{
    RecipeCard,
    MemoryCard,
    Test,
    Search,
    ShowAll,
    Setting
}

namespace Cocktailor
{
    public class NavigationTabController : MonoBehaviour
    { 
        [Header("Managers and Controllers")] 
        [SerializeField] private RecipeViewerPanel recipeViewerPanel; 
        [SerializeField] private QuizPanel quizPanel;
        [SerializeField] private SfxManager sfxManager;

        [Header("QuizCard Elements")] 
        [SerializeField] private GameObject MainPanel, SearchPanel, TestPanel, MoaPanel, SettingsPanel;
        [SerializeField] private GameObject[] tabButtons = new GameObject[6];
        [SerializeField] private Transform tabSelectedIndicator;

        private void Start()
        {
            SwitchToTab((int)TabOptions.RecipeCard);
        }

        public void SwitchToTab(int index)
        {
            var tabOptions = (TabOptions)index;

            if (recipeViewerPanel.isInQuizMode)
            {
                quizPanel.PromptUserForQuizExit();
                return;
            }

            SetTargetTab(index);
            sfxManager.PlaySfx(3);

            recipeViewerPanel.ToggleTabVisibility(tabOptions == TabOptions.RecipeCard);
            MainPanel.SetActive(tabOptions == TabOptions.RecipeCard || tabOptions == TabOptions.MemoryCard);
            SearchPanel.SetActive(tabOptions == TabOptions.Search);
            TestPanel.SetActive(tabOptions == TabOptions.Test);
            MoaPanel.SetActive(tabOptions == TabOptions.ShowAll);
            SettingsPanel.SetActive(tabOptions == TabOptions.Setting);
        }

        private void SetTargetTab(int target)
        {
            if (DOTween.IsTweening(tabSelectedIndicator)) DOTween.Kill(tabSelectedIndicator);

            var targetPos =
                new Vector3(tabSelectedIndicator.position.x, tabButtons[target].transform.position.y, 0);
            tabSelectedIndicator.DOMoveY(targetPos.y, 0.7f)
                .SetEase(Ease.OutExpo);
        }
    }
}