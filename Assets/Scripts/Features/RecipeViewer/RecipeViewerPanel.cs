using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Cocktailor
{
    /// <summary>
    ///      manages the selection and display the cocktail recipes based on different modes.
    /// </summary>
    public class RecipeViewerPanel : MonoBehaviour
    {
        [Header("Managers and Controllers")] 
        [SerializeField] private RecipeListManager recipeListManager;
        [SerializeField] private SfxManager sfxManager;
        [SerializeField] private SettingsManager settingsPanel;

        [Header("QuizCard Components")] 
        [SerializeField] private Dropdown rangeSelectorDropdown;
        [SerializeField] private ScrollRect selectionScrollRect;
        
        private RecipeCardController currentRecipeCard;
        public bool isInQuizMode, isMenuVisible, isInHideTabMode;
        private List<int> recipeSelectionList = new(40);
        private int currentSelectionIndex, totalRecipeCount;

        private void Start()
        {
            totalRecipeCount = CocktailRecipeManger.GetTotalRecipeCount();
            for (var i = 0; i < totalRecipeCount; i++) recipeSelectionList.Add(i);

            currentSelectionIndex = 0;
            InitiateSelection();
            OpenCard(currentSelectionIndex, CardAnimationType.InFromLeft);
            MenuUIManager.Instance.ShowMenuInterface();

            settingsPanel.Start();
        }

        public void InitiateSelection()
        {
            var mode = (SelectionDropdownValue)rangeSelectorDropdown.value;

            switch (mode)
            {
                case SelectionDropdownValue.NotMemorized when PlayerData.NotMemorizedRecipes.Count == 0:
                    PopupMessageManager.Instance.ShowMsg("[못 외운 레시피]가 없습니다!");
                    mode = SelectionDropdownValue.All;
                    break;
                case SelectionDropdownValue.Memorized when PlayerData.MemorizedRecipes.Count == 0:
                    PopupMessageManager.Instance.ShowMsg("[다 외운 레시피]가 없습니다!");
                    mode = SelectionDropdownValue.All;
                    break;
            }

            switch (mode)
            {
                case SelectionDropdownValue.NotMemorized:
                    recipeSelectionList = GetRecipesSelectionFromState(MemorizedState.No);
                    break;
                case SelectionDropdownValue.Memorized:
                    recipeSelectionList = GetRecipesSelectionFromState(MemorizedState.Yes);
                    break;
                default:
                    recipeSelectionList = new List<int>(Enumerable.Range(0, totalRecipeCount));
                    break;
            }

            var maxIdx = recipeSelectionList.Count;
            recipeListManager.SetupSelections(recipeSelectionList, OpenCardFromSelection);
            selectionScrollRect.verticalNormalizedPosition = 1f - currentSelectionIndex / (float)maxIdx;
        }

        private List<int> GetRecipesSelectionFromState(MemorizedState state)
        {
            var selection = new List<int>();
            for (var i = 0; i < totalRecipeCount; i++)
                if (PlayerData.GetUserState(i) == state)
                    selection.Add(i);
            return selection;
        }

        private void OpenCardFromSelection(int selectionIndex)
        {
            currentSelectionIndex = selectionIndex;

            if (currentSelectionIndex <= GetStageIdxIndex(selectionIndex))
            {
                currentRecipeCard.panelAnimControl.PlayAnim(CardAnimationType.OutFromRight);
                OpenCard(selectionIndex, CardAnimationType.InFromLeft);
            }
            else if (currentSelectionIndex > GetStageIdxIndex(selectionIndex))
            {
                currentRecipeCard.panelAnimControl.PlayAnim(CardAnimationType.OutFromLeft);
                OpenCard(selectionIndex, CardAnimationType.InFromRight);
            }
        }

        private int GetStageIdxIndex(int num)
        {
            for (var i = 0; i < recipeSelectionList.Count; i++)
                if (num == recipeSelectionList[i])
                    return i;
            return -1;
        }

        private void HandleSwipeRight()
        {
            if (currentSelectionIndex < recipeSelectionList.Count - 1)
                currentSelectionIndex++;
            else
                currentSelectionIndex = 0;
            OpenCard(recipeSelectionList[currentSelectionIndex], CardAnimationType.InFromLeft);
        }

        private void HandleSwipeLeft()
        {
            if (currentSelectionIndex > 0)
                currentSelectionIndex--;
            else
                currentSelectionIndex = recipeSelectionList.Count - 1;
            OpenCard(recipeSelectionList[currentSelectionIndex], CardAnimationType.InFromRight);
        }

        private void OpenCard(int selectionIndex, CardAnimationType anim)
        {
            // gameObject.GetComponent<TutorialControl>().UpdateStatus(1);

            var cardInfo = new RecipeCardData(
                recipeSelectionList[currentSelectionIndex],
                selectionIndex,
                recipeSelectionList.Count,
                anim,
                !isInHideTabMode,
                OnCardSwipeEvent);

            currentRecipeCard = RecipeCardManager.Instance.OpenCard(cardInfo);
            InitiateSelection();
        }

        private void OnCardSwipeEvent(SwipeEventType swipeEventType)
        {
            switch (swipeEventType)
            {
                case SwipeEventType.SwipeLeft:
                    HandleSwipeLeft();
                    break;
                case SwipeEventType.SwipeRight:
                    HandleSwipeRight();
                    break;
            }
        }

        public void ToggleTabVisibility(bool show)
        {
            isInHideTabMode = !show;
            if (currentRecipeCard != null)
                currentRecipeCard.ShowAllTabs(show);
        }

        private enum SelectionDropdownValue
        {
            All = 0,
            NotMemorized = 1,
            Memorized = 2
        }
    }
}