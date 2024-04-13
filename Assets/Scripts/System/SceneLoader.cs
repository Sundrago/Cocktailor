using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cocktailor
{
    public class SceneLoader : MonoBehaviour
    {
        public string SceneName;
        public GameObject loadingImage;
        public bool transitionReady;
        private Animator gameObjectAnimator;
        private Animator loadingImageAnimator;
        private bool sceneLoadCompleted;
        private bool transitionCompleted;

        private void Start()
        {
            if (SceneName == "") return;
            gameObjectAnimator = gameObject.GetComponent<Animator>();
            gameObjectAnimator.SetTrigger("play");
            loadingImageAnimator = loadingImage.GetComponent<Animator>();
            loadingImageAnimator.SetTrigger("play");
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
                if ((loadOperation.progress == 0.99f) & !sceneLoadCompleted & (Time.time > 1f))
                {
                    gameObjectAnimator.SetTrigger("transitionFinished");
                    loadingImageAnimator.SetTrigger("pause");
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