using System;
using System.Collections.Generic;
using Cocktailor;
using Cocktailor.Utility;
using DG.Tweening;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(DragEventManager))]
public class RecipeCardController : MonoBehaviour
{
    [Header("Managers and Controllers")]
    [SerializeField] private SfxManager sfxManager;
    [SerializeField] private CocktailIconManager iconManager;
    [SerializeField] private RecipeMarkerController recipeMarkerController;
    [SerializeField] public PanelAnimControl panelAnimControl;
    public DragEventManager DragEventManager { get; private set; }

    [Header("UI Components")] 
    [SerializeField] private Image myIcon;
    [SerializeField] private RectTransform recipePanelScaler;
    [SerializeField] private Text nameText, descriptionText, indexText;
    [SerializeField] private MemoryCardTabController iconTab, glasswareTab, methodTab, garnishTab, ingredientTab;
    [SerializeField] private GameObject closeButton, transparentBackground;
    [SerializeField] private TMP_InputField noteInputField;
    [SerializeField] private Image noteButton;
    [SerializeField] private Transform initialPosition, basePosition;

    public int CurrentRecipeIndex { get; private set; }
    
    private void Awake()
    {
        DragEventManager = GetComponent<DragEventManager>();
        PlayerData.OnuserMemorizedStatehange += WiggleCard;
    }

    public void UpdateCocktailInfo(CardInfo cardInfo)
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
        recipeMarkerController.InitializeMarker(CurrentRecipeIndex);
    }

    private void UpdateCardUIElements(CocktailRecipe recipe, CardInfo cardInfo)
    {
        indexText.text = cardInfo.CardIndex + 1 + " / " + cardInfo.CardMaxIndex;
        DragEventManager.InitDefaultPosition();
        nameText.text = recipe.Name;
        descriptionText.text = recipe.Description;
        glasswareTab.SetCardDescriptionText(recipe.Glassware);
        methodTab.SetCardDescriptionText(recipe.PreparationMethod);
        garnishTab.SetCardDescriptionText(ParseListToString(recipe.Garnish));

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

        ingredientTab.SetCardDescriptionText(ingredientsString, quantitiesSting);
    }

    private string ParseListToString(List<string> garnishes)
    {
        if (garnishes == null || garnishes.Count == 0)
        {
            return string.Empty;
        }
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
        iconTab.UpdateCardVisibility(shouldSHow, skipAnimation);
        glasswareTab.UpdateCardVisibility(shouldSHow, skipAnimation);
        methodTab.UpdateCardVisibility(shouldSHow, skipAnimation);
        garnishTab.UpdateCardVisibility(shouldSHow, skipAnimation);
        ingredientTab.UpdateCardVisibility(shouldSHow, skipAnimation);
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

    private void OnDestroy()
    {
        PlayerData.OnuserMemorizedStatehange -= WiggleCard;
    }
}