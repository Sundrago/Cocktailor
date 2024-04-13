using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Quiz
{
    public static class QuizJudge
    {
        public static int GetQuizResult(QuizCardUIManager UIManager, UserAnswer userAnswer,
            CocktailRecipe currentRecipe, int ingredientCount)
        {
            int scoreDeduction = 0;
            scoreDeduction += JudgeGlassware(UIManager, userAnswer, currentRecipe);
            scoreDeduction += JudgeMethod(UIManager, userAnswer, currentRecipe);
            scoreDeduction += JudgeGarnish(UIManager, userAnswer, currentRecipe);
            scoreDeduction += JudgeIngredienetsQuantities(UIManager, userAnswer, currentRecipe, ingredientCount);

            int maxScore = GetMaxScore(currentRecipe);
            int finalScore = Mathf.RoundToInt((maxScore + scoreDeduction) / (float)maxScore * 100);
            return finalScore;
        }

        private static int GetMaxScore(CocktailRecipe currentRecipe)
        {
            return 3 + 3 + 2 + currentRecipe.Ingredient.Count;
        }

        private static int JudgeGlassware(QuizCardUIManager UIManager, UserAnswer userAnswer,
            CocktailRecipe CurrentRecipe)
        {
            bool isCorrect = userAnswer.Glassware == CurrentRecipe.Glassware;
            UIManager.SetGlasswareScore(isCorrect);
            return isCorrect ? 0 : -3;
        }

        private static int JudgeMethod(QuizCardUIManager UIManager, UserAnswer userAnswer, CocktailRecipe CurrentRecipe)
        {
            bool isCorrect = userAnswer.PreparationMethod == CurrentRecipe.PreparationMethod;
            UIManager.SetMethodScore(isCorrect);
            return isCorrect ? 0 : -3;
        }

        private static int JudgeGarnish(QuizCardUIManager UIManager, UserAnswer userAnswer,
            CocktailRecipe CurrentRecipe)
        {
            bool isCorrect = CurrentRecipe.Garnish.Contains(userAnswer.Garnish);
            UIManager.SetGarnishScore(isCorrect);
            return isCorrect ? 0 : -2;
        }

        private static int JudgeIngredienetsQuantities(QuizCardUIManager UIManager, UserAnswer userAnswer,
            CocktailRecipe currentRecipe, int ingredientCount)
        {
            int score = 0;
            List<QuizCardUIManager.IngredientAssessment> assessments =
                new List<QuizCardUIManager.IngredientAssessment>();
            List<int> correctIdxs = new List<int>();

            for (int i = 0; i < ingredientCount; i++)
            {
                assessments.Add(
                    GetIngredientAssessment(userAnswer.Ingredients[i], currentRecipe, userAnswer.Amounts[i]));
                if (assessments[i] == QuizCardUIManager.IngredientAssessment.Correct)
                {
                    correctIdxs.Add(i);
                }
                else
                {
                    score -= 1;
                }
            }

            int needIngredientsCount = currentRecipe.Ingredient.Count - correctIdxs.Count;
            score -= needIngredientsCount;
            UIManager.UpdateIngredientScore(assessments);
            UIManager.SetNeedMoreIngredientText(needIngredientsCount);

            return score;
        }

        private static QuizCardUIManager.IngredientAssessment GetIngredientAssessment(string userIngredient,
            CocktailRecipe currentRecipe, string userAnswerAmount)
        {
            int ingredientIndex = GetIngredientIndex(userIngredient, currentRecipe);
            if (ingredientIndex == -1)
            {
                return QuizCardUIManager.IngredientAssessment.BothIncorrect;
            }
            else
            {
                return IsAmountCorrect(ingredientIndex, userAnswerAmount, currentRecipe)
                    ? QuizCardUIManager.IngredientAssessment.Correct
                    : QuizCardUIManager.IngredientAssessment.IncorrectQuantity;
            }
        }

        private static int GetIngredientIndex(string userIngredient, CocktailRecipe currentRecipe)
        {
            for (int i = 0; i < currentRecipe.Ingredient.Count; i++)
            {
                if (currentRecipe.Ingredient[i].Contains(userIngredient))
                {
                    return i;
                }
            }

            return -1;
        }

        private static bool IsAmountCorrect(int ingredientIndex, string amountString, CocktailRecipe currentRecipe)
        {
            return currentRecipe.IngredientQuantities[ingredientIndex] == amountString;
        }
    }
}