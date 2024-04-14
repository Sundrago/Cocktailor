using UnityEngine;
using UnityEngine.UI;

namespace Cocktailor
{
    public enum RecipeButtonState
    {
        DeActive,
        PressToAdd,
        Active
    }

    /// <summary>
    /// Handles the behavior of a quiz card UI button.
    /// </summary>
    public class QuizCardUIButtonController : MonoBehaviour
    {
        [SerializeField] private Text addButtonText;
        [SerializeField] private Text ingredientNameText;
        [SerializeField] private Text quantityText;
        [SerializeField] private Image iconImage;
        [SerializeField] private Animator animator;

        private static readonly int Show = Animator.StringToHash("show");

        public void SetButtonState(RecipeButtonState state)
        {
            gameObject.SetActive(state != RecipeButtonState.DeActive);
            addButtonText.gameObject.SetActive(state == RecipeButtonState.PressToAdd);
            ingredientNameText.gameObject.SetActive(state == RecipeButtonState.Active);
            quantityText.gameObject.SetActive(state == RecipeButtonState.Active);
        }

        public void SetButtonActive()
        {
            animator.SetTrigger(Show);
            ingredientNameText.text = "재료 선택";
            quantityText.text = "oz";
            SetButtonState(RecipeButtonState.Active);
        }

        public void SetIngredientNameText(string text)
        {
            ingredientNameText.text = text;
        }

        public void SetQuantityText(string text)
        {
            quantityText.text = text;
        }

        public void SetIngredientAndQuantity(string ingredient, string quantity)
        {
            ingredientNameText.text = string.IsNullOrEmpty(ingredient) ? "재료 선택" : ingredient;
            quantityText.text = string.IsNullOrEmpty(quantity) ? "oz" : quantity;
        }

        public void SetIconImage(Sprite sprite)
        {
            if (iconImage.sprite != sprite)
                iconImage.sprite = sprite;
        }
    }
}