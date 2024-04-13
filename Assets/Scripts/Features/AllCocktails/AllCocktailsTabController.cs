using UnityEngine;

namespace Cocktailor
{
    public class AllCocktailsTabController : MonoBehaviour
    {
        // [SerializeField] public Transform panel_holder;
        // [SerializeField] private RecipeCardController cardPrefab;

        public void OpenCard(int recipeIndex)
        {
            // int currentIdx = recipeIndex;
            // RecipeCardController newPanel = Instantiate(cardPrefab, panel_holder);
            //
            // newPanel.UpdateCocktailInfo(recipeIndex, 0,true, true);
            // newPanel.ShowAllCards(true, true);
            // newPanel.GetComponent<panelAnimControl>().PlayAnim(CardAnimationType.InFromLeft);
            // newPanel.GetComponent<DragEventManager>().isInSearchMode = true;

            var cardInfo = new CardInfo(
                recipeIndex,
                animationType: CardAnimationType.InFromLeft,
                onCardSwipe: OnCardClose
            );
            RecipeCardManager.Instance.OpenCard(cardInfo);
        }

        private void OnCardClose(SwipeEventType swipeEventType)
        {
            MenuUIManager.Instance.ShowMenuInterface();
        }
    }
}