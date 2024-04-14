using System;
using UnityEngine;

namespace Cocktailor
{
    public class RecipeCardData
    {
        public RecipeCardData(
            int recipeIndex,
            int cardIndex = 1,
            int cardMaxIndex = 1,
            CardAnimationType animationType = CardAnimationType.InFromLeft,
            bool isMemoryTabsAllOpen = true,
            Action<SwipeEventType> onCardSwipe = null)
        {
            RecipeIndex = recipeIndex;
            CardIndex = cardIndex;
            CardMaxIndex = cardMaxIndex;
            AnimationType = animationType;
            IsMemoryTabsAllOpen = isMemoryTabsAllOpen;
            OnCardSwipeEvent = onCardSwipe;
        }

        public int RecipeIndex { get; private set; }
        public int CardIndex { get; private set; }
        public int CardMaxIndex { get; private set; }
        public CardAnimationType AnimationType { get; }
        public bool IsMemoryTabsAllOpen { get; }
        public Action<SwipeEventType> OnCardSwipeEvent { get; }
    }
    
    public class RecipeCardManager : MonoBehaviour
    {
        [SerializeField] private RecipeCardController recipeCardObjectPrefab;
        [SerializeField] private Transform panel_holder;
        public static RecipeCardManager Instance { get; private set; }
        public RecipeCardController CurrentRecipeCard { get; private set; }
        // private Action<int, bool> OnOXMarkButtonClicked;

        private void Awake()
        {
            Instance = this;
        }

        public RecipeCardController OpenCard(RecipeCardData recipeCardData)
        {
            CloseExistingCardIfAny();
            CreateAndSetupNewRecipeCard(recipeCardData);
            MenuUIManager.Instance.HideMenuInterface();
            return CurrentRecipeCard;
        }

        private void CreateAndSetupNewRecipeCard(RecipeCardData recipeCardData)
        {
            CurrentRecipeCard = Instantiate(recipeCardObjectPrefab, panel_holder);
            CurrentRecipeCard.UpdateCocktailInfo(recipeCardData);
            CurrentRecipeCard.ShowAllTabs(recipeCardData.IsMemoryTabsAllOpen, true);
            CurrentRecipeCard.panelAnimControl.PlayAnim(recipeCardData.AnimationType);
            CurrentRecipeCard.RecipeCardSwipeHandler.OnSwipeEvent += recipeCardData.OnCardSwipeEvent;
        }

        private void CloseExistingCardIfAny()
        {
            if (CurrentRecipeCard == null || CurrentRecipeCard.panelAnimControl.IsBeingScrolled) return;
            CurrentRecipeCard.panelAnimControl.PlayAnim(CardAnimationType.OutFromRight);
        }
    }
}