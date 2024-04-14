using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Cocktailor
{
    /// <summary>
    /// Manager class for handling popup messages(use for simple warnings).
    /// </summary>
    public class PopupMessageManager : MonoBehaviour
    {
        [SerializeField] private SfxManager sfxManager;
        [SerializeField] private GameObject messageBoxObjectPrefab;
        public static PopupMessageManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void ShowMsg(string msg)
        {
            sfxManager.PlaySfx(9);

            var newMsg = Instantiate(messageBoxObjectPrefab, gameObject.transform);
            var title = newMsg.transform.Find("Text").gameObject;
            BeginMessageAnimation(msg, title, newMsg);
            newMsg.SetActive(true);
        }

        private static void BeginMessageAnimation(string msg, GameObject title, GameObject newMsg)
        {
            title.GetComponent<Text>().text = msg;
            newMsg.transform.DOMoveY(-Screen.height, 0.5f)
                .From()
                .SetRelative()
                .SetEase(Ease.OutQuint);
            newMsg.transform.DOMoveY(Screen.height / 3f, 0.5f)
                .SetRelative()
                .SetEase(Ease.OutQuint)
                .SetDelay(2.5f)
                .OnComplete(() => newMsg.SetActive(false));
        }
    }
}