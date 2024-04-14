using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cocktailor
{
    public class RecipeSelector : RecipeSelectorBase
    {
        [SerializeField] private Text titleText;

        public void InitSelector(RecipeType recipeType, int selectedIndex = -1, int currentSelected2 = -1,
            int ingredientNo = -1)
        {
            base.Init(recipeType, selectedIndex, currentSelected2, ingredientNo);
            titleText.text = recipeType.ToString(); // Set title based on recipe type
        }

        public void SetIconSprites(List<Sprite> sprites)
        {
            if (sprites.Count != iconImages.Count)
            {
                return;
            }

            for (int i = 0; i < iconImages.Count; i++)
            {
                iconImages[i].sprite = sprites[i];
            }
        }
    }
}