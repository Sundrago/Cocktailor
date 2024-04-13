using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Cocktailor
{
    public class OpenPro : MonoBehaviour
    {
        [SerializeField] private Dropdown dropdown;

        [FormerlySerializedAs("settings")] [SerializeField]
        private SettingsManager settingsManager;
        // [FormerlySerializedAs("main")] [SerializeField] private RecipeViewerManager recipeViewerManager;

        public void OpenProPanelIfNotSubscribed()
        {
            if (PlayerData.HasSubscribed())
            {
                gameObject.SetActive(false);
                return;
            }

            dropdown.Hide();
            settingsManager.OpenCocktailorPro();
        }

        public void OpenProSettingsWhenLiteClicked()
        {
            settingsManager.OpenCocktailorPro();
        }

        public void ShowProSubscriptionMessage()
        {
            PopupMessageManager.Instance.ShowMsg("칵테일러 Pro를 이용중입니다.");
        }
    }
}