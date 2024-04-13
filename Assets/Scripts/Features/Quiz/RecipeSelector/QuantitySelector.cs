using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Quiz
{
    public class QuantitySelector : IngredientSelectorUI
    {
        [SerializeField] private List<Image> iconImages2;
        [SerializeField] private Button confrimButton;
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
            OpenPanel(selectedIndex);
        }

        protected override void InitiateButtons()
        {
            InitButtonStatusAndImages(iconImages, OnAmountButtonClick);
            InitButtonStatusAndImages(iconImages2, OnUnitButtonClick);
            confrimButton.interactable = (selectedIndex2 != -1);
        }

        private void InitButtonStatusAndImages(List<Image> iconImages, Action<int> buttonClick)
        {
            for (int i = 0; i < iconImages.Count; i++)
            {
                var iconImage = iconImages[i];
                Button button = iconImage.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                var i1 = i;
                button.onClick.AddListener(() => buttonClick(i1));
            }

            confrimButton.interactable = false;
        }

        private void OnAmountButtonClick(int index)
        {
            if (index == selectedIndex)
                index = -1;
            selectedIndex = index;
            UpdateIconImages();
        }

        private void OnUnitButtonClick(int index)
        {
            selectedIndex2 = index;
            UpdateIconImages();
            
            if (index == 5)
            {
                foreach (Image iconImage in iconImages)
                {
                    iconImage.color = unSelected;
                }
                selectedIndex = -1;
            }

            if (IsConfirmButtonInteractable()) ConfirmButtonClicked();
        }

        protected override void UpdateIconImages()
        {
            confrimButton.interactable = IsConfirmButtonInteractable();
            base.UpdateIconImages();
            if (selectedIndex2 == -1)
            {
                foreach (Image iconImage in iconImages2)
                {
                    iconImage.color = defaultColor;
                }
                return;
            }
            
            foreach (Image iconImage in iconImages2)
            {
                iconImage.color = (iconImages2.IndexOf(iconImage) == selectedIndex2) ? defaultColor : unSelected;
            }
        }

        private bool IsConfirmButtonInteractable()
        {
            if (selectedIndex2 == 5) return true;
            else return (selectedIndex != -1 && selectedIndex2 != -1);
        }

        public void ConfirmButtonClicked()
        {
            string output = QuizRecipeSelectManager.GetAmount(selectedIndex, selectedIndex2);
            OnValueSelected?.Invoke(RecipeType.Quantity, output, selectedIndex, selectedIndex2, ingredientNo);
            ClosePanel();
        }
    }
}