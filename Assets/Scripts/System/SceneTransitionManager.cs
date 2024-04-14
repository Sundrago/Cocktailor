using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cocktailor
{
    /// <summary>
    /// Manages scene transitions and provides a method to indicate when the transition is ready.
    /// </summary>
    /// 
    public class SceneTransitionManager : MonoBehaviour
    {
        [SerializeField] private GameObject loadingImage;
        [SerializeField] private string SceneName;
        
        private Animator gameObjectAnimator, loadingImageAnimator;
        private bool sceneLoadCompleted, transitionCompleted, transitionReady;

        private static readonly int Play = Animator.StringToHash("play");
        private static readonly int TransitionFinished = Animator.StringToHash("transitionFinished");
        private static readonly int Pause = Animator.StringToHash("pause");

        private void Start()
        {
            if (string.IsNullOrEmpty(SceneName)) return;
            
            gameObjectAnimator = gameObject.GetComponent<Animator>();
            gameObjectAnimator.SetTrigger(Play);
            loadingImageAnimator = loadingImage.GetComponent<Animator>();
            loadingImageAnimator.SetTrigger(Play);
            LoadScene();
        }

        private void LoadScene()
        {
            var loadOperation = SceneManager.LoadSceneAsync(SceneName);
            loadOperation.allowSceneActivation = false;
            StartCoroutine(ProcessSceneLoad(loadOperation));
        }

        private IEnumerator ProcessSceneLoad(AsyncOperation loadOperation)
        {
            while (!transitionCompleted)
            {
                if ((loadOperation.progress >= 0.99f) & !sceneLoadCompleted & (Time.time > 1f))
                {
                    gameObjectAnimator.SetTrigger(TransitionFinished);
                    loadingImageAnimator.SetTrigger(Pause);
                    sceneLoadCompleted = true;
                }

                if (transitionReady)
                {
                    transitionCompleted = true;
                    loadOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        public void TransitionReadyCall()
        {
            if (SceneName == "") return;
            transitionReady = true;
        }
    }
}