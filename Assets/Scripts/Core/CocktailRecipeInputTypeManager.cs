using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Cocktailor
{
    /// <summary>
    /// Manages and validates input data types for cocktail recipes in a quiz tab.
    /// </summary>
    public static class CocktailRecipeInputTypeManager
    {
        static CocktailRecipeInputTypeManager()
        {
            DeSerializeIngredientTypes();
            ValidateRecipeData();
        }

        public static RecipeTypeHolder RecipeTypeHolderData { get; private set; }

        private static void DeSerializeIngredientTypes()
        {
            var jsonString = Resources.Load<TextAsset>("ingredientTypes").text;
            RecipeTypeHolderData = JsonConvert.DeserializeObject<RecipeTypeHolder>(jsonString);
        }

        private static void ValidateRecipeData()
        {
            for (var i = 0; i < CocktailRecipeManger.GetTotalRecipeCount(); i++)
            {
                var recipe = CocktailRecipeManger.GetCocktailRecipeByIndex(i);
                IsValueInList(recipe.Glassware, RecipeTypeHolderData.GlasswareTypes);
                IsValueInList(recipe.PreparationMethod, RecipeTypeHolderData.MethodTypes);
                foreach (var garnish in recipe.Garnish) IsValueInList(garnish, RecipeTypeHolderData.GarnishTypes);
                foreach (var ingredientString in recipe.Ingredient)
                    IsIngredientInList(ingredientString[0], RecipeTypeHolderData);
            }
        }

        private static void IsIngredientInList(string answerString, RecipeTypeHolder recipeTypeHolder)
        {
            for (var i = 0; i < recipeTypeHolder.IngredientTypes.Length; i++)
            {
                var ingredientType = recipeTypeHolder.IngredientTypes[i];
                for (var j = 0; j < ingredientType.Ingredients.Length; j++)
                    if (answerString == ingredientType.Ingredients[j])
                        return;
            }

            Debug.LogError($"{answerString} not in {recipeTypeHolder}");
        }

        private static void IsValueInList(string value, string[] array)
        {
            var list = array.ToList();

            if (!list.Contains(value)) Debug.LogError($"{value} not in {list}");
        }

        public static string GetGlassware(int i)
        {
            return RecipeTypeHolderData.GlasswareTypes[i];
        }

        public static string GetGarnish(int i)
        {
            return RecipeTypeHolderData.GarnishTypes[i];
        }

        public static string GetMethod(int i)
        {
            return RecipeTypeHolderData.MethodTypes[i];
        }

        public static string GetIngredient(int i, int j)
        {
            return RecipeTypeHolderData.IngredientTypes[i].Ingredients[j];
        }

        public static string GetAmount(int i, int j)
        {
            if (i == -1)
                return RecipeTypeHolderData.AmountDataTypes.UnitTypes[j];

            return RecipeTypeHolderData.AmountDataTypes.ValueTypes[i] +
                   RecipeTypeHolderData.AmountDataTypes.UnitTypes[j];
        }

        public class RecipeTypeHolder
        {
            public AmountDataTypes AmountDataTypes;
            public string[] GarnishTypes;
            public string[] GlasswareTypes;
            public IngredientType[] IngredientTypes;
            public string[] MethodTypes;
        }

        public class IngredientType
        {
            public string IngredientCategory;
            public string[] Ingredients;
        }

        public class AmountDataTypes
        {
            public string[] UnitTypes;
            public string[] ValueTypes;
        }
    }
}