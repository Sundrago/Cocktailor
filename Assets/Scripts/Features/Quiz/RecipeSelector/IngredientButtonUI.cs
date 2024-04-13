using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Features.Quiz
{
    public enum RcpAmtButtonState { DeActive, PressToAdd, Active}
    public class IngredientButtonUI : MonoBehaviour
    {
        [SerializeField] private Text addButtonText;
        [SerializeField] private Text ingredientNameText;
        [SerializeField] private Text quantityText;
        [SerializeField] private Image iconImage;
        [SerializeField] private Animator animator;
        
        public void SetButtonState(RcpAmtButtonState state)
        {
            gameObject.SetActive(state != RcpAmtButtonState.DeActive);
            addButtonText.gameObject.SetActive(state == RcpAmtButtonState.PressToAdd);
            ingredientNameText.gameObject.SetActive(state == RcpAmtButtonState.Active);
            quantityText.gameObject.SetActive(state == RcpAmtButtonState.Active);
        }

        public void SetButtonActive()
        {
            animator.SetTrigger("show");
            ingredientNameText.text = "재료 선택";
            quantityText.text = "oz";
            SetButtonState(RcpAmtButtonState.Active);
        }
        public void SetIngredientNameText(string text)
        {
            ingredientNameText.text = text;
        }

        public void SetQuantityText(string text)
        {
            quantityText.text = text;
        }

        public void SetIngredientAndQuantity(string ingredient, string quantity)
        {
            ingredientNameText.text = string.IsNullOrEmpty(ingredient)? "재료 선택" : ingredient;
            quantityText.text = string.IsNullOrEmpty(quantity)? "oz" : quantity;
        }

        public void SetIconImage(Sprite sprite)
        {
            if(iconImage.sprite != sprite)
                iconImage.sprite = sprite;
        }

        // [Sirenix.OdinInspector.Button]
        // private void GetChildes()
        // {
        //     iconImage = GetComponent<Image>();
        //     addButtonText = transform.Find("add").GetComponent<Text>();
        //     ingredientNameText = transform.Find("receipe").GetComponent<Text>();
        //     quantityText = transform.Find("amount").GetComponent<Text>();
        //     ingredientImageIcon = transform.Find("btnL").GetComponent<Image>();
        //     quantityImageIcon = transform.Find("btnR").GetComponent<Image>();
        //     animator = GetComponent<Animator>();
        // }
    }
}