using Cocktailor;
using Features.Quiz;
using UnityEngine;

public enum RecipeType
{
    Glassware,
    Garnish,
    Method,
    Ingredients,
    Quantity
}

[RequireComponent(typeof(DragEventManager))]
[RequireComponent(typeof(PanelAnimControl))]
public class QuizCardController : MonoBehaviour
{
    [Header("Managers and controllers")] [SerializeField]
    private QuizCardUIManager UIManager;

    [SerializeField]
    private IngredientSelectorUI glasswareSlector, garnishSelector, MethodSeletor, IngredientSelector, AmountSelector;

    public DragEventManager DragEventManager { get; private set; }
    public PanelAnimControl PanelAnimControl { get; private set; }
    public int CocktailIndex { get; private set; }
    public CocktailRecipe CurrentRecipe { get; private set; }
    public int Score { get; private set; } = -1;
    public bool AnswerMode { get; private set; }

    private UserAnswer userAnswer;
    private int ingredientCount = 1;

    private void Awake()
    {
        DragEventManager = GetComponent<DragEventManager>();
        PanelAnimControl = GetComponent<PanelAnimControl>();
    }

    public void LoadCocktail(int cocktailIndex, int quizIndex)
    {
        CocktailIndex = cocktailIndex;
        CurrentRecipe = CocktailRecipeManger.GetCocktailRecipeByIndex(cocktailIndex);

        GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 30, 0);
        transform.localScale = new Vector3(1f, 1f, 1f);
        DragEventManager.InitDefaultPosition();
        ingredientCount = 0;
        userAnswer = new UserAnswer();

        InitSelectors();
        UIManager.InitUI(cocktailIndex, quizIndex);
        UIManager.UpdateIngredientButton(userAnswer, ingredientCount);
    }

    private void InitSelectors()
    {
        glasswareSlector.Init(RecipeType.Glassware);
        glasswareSlector.OnValueSelected += HandleUserRecipeSelection;

        garnishSelector.Init(RecipeType.Garnish);
        garnishSelector.OnValueSelected += HandleUserRecipeSelection;

        MethodSeletor.Init(RecipeType.Method);
        MethodSeletor.OnValueSelected += HandleUserRecipeSelection;
        HideAllSelector();
    }

    private void HideAllSelector()
    {
        glasswareSlector.gameObject.SetActive(false);
        garnishSelector.gameObject.SetActive(false);
        MethodSeletor.gameObject.SetActive(false);
        IngredientSelector.gameObject.SetActive(false);
        AmountSelector.gameObject.SetActive(false);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
        DragEventManager.Centerize();
        HideAllSelector();
    }

    public void TestBtnCheckClicked()
    {
        QuizFinished();
    }

    public void QuizFinished()
    {
        DragEventManager.SwipeRight();
        AnswerMode = true;
        HideAllSelector();
        ScoreUserAnswer();
    }

    private void ScoreUserAnswer()
    {
        Score = QuizJudge.GetQuizResult(UIManager, userAnswer, CurrentRecipe, ingredientCount);
        UIManager.SetScoreUI(Score);
    }
    
    private void HandleUserRecipeSelection(RecipeType recipeType, string selectedAnswer, int firstChoiceIndex,
        int secondChoiceIndex = -1, int ingredientIndex = -1)
    {
        if (ShouldRemoveIngredient(recipeType, firstChoiceIndex))
        {
            RemoveIngredient(ingredientIndex);
            return;
        }

        userAnswer.UpdateAnswer(recipeType, selectedAnswer, firstChoiceIndex, secondChoiceIndex, ingredientIndex);
        UIManager.UpdateAnswerUI(recipeType, selectedAnswer, ingredientIndex);
        ScoreUserAnswer();
    }

    private bool ShouldRemoveIngredient(RecipeType recipeType, int choiceIndex)
    {
        return recipeType == RecipeType.Ingredients && choiceIndex == -1;
    }
    
    private void RemoveIngredient(int index)
    {
        ingredientCount -= 1;
        userAnswer.RemoveIngredientAt(index);
        UIManager.UpdateIngredientButton(userAnswer, ingredientCount);
    }

    public void GlassBtnClicked()
    {
        if (AnswerMode) return;
        glasswareSlector.OpenPanel(userAnswer.GlasswareIndex);
    }

    public void GarnishBtnClicked()
    {
        if (AnswerMode) return;
        garnishSelector.OpenPanel(userAnswer.GarnishIndex);
    }

    public void MethodeBtnClicked()
    {
        if (AnswerMode) return;
        MethodSeletor.OpenPanel(userAnswer.MethodIndex);
    }

    public void IngredientBtnClicked(int i)
    {
        if (AnswerMode) return;
        if (i == ingredientCount)
        {
            UIManager.SetIngredientButtonActive(i);
            if (i < 7)
                ingredientCount += 1;
            UIManager.UpdateIngredientButton(userAnswer, ingredientCount);
            return;
        }

        IngredientSelector.Init(RecipeType.Ingredients, i, userAnswer.IngredientIndex[i].Item1,
            userAnswer.IngredientIndex[i].Item2);
        IngredientSelector.OnValueSelected += HandleUserRecipeSelection;
    }

    public void AmountBtnClicked(int i)
    {
        if (AnswerMode) return;
        if (i == ingredientCount)
        {
            UIManager.SetIngredientButtonActive(i);
            if (i < 7)
                ingredientCount += 1;
            UIManager.UpdateIngredientButton(userAnswer, ingredientCount);
            return;
        }

        AmountSelector.Init(RecipeType.Ingredients, i, userAnswer.AmountIndex[i].Item1,
            userAnswer.AmountIndex[i].Item2);
        AmountSelector.OnValueSelected += HandleUserRecipeSelection;
    }

    public void OpenCardAnswer()
    {
        AnswerMode = true;
        ShowPanel();
    }
}