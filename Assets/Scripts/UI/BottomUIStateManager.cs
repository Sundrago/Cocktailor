using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Cocktailor
{
    /// <summary>
    /// Manages the state and layout of the bottom UI.
    /// </summary>
    public class BottomUIStateManager : MonoBehaviour
    {
        public enum BottomUILayout
        {
            DefaultDisplayed = 0,
            QuizDisplayed = 1,
            QuizFinishedDisplayed = 2
        }

        [SerializeField] private List<GameObject> uiLayoutGroups;
        [SerializeField] private Text notMemorizedCountText, memorizedCountText;
        [SerializeField] private Button memorizedButton, notMemorizedButton, qLeftButton, qRightButton;
        [SerializeField] private Text quizLeftButtonText, quizRightButtonText;

        private void Start()
        {
            PlayerData.OnuserMemorizedStatehange += UpdateRecipeCountDisplay;
            QuizPanel.Instance.OnQuizCardIndexChange += UpdateQuizButtonText;

            UpdateRecipeCountDisplay(0, 0);
        }

        public void SwitchUILayout(BottomUILayout layoutToDisplay)
        {
            foreach (var uiLayoutGroup in uiLayoutGroups)
            {
                var isActive = uiLayoutGroups.IndexOf(uiLayoutGroup) == (int)layoutToDisplay;
                uiLayoutGroup.SetActive(isActive);
            }
        }

        private void UpdateRecipeCountDisplay(int recipeIdx, MemorizedState state)
        {
            notMemorizedCountText.text = PlayerData.NotMemorizedRecipes.Count.ToString();
            memorizedCountText.text = PlayerData.MemorizedRecipes.Count.ToString();
        }

        private void UpdateQuizButtonText(int currentQuizCardIndex)
        {
            quizLeftButtonText.text = currentQuizCardIndex == 0 ? "종료 하기" : "이전 문제";
            quizRightButtonText.text = currentQuizCardIndex == 2 ? "답안 제출" : "다음 문제";
        }


        public void OnMemorizedButtonClick(bool isOMarker)
        {
            AnimateButton(isOMarker ? memorizedButton.transform : notMemorizedButton.transform);

            if (RecipeCardManager.Instance.CurrentRecipeCard != null)
            {
                var currentRecipeIndex = RecipeCardManager.Instance.CurrentRecipeCard.CurrentRecipeIndex;
                PlayerData.UpdateUserState(currentRecipeIndex, isOMarker);
            }
        }

        private void AnimateButton(Transform button)
        {
            button.transform.localScale = new Vector3(1, 1, 1);
            button.transform.DOScale(1.2f, 0.2f);
            button.transform.DOScale(1f, 1f)
                .SetDelay(0.2f);
        }
    }
}