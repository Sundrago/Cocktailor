using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Cocktailor
{
    public class SearchCocktails : MonoBehaviour
    {
        [SerializeField] public RectTransform contents;
        [SerializeField] public GameObject cocktail;
        [SerializeField] public Text inputField;
        [SerializeField] public GameObject myPanel, panel_holder;

        [SerializeField] private RecipeListManager recipeListManager;

        private string previousText;
        // [SerializeField] private List<Selection> selections = new();
        // [SerializeField] private Transform selectionHolder;

        private List<int> recipeFound = new();
        private int totalRecipeCount;

        private void Start()
        {
            totalRecipeCount = 40; //CocktailRecipeManger.GetTotalRecipeCount();
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
        //
        // private void SetupSelections()
        // {
        //     if (recipeFound.Count == 0)
        //     {
        //         selectionHolder.gameObject.SetActive(false);
        //         return;
        //     }
        //
        //     selectionHolder.gameObject.SetActive(true);
        //
        //     for (var i = 0; i < recipeFound.Count; i++) selections[i].Init(recipeFound[i], i + 1, OpenCard);
        //     for (var i = recipeFound.Count; i < selections.Count; i++) selections[i].gameObject.SetActive(false);
        //
        //     UpdateSizeDelta();
        //     previousText = inputField.text;
        // }
        //
        // private void UpdateSizeDelta()
        // {
        //     var lastSelectionRect = selections[recipeFound.Count - 1].GetComponent<RectTransform>();
        //     contents.sizeDelta = new Vector2(contents.sizeDelta.x, lastSelectionRect.anchoredPosition.y * -1 + 550);
        // }

        private void OpenCard(int recipeIndex)
        {
            var cardInfo = new CardInfo(
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