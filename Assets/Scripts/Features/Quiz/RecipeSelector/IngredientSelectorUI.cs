using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace Features.Quiz
{
    public abstract class IngredientSelectorUI : MonoBehaviour
    {
        [SerializeField] protected List<Image> iconImages;
        [SerializeField] private Animator panelAnimator;
        
        public Action< RecipeType, string, int, int, int> OnValueSelected;
        protected int selectedIndex, ingredientNo;
        protected RecipeType recipeType;
        protected static Color defaultColor = new Color(1f, 1f, 1f, 1f);
        protected static Color unSelected = new Color(1f, 1f, 1f, 0.35f);
        
        private static readonly int Show = Animator.StringToHash("show");
        private static readonly int Hide = Animator.StringToHash("hide");

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
                foreach (Image iconImage in iconImages)
                {
                    iconImage.color = defaultColor;
                }
                return;
            }
            
            foreach (Image iconImage in iconImages)
            {
                iconImage.color = (iconImages.IndexOf(iconImage) == selectedIndex) ? defaultColor : unSelected;
            }
        }

        public virtual void OnButtonClick(int idx)
        {
            selectedIndex = idx;
            string answer = string.Empty;
            switch (recipeType)
            {
                case RecipeType.Glassware:
                    answer = QuizRecipeSelectManager.GetGlassware(idx);
                    break;
                case RecipeType.Garnish:
                    answer = QuizRecipeSelectManager.GetGarnish(idx);
                    break;
                case RecipeType.Method:
                    answer = QuizRecipeSelectManager.GetMethod(idx);
                    break;
            }
            OnValueSelected?.Invoke(recipeType, answer, selectedIndex, -1, ingredientNo);
            UpdateIconImages();
            ClosePanel();
        }
        
        [Button]
        protected virtual void InitiateButtons()
        {
            for(int i = 0; i<iconImages.Count; i++)
            {
                var iconImage = iconImages[i];
                Button button = iconImage.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                var i1 = i;
                button.onClick.AddListener(()=>OnButtonClick(i1));
                iconImage.color = defaultColor;
            }
        }

        public void OpenPanel(int selectedIndex = -1)
        {
            this.selectedIndex = selectedIndex;
            panelAnimator.gameObject.SetActive(true);
            panelAnimator.SetTrigger(Show);
            
            if(selectedIndex!=-1)
                UpdateIconImages();
        }

        public void ClosePanel()
        {
            panelAnimator.SetTrigger(Hide);
        }

        public void SetActiveFalse()
        {
            if(gameObject.activeSelf)
                gameObject.SetActive(false);
        }
    }
}