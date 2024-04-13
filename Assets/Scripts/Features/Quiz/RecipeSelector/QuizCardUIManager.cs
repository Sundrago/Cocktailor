using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Cocktailor
{
    public class QuizCardUIManager : MonoBehaviour
    {
        public enum IngredientAssessment
        {
            Idle,
            Correct,
            IncorrectIngredient,
            IncorrectQuantity,
            BothIncorrect
        }

        [SerializeField]
        private Text glasswareText, methodText, garnishText, nameText, descriptionText, indexText, scoreText;

        [SerializeField] private Image scoreImage, iconImage, glasswareImage, methodImage, garnishImage;
        [SerializeField] private IngredientButtonUI[] ingredientButtons;

        [FormerlySerializedAs("ingredientIconImages")] [SerializeField]
        private Sprite[] ingredientTabImages;

        [SerializeField] private Sprite[] glasswareTabImages;
        [SerializeField] private Sprite[] garnishTabImages, methodTabImages;
        [SerializeField] private MemoryCardTabController iconTab;
        [SerializeField] private Text needMoreIngredientText;

        public void InitUI(int cocktailIndex, int quizIndex)
        {
            var recipe = CocktailRecipeManger.GetCocktailRecipeByIndex(cocktailIndex);
            nameText.text = recipe.EnglishName;
            descriptionText.text = recipe.Description;
            scoreText.gameObject.SetActive(false);
            scoreImage.gameObject.SetActive(false);
            needMoreIngredientText.gameObject.SetActive(false);
            iconTab.UpdateCardVisibility(false, true);
            iconImage.sprite = CocktailIconManager.Instance.GetIconSpriteAtIndexOf(cocktailIndex);
            glasswareText.text = "글라스 선택";
            garnishText.text = "가니시 선택";
            methodText.text = "기법 선택";
            indexText.text = "Q " + (quizIndex + 1) + "/3";
        }

        public void UpdateIngredientButton(UserAnswer userAnswer, int ingredientCount)
        {
            for (var i = 0; i < ingredientButtons.Length; i++)
                if (i == ingredientCount)
                {
                    ingredientButtons[i].SetButtonState(RcpAmtButtonState.PressToAdd);
                    // ingredientButtons[i].SetIconImage(ingredientTabImages[0]);
                }
                else if (i < ingredientCount)
                {
                    ingredientButtons[i].SetButtonState(RcpAmtButtonState.Active);
                    ingredientButtons[i].SetIngredientAndQuantity(userAnswer.Ingredients[i], userAnswer.Amounts[i]);
                    // ingredientButtons[i].SetIconImage(ingredientTabImages[1]);
                }
                else
                {
                    ingredientButtons[i].SetButtonState(RcpAmtButtonState.DeActive);
                }
        }

        public void UpdateAnswerUI(RecipeType recipeType, string selectedAnswer, int ingredientIndex)
        {
            switch (recipeType)
            {
                case RecipeType.Glassware:
                    glasswareText.text = selectedAnswer;
                    break;
                case RecipeType.Garnish:
                    garnishText.text = selectedAnswer;
                    break;
                case RecipeType.Method:
                    methodText.text = selectedAnswer;
                    break;
                case RecipeType.Ingredients:
                    ingredientButtons[ingredientIndex].SetIngredientNameText(selectedAnswer);
                    break;
                case RecipeType.Quantity:
                    ingredientButtons[ingredientIndex].SetQuantityText(selectedAnswer);
                    break;
            }
        }

        public void UpdateIngredientScore(List<IngredientAssessment> ingredientAssessments)
        {
            for (var i = 0; i < ingredientAssessments.Count; i++)
                ingredientButtons[i].SetIconImage(ingredientTabImages[(int)ingredientAssessments[i]]);
        }

        public void SetGlasswareScore(bool isCorrect)
        {
            glasswareImage.sprite = glasswareTabImages[isCorrect ? 0 : 1];
        }

        public void SetMethodScore(bool isCorrect)
        {
            methodImage.sprite = methodTabImages[isCorrect ? 0 : 1];
        }

        public void SetGarnishScore(bool isCorrect)
        {
            garnishImage.sprite = garnishTabImages[isCorrect ? 0 : 1];
        }

        public void SetIngredientButtonActive(int index)
        {
            ingredientButtons[index].SetButtonActive();
        }

        public void SetNeedMoreIngredientText(int amount)
        {
            if (amount == 0)
            {
                needMoreIngredientText.gameObject.SetActive(false);
                return;
            }

            needMoreIngredientText.text = amount + "가지 재료 부족!";
            needMoreIngredientText.gameObject.SetActive(true);
        }

        public void SetScoreUI(int score)
        {
            scoreText.text = score.ToString();
            scoreImage.gameObject.SetActive(true);
            scoreText.gameObject.SetActive(true);
        }
    }
}