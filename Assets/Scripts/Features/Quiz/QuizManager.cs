using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Cocktailor
{
    public class QuizManager : MonoBehaviour
    {
        private const int TestDurationInSec = 420;

        [Header("Managers and Controllers")] [SerializeField]
        private RecipeViewerManager recipeViewerManager;

        [SerializeField] private BottomUIStateManager bottomUIStateManager;
        [SerializeField] private SfxManager sfxManager;
        [SerializeField] private ReviewAnswerPanelController reviewAnswerPanelController;

        [Header("UI Components")] [SerializeField]
        private Dropdown rangeChoiceDropdown;

        [SerializeField] private QuizCardController quizCardPrefab;
        [SerializeField] private Transform quizCardHolder;
        [SerializeField] private GameObject preparationStage, reviewStage, quizStage;
        [SerializeField] private Text timerDisplay;
        private int currentListIdx, currentQuizCardIndex;

        //fields
        private List<QuizCardController> quizCards;
        private QuizState quizState;
        private int[] randomRecipeSelections;
        private float startTime;
        private float timer;
        public static QuizManager Instance { get; private set; }

        //properties
        public Action<int> OnQuizCardIndexChange { get; set; }

        private int CurrentQuizCardIndex
        {
            get => currentQuizCardIndex;
            set
            {
                if (currentQuizCardIndex == value) return;

                currentQuizCardIndex = value;
                OnQuizCardIndexChange?.Invoke(value);
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            InitializeQuizState(QuizState.PreparatonStage);
        }

        private void Update()
        {
            if (quizState == QuizState.QuizStage) UpdateQuizTimerUI();
        }

        private void UpdateQuizTimerUI()
        {
            timer = TestDurationInSec + startTime - Time.time;
            if (timer > 0)
            {
                timerDisplay.text = FormatTime(timer);
            }
            else
            {
                timerDisplay.text = "시간 종료";
                FinishTest();
            }
        }

        private string FormatTime(float timeInSec)
        {
            var minutes = Mathf.FloorToInt(timeInSec / 60);
            var seconds = Mathf.FloorToInt(timeInSec % 60);
            return minutes > 0 ? $"{minutes}분 {seconds}초" : $"{seconds}초";
        }

        private void InitializeQuizState(QuizState quizState)
        {
            this.quizState = quizState;

            preparationStage.SetActive(quizState == QuizState.PreparatonStage);
            quizStage.SetActive(quizState == QuizState.QuizStage);
            reviewStage.SetActive(quizState == QuizState.ReviewStage);
        }

        public void InitializeQuizRecipeAndBeginQuiz()
        {
            var rangeChoice = (QuizRangeChoice)rangeChoiceDropdown.value;
            randomRecipeSelections = new int[3];

            switch (rangeChoice)
            {
                case QuizRangeChoice.TotalRange:
                    GenerateQuizRandomNumbers(CocktailRecipeManger.GetTotalRecipeCount());
                    break;
                case QuizRangeChoice.NotMemorizedRecipesRange:
                    SetupQuizFromGivenRecipes(PlayerData.NotMemorizedRecipes);
                    break;
                case QuizRangeChoice.MemorizedRecipesRange:
                    SetupQuizFromGivenRecipes(PlayerData.MemorizedRecipes);
                    break;
            }

            BeginQuiz();
        }

        public void ContinueQuiz()
        {
            MenuUIManager.Instance.HideMenuInterface();
        }

        private void GenerateQuizRandomNumbers(int range)
        {
            randomRecipeSelections[0] = GetRandomUniqueNumber(range);
            randomRecipeSelections[1] = GetRandomUniqueNumber(range, randomRecipeSelections.Take(1).ToArray());
            randomRecipeSelections[2] = GetRandomUniqueNumber(range, randomRecipeSelections.Take(2).ToArray());
        }

        private void SetupQuizFromGivenRecipes(List<int> selectedRecipeList)
        {
            var selectedRangeCount = selectedRecipeList.Count;
            var totalRecipeCount = CocktailRecipeManger.GetTotalRecipeCount();
            if (selectedRangeCount < 3)
            {
                randomRecipeSelections[0] = selectedRangeCount >= 1
                    ? selectedRecipeList[0]
                    : GetRandomUniqueNumber(totalRecipeCount);
                randomRecipeSelections[1] = selectedRangeCount >= 2
                    ? selectedRecipeList[1]
                    : GetRandomUniqueNumber(totalRecipeCount, randomRecipeSelections.Take(1).ToArray());
                randomRecipeSelections[2] =
                    GetRandomUniqueNumber(totalRecipeCount, randomRecipeSelections.Take(2).ToArray());
                PopupManager.Instance.OpenPopup(PopupType.SingleButton,
                    "선택한 범위에서 출제 가능한 레시피가 부족합니다.\n일부 문제는 전체 범위에서 출제됩니다.", BeginQuiz);
            }
            else
            {
                randomRecipeSelections[0] = GetRandomUniqueNumber(selectedRangeCount);
                randomRecipeSelections[1] =
                    GetRandomUniqueNumber(selectedRangeCount, randomRecipeSelections.Take(1).ToArray());
                randomRecipeSelections[2] =
                    GetRandomUniqueNumber(selectedRangeCount, randomRecipeSelections.Take(2).ToArray());

                randomRecipeSelections[0] = selectedRecipeList[randomRecipeSelections[0]];
                randomRecipeSelections[1] = selectedRecipeList[randomRecipeSelections[1]];
                randomRecipeSelections[2] = selectedRecipeList[randomRecipeSelections[2]];
            }
        }

        private int GetRandomUniqueNumber(int count, params int[] excludes)
        {
            int rnd;
            do
            {
                rnd = Random.Range(0, count);
            } while (excludes.Contains(rnd));

            return rnd;
        }

        private void BeginQuiz()
        {
            InitializeQuiz();
            SetupUIForQuiz();
            SetupQuizCards();
        }

        private void InitializeQuiz()
        {
            InitializeQuizState(QuizState.QuizStage);
            recipeViewerManager.isInQuizMode = true;
            startTime = Time.time;
        }

        private void SetupUIForQuiz()
        {
            sfxManager.PlaySfx(9);
            bottomUIStateManager.SwitchUILayout(BottomUIStateManager.BottomUILayout.QuizDisplayed);
            timerDisplay.gameObject.SetActive(true);
            if (RecipeCardManager.Instance.CurrentRecipeCard != null)
                RecipeCardManager.Instance.CurrentRecipeCard.gameObject.SetActive(false);
            MenuUIManager.Instance.HideMenuInterface();
        }

        private void SetupQuizCards()
        {
            CurrentQuizCardIndex = 0;
            quizCards = GetQuizCards();
        }

        private List<QuizCardController> GetQuizCards()
        {
            quizCards = new List<QuizCardController>();
            for (var i = 0; i < 3; i++)
            {
                quizCards.Add(Instantiate(quizCardPrefab,
                    quizCardPrefab.transform.position, Quaternion.identity, quizCardHolder.transform));
                quizCards[i].LoadCocktail(randomRecipeSelections[i], i);
                quizCards[i].DragEventManager.OnSwipeEvent += ProcessSwipeEvent;

                if (i != 0) quizCards[i].HidePanel();
            }

            return quizCards;
        }

        private void ProcessSwipeEvent(SwipeEventType swipeEventType)
        {
            switch (swipeEventType)
            {
                case SwipeEventType.SwipeLeft when CurrentQuizCardIndex is 0 or 1:
                    OpenQuizCard(CurrentQuizCardIndex + 1);
                    quizCards[CurrentQuizCardIndex].PanelAnimControl.PlayAnim(CardAnimationType.InFromRight);
                    break;
                case SwipeEventType.SwipeLeft when CurrentQuizCardIndex is 2:
                    quizCards[2].ShowPanel();
                    quizCards[2].PanelAnimControl.PlayAnim(CardAnimationType.InFromLeft);
                    PromptUserForQuizSubmission();
                    break;
                case SwipeEventType.SwipeRight when CurrentQuizCardIndex is 0:
                    quizCards[0].ShowPanel();
                    PromptUserForQuizExit();
                    quizCards[0].PanelAnimControl.PlayAnim(CardAnimationType.InFromRight);
                    break;
                case SwipeEventType.SwipeRight when CurrentQuizCardIndex is 1 or 2:
                    OpenQuizCard(CurrentQuizCardIndex - 1);
                    quizCards[CurrentQuizCardIndex].PanelAnimControl.PlayAnim(CardAnimationType.InFromLeft);
                    break;
            }
        }

        private void OpenQuizCard(int i)
        {
            CurrentQuizCardIndex = i;
            quizCards[i].ShowPanel();
        }

        private void QuitTest()
        {
            bottomUIStateManager.SwitchUILayout(BottomUIStateManager.BottomUILayout.DefaultDisplayed);
            recipeViewerManager.isInQuizMode = false;
            timerDisplay.gameObject.SetActive(false);
            RemoveQuizCards();
            if (RecipeCardManager.Instance.CurrentRecipeCard != null)
                RecipeCardManager.Instance.CurrentRecipeCard.gameObject.SetActive(true);

            InitializeQuizState(QuizState.PreparatonStage);
        }

        private void RemoveQuizCards()
        {
            foreach (var quizCard in quizCards) Destroy(quizCard);
            quizCards.Clear();
        }

        private void FinishTest()
        {
            InitializeQuizState(QuizState.ReviewStage);

            sfxManager.PlaySfx(8);
            recipeViewerManager.isInQuizMode = true;
            timerDisplay.gameObject.SetActive(false);
            bottomUIStateManager.SwitchUILayout(BottomUIStateManager.BottomUILayout.QuizFinishedDisplayed);

            foreach (var quizCard in quizCards)
            {
                quizCard.DragEventManager.OnSwipeEvent -= ProcessSwipeEvent;
                quizCard.QuizFinished();
            }

            reviewAnswerPanelController.UpdateTestResults(quizCards);
        }

        public void PromptUserForQuizExit()
        {
            if (!recipeViewerManager.isInQuizMode)
            {
                QuitTest();
                return;
            }

            PopupManager.Instance.OpenPopup(PopupType.TwoButtons, "진행중인 시험이 있습니다.\n종료하시겠습니까?", QuitTest);
        }

        public void PromptUserForQuizSubmission()
        {
            PopupManager.Instance.OpenPopup(PopupType.TwoButtons, "답안을 제출하시겠습니까?", FinishTest);
        }

        public void TerminateQuiz()
        {
            PopupManager.Instance.OpenPopup(PopupType.TwoButtons, "시험을 종료합니다",
                () =>
                {
                    RemoveQuizCards();
                    InitializeQuizState(QuizState.PreparatonStage);
                });
        }

        //enums
        private enum QuizState
        {
            PreparatonStage,
            QuizStage,
            ReviewStage
        }

        private enum QuizRangeChoice
        {
            TotalRange = 0,
            NotMemorizedRecipesRange = 1,
            MemorizedRecipesRange = 2
        }
    }
}