using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Quiz
{
    public class IngredientSelector : IngredientSelectorUI
    {
        [SerializeField] private SecondIngredientSelector secondIngredientSelector;
        private int selectedIndex2;
        
        public override void Init(RecipeType recipeType, int ingredientNo, int selectedIndex = -1,
            int selectedIndex2 = -1)
        {
            this.recipeType = recipeType;
            this.ingredientNo = ingredientNo;
            this.selectedIndex = selectedIndex;
            this.selectedIndex2 = selectedIndex2;

            OnValueSelected = null;
            InitiateButtons();
            UpdateIconImages();
            secondIngredientSelector.gameObject.SetActive(false);
            OpenPanel(selectedIndex);
        }
        
        protected override void InitiateButtons()
        {
            for(int i = 0; i<iconImages.Count; i++)
            {
                var iconImage = iconImages[i];
                Button button = iconImage.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                var i1 = i;
                button.onClick.AddListener(()=>OnButtonClick(i1));
            }
        }

        public override void OnButtonClick(int index)
        {
            if (index == -1)
            {
                OnValueSelected?.Invoke(RecipeType.Ingredients, String.Empty, -1, -1, ingredientNo);
                ClosePanel();
                return;
            }
            selectedIndex = index;
            UpdateIconImages();
            secondIngredientSelector.InitAndOpen(QuizRecipeSelectManager.RecipeTypeHolderData.IngredientTypes[selectedIndex].Ingredients, selectedIndex2, Selection2Click);
        }
        
        private void Selection2Click(int index, string answer)
        {
            selectedIndex2 = index;
            OnValueSelected?.Invoke(recipeType, answer, selectedIndex, selectedIndex2, ingredientNo);
            ClosePanel();
        }
    }
}