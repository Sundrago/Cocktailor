using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Cocktailor
{
    public abstract class RecipeSelectorBase : MonoBehaviour
    {
        protected static Color defaultColor = new(1f, 1f, 1f, 1f);
        protected static Color unSelected = new(1f, 1f, 1f, 0.35f);

        private static readonly int Show = Animator.StringToHash("show");
        private static readonly int Hide = Animator.StringToHash("hide");
        [SerializeField] protected List<Image> iconImages;
        [SerializeField] private Animator panelAnimator;

        public Action<RecipeType, string, int, int, int> OnValueSelected;
        protected RecipeType recipeType;
        protected int selectedIndex, ingredientNo;

        public virtual void Init(RecipeType recipeType, int selectedIndex = -1, int currentSelected2 = -1,
            int ingredientNo = -1)
        {
            this.recipeType = recipeType;
            this.ingredientNo = ingredientNo;
            this.selectedIndex = selectedIndex;

            OnValueSelected = null;
            InitiateButtons();
        }

        protected virtual void UpdateIconImages()
        {
            if (selectedIndex == -1)
            {
                foreach (var iconImage in iconImages) iconImage.color = defaultColor;
                return;
            }

            foreach (var iconImage in iconImages)
                iconImage.color = iconImages.IndexOf(iconImage) == selectedIndex ? defaultColor : unSelected;
        }

        public virtual void OnButtonClick(int idx)
        {
            selectedIndex = idx;
            var answer = string.Empty;
            switch (recipeType)
            {
                case RecipeType.Glassware:
                    answer = CocktailRecipeIngredientManager.GetGlassware(idx);
                    break;
                case RecipeType.Garnish:
                    answer = CocktailRecipeIngredientManager.GetGarnish(idx);
                    break;
                case RecipeType.Method:
                    answer = CocktailRecipeIngredientManager.GetMethod(idx);
                    break;
            }

            OnValueSelected?.Invoke(recipeType, answer, selectedIndex, -1, ingredientNo);
            UpdateIconImages();
            ClosePanel();
        }

        [Button]
        protected virtual void InitiateButtons()
        {
            for (var i = 0; i < iconImages.Count; i++)
            {
                var iconImage = iconImages[i];
                var button = iconImage.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                var i1 = i;
                button.onClick.AddListener(() => OnButtonClick(i1));
                iconImage.color = defaultColor;
            }
        }

        public void OpenPanel(int selectedIndex = -1)
        {
            this.selectedIndex = selectedIndex;
            panelAnimator.gameObject.SetActive(true);
            panelAnimator.SetTrigger(Show);

            if (selectedIndex != -1)
                UpdateIconImages();
        }

        public void ClosePanel()
        {
            panelAnimator.SetTrigger(Hide);
        }

        public void SetActiveFalse()
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }
    }
}