using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Cocktailor
{
    public class SearchManager : MonoBehaviour
    {
        [SerializeField] public RectTransform contents;
        [SerializeField] public GameObject cocktail;
        [SerializeField] public Text inputField;
        [SerializeField] public GameObject myPanel, panel_holder;
        [SerializeField] private RecipeListManager recipeListManager;

        private string previousText;

        private List<int> recipeFound = new();
        private int totalRecipeCount;

        private void Start()
        {
            totalRecipeCount = 40;
            SearchUpdateFoundList();
        }

        private void Update()
        {
            if (Time.frameCount % 30 == 0)
            {
                if (!HasInputFieldTextChanged()) return;
                SearchUpdateFoundList();
            }
        }

        private bool HasInputFieldTextChanged()
        {
            if (previousText == inputField.text)
                return false;
            return true;
        }

        public void SearchUpdateFoundList()
        {
            var key = inputField.text;
            recipeFound = new List<int>();

            void AppendRecipesMatchingKey(Func<CocktailRecipe, object> getField)
            {
                for (var i = 0; i < totalRecipeCount; i++)
                {
                    var obj = getField(CocktailRecipeManger.GetCocktailRecipeByIndex(i));

                    if (obj is string field && field.Contains(key))
                    {
                        if (!recipeFound.Contains(i)) recipeFound.Add(i);
                    }
                    else if (obj is List<string> list)
                    {
                        if (list.Any(item => item.Contains(key)) && !recipeFound.Contains(i)) recipeFound.Add(i);
                    }
                }
            }

            AppendRecipesMatchingKey(recipe => recipe.Name);
            AppendRecipesMatchingKey(recipe => recipe.Glassware);
            AppendRecipesMatchingKey(recipe => recipe.Ingredient);
            AppendRecipesMatchingKey(recipe => recipe.PreparationMethod);
            AppendRecipesMatchingKey(recipe => recipe.Garnish);

            recipeListManager.SetupSelections(recipeFound, OpenCard);
            previousText = inputField.text;
        }

        private void OpenCard(int recipeIndex)
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