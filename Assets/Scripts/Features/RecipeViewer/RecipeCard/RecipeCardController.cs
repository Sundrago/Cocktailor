using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Cocktailor
{
    [RequireComponent(typeof(RecipeCardSwipeHandler))]
    public class RecipeCardController : MonoBehaviour
    {
        [Header("Managers and Controllers")] [SerializeField]
        private SfxManager sfxManager;

        [SerializeField] private CocktailIconManager iconManager;
        [FormerlySerializedAs("recipeMarkerController")] [SerializeField] private RecipeCardMarkerController recipeCardMarkerController;
        [SerializeField] public PanelAnimControl panelAnimControl;

        [Header("QuizCard Components")] [SerializeField]
        private Image myIcon;

        [SerializeField] private RectTransform recipePanelScaler;
        [SerializeField] private Text nameText, descriptionText, indexText;
        [FormerlySerializedAs("iconTab")] [SerializeField] private RecipeCardVisibilityController iconVisibility;
        [FormerlySerializedAs("glasswareTab")] [SerializeField] private RecipeCardVisibilityController glasswareVisibility;
        [FormerlySerializedAs("methodTab")] [SerializeField] private RecipeCardVisibilityController methodVisibility;
        [FormerlySerializedAs("garnishTab")] [SerializeField] private RecipeCardVisibilityController garnishVisibility;
        [FormerlySerializedAs("ingredientTab")] [SerializeField] private RecipeCardVisibilityController ingredientVisibility;
        [SerializeField] private GameObject closeButton, transparentBackground;
        [SerializeField] private TMP_InputField noteInputField;
        [SerializeField] private Image noteButton;
        [SerializeField] private Transform initialPosition, basePosition;
        public RecipeCardSwipeHandler RecipeCardSwipeHandler { get; private set; }

        public int CurrentRecipeIndex { get; private set; }

        private void Awake()
        {
            RecipeCardSwipeHandler = GetComponent<RecipeCardSwipeHandler>();
            PlayerData.OnuserMemorizedStatehange += WiggleCard;
        }

        private void OnDestroy()
        {
            PlayerData.OnuserMemorizedStatehange -= WiggleCard;
        }

        public void UpdateCocktailInfo(RecipeCardData cardInfo)
        {
            CurrentRecipeIndex = cardInfo.RecipeIndex;

            var cardIndex = cardInfo.CardIndex;
            var maxRecipeCount = cardInfo.CardMaxIndex;
            var recipe = CocktailRecipeManger.GetCocktailRecipeByIndex(CurrentRecipeIndex);

            UpdateCardUISize(recipe);
            UpdateCardUIElements(recipe, cardInfo);
            UpdateCardButtonElements(maxRecipeCount);
        }

        private void UpdateCardUISize(CocktailRecipe recipe)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 30, 0);
            SetIngredientUIHeight(GetIngredientUIHeightByAmount(recipe.Ingredient.Count));
            myIcon.sprite = iconManager.GetIconSpriteAtIndexOf(CurrentRecipeIndex);
        }

        private void UpdateCardButtonElements(int maxRecipeCount)
        {
            var alpha = string.IsNullOrEmpty(PlayerData.GetNote(CurrentRecipeIndex)) ? 0.3f : 0.8f;
            noteButton.color = SetColorAlpha(noteButton.color, alpha);
            closeButton.SetActive(maxRecipeCount == 1);
            indexText.gameObject.SetActive(maxRecipeCount != 1);
            recipeCardMarkerController.InitializeMarker(CurrentRecipeIndex);
        }

        private void UpdateCardUIElements(CocktailRecipe recipe, RecipeCardData cardInfo)
        {
            indexText.text = cardInfo.CardIndex + 1 + " / " + cardInfo.CardMaxIndex;
            RecipeCardSwipeHandler.InitDefaultPosition();
            nameText.text = recipe.Name;
            descriptionText.text = recipe.Description;
            glasswareVisibility.SetCardDescriptionText(recipe.Glassware);
            methodVisibility.SetCardDescriptionText(recipe.PreparationMethod);
            garnishVisibility.SetCardDescriptionText(ParseListToString(recipe.Garnish));

            var ingredientsString = "";
            var quantitiesSting = "";
            for (var j = 0; j < recipe.Ingredient.Count; j++)
            {
                ingredientsString += ParseListToString(recipe.Ingredient[j]);
                quantitiesSting += recipe.IngredientQuantities[j];
                if (j < recipe.Ingredient.Count - 1)
                {
                    ingredientsString += "\n";
                    quantitiesSting += "\n";
                }
            }

            ingredientVisibility.SetCardDescriptionText(ingredientsString, quantitiesSting);
        }

        private string ParseListToString(List<string> garnishes)
        {
            if (garnishes == null || garnishes.Count == 0) return string.Empty;

            return garnishes.Count == 1 ? garnishes[0] : $"{garnishes[0]} or {garnishes[1]}";
        }

        private float GetIngredientUIHeightByAmount(int count)
        {
            return count switch
            {
                <= 2 => 145,
                <= 3 => 180,
                <= 4 => 200,
                _ => 250
            };
        }

        private void SetIngredientUIHeight(float y)
        {
            recipePanelScaler.anchoredPosition = new Vector2(0, -y / 2f);
            recipePanelScaler.sizeDelta = new Vector2(recipePanelScaler.sizeDelta.x, y);
        }

        private Color SetColorAlpha(Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        public void ShowAllTabs(bool shouldSHow, bool skipAnimation = false)
        {
            iconVisibility.UpdateCardVisibility(shouldSHow, skipAnimation);
            glasswareVisibility.UpdateCardVisibility(shouldSHow, skipAnimation);
            methodVisibility.UpdateCardVisibility(shouldSHow, skipAnimation);
            garnishVisibility.UpdateCardVisibility(shouldSHow, skipAnimation);
            ingredientVisibility.UpdateCardVisibility(shouldSHow, skipAnimation);
        }

        public void SetNoteVisiblity(bool show)
        {
            if (DOTween.IsTweening(noteInputField.transform)) DOTween.Kill(noteInputField.transform);

            if (show)
            {
                sfxManager.PlaySfx(5);

                noteInputField.transform.position = initialPosition.transform.position;
                noteInputField.transform.DOMove(basePosition.transform.position, 0.7f)
                    .SetEase(Ease.OutExpo);

                LoadNoteData();
                noteInputField.gameObject.SetActive(true);
                transparentBackground.SetActive(true);
                return;
            }

            sfxManager.PlaySfx(4);

            noteInputField.transform.DOMove(initialPosition.transform.position, 0.7f)
                .SetEase(Ease.OutExpo)
                .OnComplete(HideNote);

            SaveNoteData();
        }

        private void HideNote()
        {
            noteInputField.gameObject.SetActive(false);
            transparentBackground.SetActive(false);
        }

        private void SaveNoteData()
        {
            PlayerData.SetNote(CurrentRecipeIndex, noteInputField.text);
        }

        private void LoadNoteData()
        {
            noteInputField.text = PlayerData.GetNote(CurrentRecipeIndex);
        }

        private void WiggleCard(int index, MemorizedState memorizedState)
        {
            if (CurrentRecipeIndex != index) return;

            if (memorizedState == MemorizedState.Yes)
                panelAnimControl.PlayAnim(CardAnimationType.WiggleRight);
            else
                panelAnimControl.PlayAnim(CardAnimationType.WiggleLeft);
        }
    }
}