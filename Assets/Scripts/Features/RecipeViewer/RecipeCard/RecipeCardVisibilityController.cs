using UnityEngine;
using UnityEngine.UI;

namespace Cocktailor
{
    /// <summary>
    /// Controls the visibility and behavior of a recipe card in the user interface.
    /// </summary>
    public class RecipeCardVisibilityController : MonoBehaviour
    {
        [SerializeField] private Animator cardOpenAnimator;
        [SerializeField] private Animator cardCloseAnimator;
        [SerializeField] private Text cardDescriptionText, cardDescriptionText2;
        [SerializeField] private RectTransform scaler;

        private bool isTabOpen = true;

        public void ToggleCardVisibility()
        {
            UpdateCardVisibility(!isTabOpen);
        }

        public void UpdateCardVisibility(bool isVisible, bool skipAnimation = false)
        {
            isTabOpen = isVisible;

            if (skipAnimation)
            {
                cardOpenAnimator.SetTrigger(isVisible ? "idle" : "hidden");
                cardCloseAnimator.SetTrigger(isVisible ? "hidden" : "idle");
                return;
            }

            cardOpenAnimator.SetTrigger(isVisible ? "show" : "hide");
            cardCloseAnimator.SetTrigger(isVisible ? "hide" : "show");
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
}