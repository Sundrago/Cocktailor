using UnityEngine;

namespace Cocktailor
{
    /// <summary>
    /// Manages the functionality of the All Cocktails tab in the main QuizCard.
    /// </summary>
    public class ViewAllCocktailsPanel : MonoBehaviour
    {
        public void OpenCard(int recipeIndex)
        {
            var cardInfo = new RecipeCardData(
                recipeIndex,
                animationType: CardAnimationType.InFromLeft,
                onCardSwipe: CardClosed
            );
            RecipeCardManager.Instance.OpenCard(cardInfo);
        }

        private void CardClosed(SwipeEventType swipeEventType)
        {
            MenuUIManager.Instance.ShowMenuInterface();
        }
    }
}