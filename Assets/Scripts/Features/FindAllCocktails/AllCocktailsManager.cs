using UnityEngine;

namespace Cocktailor
{
    /// <summary>
    /// Manages the functionality of the All Cocktails tab in the main QuizCard.
    /// </summary>
    public class AllCocktailsManager : MonoBehaviour
    {
        public void OpenCard(int recipeIndex)
        {
            var cardInfo = new RecipeCardData(
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