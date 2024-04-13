using System;
using Cocktailor;
using UnityEngine;

namespace UI
{
    public class CocktailIconManager : MonoBehaviour
    {
        [SerializeField] private Sprite[] iconSprites;
        [SerializeField] private Sprite[] smallMarkIcon;

        public static CocktailIconManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public Sprite GetIconSpriteAtIndexOf(int index)
        {
            if (index < 0 || index >= iconSprites.Length)
            {
                Debug.LogError("Index out of bounds while accessing sprite array.");
                return null;
            }
            return iconSprites[index];
        }
        
        public Sprite GetSmallMarkIconAtIndexOf(int index)
        {
            if (index < 0 || index >= smallMarkIcon.Length)
            {
                Debug.LogError("Index out of bounds while accessing small mark icon array.");
                return null;
            }
            return smallMarkIcon[index];
        }
    }
}