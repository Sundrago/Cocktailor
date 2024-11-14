using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Cocktailor
{
    public class AlertDialogueObject : MonoBehaviour
    {
        [SerializeField] private Text title, message;
        [SerializeField] private AlertDialogButton buttonPrefab;
        [SerializeField] private Transform buttonHolder;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Transform alertPanel;
        
        public AlertDialogueObject SetTitle(string title)
        {
            this.title.text = title;
            return this;
        }
        
        public AlertDialogueObject SetMessage(string title)
        {
            this.message.text = title;
            return this;
        }

        public void AddButton(string buttonText, UnityAction action)
        {
            AlertDialogButton button = Instantiate(buttonPrefab, buttonHolder);
            button.Init(buttonText, action, Hide, Color.white);
            button.gameObject.SetActive(true);
        }
        
        public void AddCancelButton(string buttonText, UnityAction action)
        {
            AlertDialogButton button = Instantiate(buttonPrefab, buttonHolder);
            button.Init(buttonText, action, Hide, new Color(1,0.9f, 0.9f));
            button.gameObject.SetActive(true);
        }
        
        [Button]
        public void Show()
        {
            alertPanel.localPosition = new Vector3(0, -3000, 0);
            backgroundImage.color = new Color(1, 1, 1, 0);
            
            backgroundImage.DOFade(0.3f, 0.3f);
            alertPanel.DOLocalMoveY(0, 0.3f).SetEase(Ease.OutExpo);
            
            gameObject.SetActive(true);
        }

        [Button]
        public void Hide()
        {
            backgroundImage.DOFade(0, 0.3f);
            alertPanel.DOLocalMoveY(3000, 0.5f).SetEase(Ease.InOutExpo)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                });
        }

    }
}