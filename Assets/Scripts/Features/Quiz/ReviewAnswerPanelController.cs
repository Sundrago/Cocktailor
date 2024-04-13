using System;
using System.Collections.Generic;
using Cocktailor.Utility;
using Features.Quiz;
using Features.RecipeViewer;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Features.Quize
{
    public class ReviewAnswerPanelController : MonoBehaviour
    {
        [Header("Managers and Controllers")]
        [SerializeField] private SfxManager sfxManager;
        [SerializeField] private QuizManager quizManager;
        [SerializeField] private BottomUIStateManager bottomUIStateManager;
        [SerializeField] private Animator reviewAnswerAnimator;

        [Header("UI Components")]
        [SerializeField] private Text ansFinalScore;
        [SerializeField] private Text[] ansName, ansScore;
        [SerializeField] private RecipeCardController answerCardPrefab;
        [SerializeField] private Transform cardHolder;
        [SerializeField] private Text[] testShowButtonTexts;
        
        private List<QuizCardController> quizCards;
        private bool[] answerReady;
        private int currentQuizCardIndex;
        private RecipeCardController answerCard;
        
        private static readonly int Show = Animator.StringToHash("show");
        
        private enum ReviewState { OnMain, OnQuizCard, OnAnswerCard }

        private ReviewState reviewState;

        public void UpdateTestResults(List<QuizCardController> quizCards)
        {
            SetReviewState(ReviewState.OnMain);
            
            this.quizCards = quizCards;

            var totalScore = quizCards[0].Score;
            totalScore += quizCards[1].Score;
            totalScore += quizCards[2].Score;
            
            if (totalScore >= 210) ansFinalScore.text = "테스트 결과 - 합격!";
            else ansFinalScore.text = "테스트 결과 - 불합격";
            ansName[0].text = "1. " + quizCards[0].CurrentRecipe.Name;
            ansName[1].text = "2. " + quizCards[1].CurrentRecipe.Name;
            ansName[2].text = "3. " + quizCards[2].CurrentRecipe.Name;

            for (var i = 0; i < 3; i++)
                // if (quizCards[i].GetComponent<QuizCardController>().Score < 70)
                //     ansScore[i].text = "FAIL";
                // else
                ansScore[i].text = "" + quizCards[i].Score + "점";

            answerReady = new bool[3];
            UpdateAnswerButtonState();
        }

        public void OnCheckAnswerButtonClick(int i)
        {
            sfxManager.PlaySfx(3);

            if (!answerReady[i])
            {
                // gameObject.GetComponent<AdManager>().ShowRewardAds(AdManager.AdType.CheckQuiz, i);
                //NEED TO IMPLEMENT
                answerReady[i] = true;
                UpdateAnswerButtonState();
                return;
            }
            OpenQuizCard(i);
        }

        private void SetReviewState(ReviewState reviewState)
        {
            this.reviewState = reviewState;
            switch (this.reviewState)
            {
                case ReviewState.OnMain:
                    MenuUIManager.Instance.ShowMenuInterface();
                    break;
                case ReviewState.OnQuizCard:
                    MenuUIManager.Instance.HideMenuInterface();
                    break;
                case ReviewState.OnAnswerCard:
                    MenuUIManager.Instance.HideMenuInterface();
                    break;
            }
        }
        
        private void OpenQuizCard(int i)
        {
            SetReviewState(ReviewState.OnQuizCard);

            CheckIfOpenQuizCardExists();
            currentQuizCardIndex = i;
            quizCards[i].OpenCardAnswer();
            quizCards[i].PanelAnimControl.PlayAnim(CardAnimationType.InFromRight);
            quizCards[i].DragEventManager.OnSwipeEvent += OnQuizCardClose;
        }

        private void CheckIfOpenQuizCardExists()
        {
            foreach (var quizCard in quizCards)
            {
                if(quizCard.gameObject.activeSelf) 
                    quizCard.DragEventManager.SwipeRight();
            }
        }

        private void OnQuizCardClose(SwipeEventType swipeEventType)
        {
            SetReviewState(ReviewState.OnMain);
            
            quizCards[currentQuizCardIndex].DragEventManager.OnSwipeEvent -= OnQuizCardClose;
        }

        public void OpenAnswerCard()
        {
            if (reviewState == ReviewState.OnAnswerCard)
            {
                CloseAnswerCard();
                return;
            }
            SetReviewState(ReviewState.OnAnswerCard);
            
            int recipeIndex = quizCards[currentQuizCardIndex].CocktailIndex;

            CardInfo cardInfo = new CardInfo(
                recipeIndex: recipeIndex,
                cardIndex: currentQuizCardIndex,
                cardMaxIndex: quizCards.Count,
                onCardSwipe: OnAnswerCardClose);

            answerCard = RecipeCardManager.Instance.OpenCard(cardInfo);
        }

        private void CloseAnswerCard()
        {
            answerCard.DragEventManager.SwipeRight();
        }
        
        private void OnAnswerCardClose(SwipeEventType swipeEventType)
        {
            SetReviewState(ReviewState.OnQuizCard);
            
            answerCard.DragEventManager.OnSwipeEvent -= OnAnswerCardClose;
        }
        
        private void UpdateAnswerButtonState()
        {
            bool hasUserSubscribed = PlayerData.HasSubscribed();

            for (int i = 0; i < 3; i++)
            {
                if (hasUserSubscribed) answerReady[i] = true;
                testShowButtonTexts[i].text = answerReady[i] ? "답안 확인!" : "광고보고 답안확인";
            }
        }
    }
}