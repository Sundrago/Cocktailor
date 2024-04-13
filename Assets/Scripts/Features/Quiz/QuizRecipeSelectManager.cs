using System;
using System.Collections.Generic;
using System.Linq;
using Cocktailor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Features.Quiz
{
    public static class QuizRecipeSelectManager
    {
        public static RecipeTypeHolder RecipeTypeHolderData { get; private set; }

        static QuizRecipeSelectManager()
        {
            DeSerializeIngredientTypes();
            ValidateRecipeData();
        }

        private static void DeSerializeIngredientTypes()
        {
            string jsonString = Resources.Load<TextAsset>("ingredientTypes").text;
            RecipeTypeHolderData = Newtonsoft.Json.JsonConvert.DeserializeObject<RecipeTypeHolder>(jsonString);
        }
        
        private static void ValidateRecipeData()
        {
            for (int i = 0; i < CocktailRecipeManger.GetTotalRecipeCount(); i++)
            {
                var recipe = CocktailRecipeManger.GetCocktailRecipeByIndex(i);
                IsValueInList(recipe.Glassware, RecipeTypeHolderData.GlasswareTypes);
                IsValueInList(recipe.PreparationMethod, RecipeTypeHolderData.MethodTypes);
                foreach (var garnish in recipe.Garnish)
                {
                    IsValueInList(garnish, RecipeTypeHolderData.GarnishTypes);
                }
                foreach (var ingredientString in recipe.Ingredient)
                {
                    IsIngredientInList(ingredientString[0], RecipeTypeHolderData);
                }
            }
        }
        
        private static void IsIngredientInList(string answerString, RecipeTypeHolder recipeTypeHolder)
        {
            for (int i = 0; i < recipeTypeHolder.IngredientTypes.Length; i++)
            {
                var ingredientType = recipeTypeHolder.IngredientTypes[i];
                for (int j = 0; j < ingredientType.Ingredients.Length; j++)
                {
                    if (answerString == ingredientType.Ingredients[j])
                    {
                        return;
                    }
                }
            }

            Debug.LogError($"{answerString} not in {recipeTypeHolder}");
        }
        
        private static void IsValueInList(string value, string[] array)
        {
            List<string> list = array.ToList();
            
            if (!list.Contains(value))
            {
                Debug.LogError($"{value} not in {list}");
            }
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
            if(i==-1) 
                return RecipeTypeHolderData.AmountDataTypes.UnitTypes[j];
            
            return RecipeTypeHolderData.AmountDataTypes.ValueTypes[i] +
                   RecipeTypeHolderData.AmountDataTypes.UnitTypes[j];
        }
        
        public class RecipeTypeHolder
        {
            public string[] GlasswareTypes;
            public string[] GarnishTypes;
            public string[] MethodTypes;
            public IngredientType[] IngredientTypes;
            public AmountDataTypes AmountDataTypes;
        }
        
        public class IngredientType
        {
            public string IngredientCategory;
            public string[] Ingredients;
        }
        
        public class AmountDataTypes
        {
            public string[] ValueTypes;
            public string[] UnitTypes;
        }
    }
}