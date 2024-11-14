using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Cocktailor
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class AlertDialogButton : MonoBehaviour
    {
        [SerializeField] private Text buttonText;
        private Button button;
        private Image image;
        
        public void Init(string buttonText, UnityAction action, UnityAction action2, Color color)
        {
            button = GetComponent<Button>();
            image = GetComponent<Image>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
            button.onClick.AddListener(action2);
            this.buttonText.text = buttonText;
            image.color = color;
        }
    }
}