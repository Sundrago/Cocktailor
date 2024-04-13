using System;
using System.Linq;

namespace Features.Quiz
{
        public class UserAnswer
    {
        public string Glassware { get; private set; }
        public string Garnish { get;private set; }
        public string PreparationMethod { get; private set; }
        public string[] Ingredients { get; private set; } = Enumerable.Repeat((""), 7).ToArray();
        public string[] Amounts { get; private set; } = Enumerable.Repeat((""), 7).ToArray();

        public int GlasswareIndex = -1, GarnishIndex =-1, MethodIndex =-1;
        public (int, int)[] IngredientIndex { get; private set; } = Enumerable.Repeat((-1, -1), 7).ToArray();
        public (int, int)[] AmountIndex { get; private set; } = Enumerable.Repeat((-1, -1), 7).ToArray();
        
        
        public void UpdateAnswer(RecipeType recipeType, string selectedAnswer, int firstChoiceIndex, int secondChoiceIndex = -1, int ingredientIndex = -1)
        {
            switch (recipeType)
            {
                case RecipeType.Glassware:
                    Glassware = selectedAnswer;
                    GlasswareIndex = firstChoiceIndex;
                    break;
                case RecipeType.Garnish:
                    Garnish = selectedAnswer;
                    GarnishIndex = firstChoiceIndex;
                    break;
                case RecipeType.Method:
                    PreparationMethod = selectedAnswer;
                    MethodIndex = firstChoiceIndex;
                    break;
                case RecipeType.Ingredients:
                    Ingredients[ingredientIndex] = selectedAnswer;
                    IngredientIndex[ingredientIndex] = (firstChoiceIndex, secondChoiceIndex);
                    break;
                case RecipeType.Quantity:
                    Amounts[ingredientIndex] = selectedAnswer;
                    AmountIndex[ingredientIndex] = (firstChoiceIndex, secondChoiceIndex);
                    break;
            }
        }

        public void RemoveIngredientAt(int index)
        {
            int maxIndex = Ingredients.Length;
            for (int i = index; i < maxIndex - 1; i++)
            {
                Ingredients[i] = String.Copy(Ingredients[i + 1]);
                Amounts[i] = String.Copy(Amounts[i + 1]);
            }

            Ingredients[maxIndex - 1] = string.Empty;
            Amounts[maxIndex-1] = string.Empty;
        }
    }
}