using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Cocktailor
{
    public class CocktailRecipe
    {
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string Description { get; set; }
        public string Glassware { get; set; }
        public string PreparationMethod { get; set; }
        public List<string> Garnish { get; set; }
        public List<List<string>> Ingredient { get; set; }
        public List<string> IngredientQuantities { get; set; }
    }

    /// <summary>
    /// Manages and holds cocktail recipe data.
    /// </summary>
    public static class CocktailRecipeManger
    {
        private static Dictionary<int, CocktailRecipe> recipes;

        private static Dictionary<int, CocktailRecipe> Recipes
        {
            get
            {
                recipes ??= LoadCocktailRecipesFromJson();
                return recipes;
            }
        }

        private static Dictionary<int, CocktailRecipe> LoadCocktailRecipesFromJson()
        {
            try
            {
                var jsonString = LoadCocktailRecipeJson();
                return DeserializeCocktailRecipes(jsonString);
            }
            catch (JsonException jsonEx)
            {
                Debug.LogError("JSON Error: " + jsonEx.Message);
            }
            catch (Exception ex)
            {
                Debug.LogError("An error occurred: " + ex.Message);
            }

            return new Dictionary<int, CocktailRecipe>();
        }

        private static string LoadCocktailRecipeJson()
        {
            var cocktailRecipeJson = Resources.Load<TextAsset>("cocktailRecipe");
            if (cocktailRecipeJson == null)
            {
                Debug.LogError("CocktailRecipe JSON file not found in Resources.");
                return "";
            }

            return cocktailRecipeJson.text;
        }

        private static Dictionary<int, CocktailRecipe> DeserializeCocktailRecipes(string jsonString)
        {
            var recipes = JsonConvert.DeserializeObject<Dictionary<int, CocktailRecipe>>(jsonString);
            return recipes ?? new Dictionary<int, CocktailRecipe>();
        }


        // Returns the cocktail recipe by its index.
        public static CocktailRecipe GetCocktailRecipeByIndex(int index)
        {
            if (!Recipes.ContainsKey(index))
                throw new KeyNotFoundException($"Key {index} is not found in the recipes dictionary.");
            return Recipes[index];
        }

        // Returns the total count of recipe data in the CocktailRecipeManger.
        public static int GetTotalRecipeCount()
        {
            return Recipes.Count;
        }
    }
}