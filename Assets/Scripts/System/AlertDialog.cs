using System;
using UnityEngine;

namespace Cocktailor
{
    public class AlertDialog : MonoBehaviour
    {
        [SerializeField] private AlertDialogueObject alertDialoguePrefab;

        public static AlertDialog Instance;

        private void Awake()
        {
            Instance = this;
        }

        public AlertDialogueObject CreateInstance()
        {
            return Instantiate(alertDialoguePrefab, gameObject.transform);
        }

    }
}