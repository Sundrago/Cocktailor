using UI;
using UnityEngine;

namespace System
{
    public class RecipeCardManager : MonoBehaviour
    {
        public static RecipeCardManager Instance { get; private set; }

        [SerializeField] private RecipeCardController recipeCardObjectPrefab;
        [SerializeField] private Transform panel_holder;

        public RecipeCardController CurrentRecipeCard { get; private set; }
        // private Action<int, bool> OnOXMarkButtonClicked;

        private void Awake()
        {
            Instance = this;
        }

        public RecipeCardController OpenCard(CardInfo cardInfo)
        {
            CloseExistingCardIfAny();
            CreateAndSetupNewRecipeCard(cardInfo);
            MenuUIManager.Instance.HideMenuInterface();
            return CurrentRecipeCard;
        }

        private void CreateAndSetupNewRecipeCard(CardInfo cardInfo)
        {
            CurrentRecipeCard = Instantiate(recipeCardObjectPrefab, panel_holder);
            CurrentRecipeCard.UpdateCocktailInfo(cardInfo);
            CurrentRecipeCard.ShowAllTabs(cardInfo.IsMemoryTabsAllOpen, true);
            CurrentRecipeCard.panelAnimControl.PlayAnim(cardInfo.AnimationType);
            CurrentRecipeCard.DragEventManager.OnSwipeEvent += cardInfo.OnCardSwipeEvent;
        }

        private void CloseExistingCardIfAny()
        {
            if (CurrentRecipeCard == null || CurrentRecipeCard.panelAnimControl.IsBeingScrolled) return;
            CurrentRecipeCard.panelAnimControl.PlayAnim(CardAnimationType.OutFromRight);
        }
    }

    public class CardInfo
    {
        public int RecipeIndex { get; private set; }
        public int CardIndex { get; private set; }
        public int CardMaxIndex { get; private set; }
        public CardAnimationType AnimationType { get; }
        public bool IsMemoryTabsAllOpen { get; }
        public Action<SwipeEventType> OnCardSwipeEvent { get; }

        public CardInfo(
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
    }
}