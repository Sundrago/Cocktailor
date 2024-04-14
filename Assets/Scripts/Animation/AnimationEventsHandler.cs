using System;
using UnityEngine;

namespace Cocktailor
{
    /// <summary>
    /// provides a collection of public methods intended to serve as event handlers
    /// for Unity's Animation system
    /// </summary>
    public class AnimationEventsHandler : MonoBehaviour
    {
        [SerializeField] private GameObject targetObject;
        [SerializeField] private GameObject parentObject;
        
        public Action StandaloneAction
        {
            get => standaloneAction;
            set => standaloneAction = value ?? throw new ArgumentNullException(nameof(value), "Action cannot be null.");
        }
    
        public Action<GameObject> GameObjectAction
        {
            get => gameObjectAction;
            set => gameObjectAction = value ?? throw new ArgumentNullException(nameof(value), "Action cannot be null.");
        }
        
        private Action standaloneAction;
        private Action<GameObject> gameObjectAction;
        
        public void SetTargetObject(GameObject target)
        {
            targetObject = target;
        }
        
        public void SetParentObject(GameObject parent)
        {
            parentObject = parent;
        }
        
        public void DestroyParent()
        {
            Destroy(parentObject);
        }
        
        public void PerformActionOnTarget()
        {
            gameObjectAction?.Invoke(targetObject);
        }
        
        public void PerformAction()
        {
            standaloneAction?.Invoke();
        }
    
        public void PlayAudioSource()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if(audioSource != null)
                audioSource.Play();
        }

        public void DeactivateParentObject()
        {
            parentObject.SetActive(false);
        }
    
        public void DestroyParentObject()
        {
            Destroy(parentObject);
        }
    
        public void DeactivateTargetObject()
        {
            targetObject.SetActive(false);
        }
    }
}