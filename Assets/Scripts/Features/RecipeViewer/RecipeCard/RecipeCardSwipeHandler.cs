using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Cocktailor
{
    public enum SwipeEventType
    {
        SwipeLeft,
        SwipeRight
    }

    /// <summary>
    /// Handles UI swipe events for a recipe card and quiz card.
    /// </summary>
    public class RecipeCardSwipeHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private const float Velocity = 0.125f;
        
        [SerializeField] private SfxManager sfxManager;
        [SerializeField] private bool isInQuestionMode;
        
        private Vector2 defaultMousePosition;
        private Vector3 defaultPosition, targetPosition;
        private float defaultSize, targetRotation, targetSize;
        private bool initiated;
        private bool isDraggableCentered, shouldBeDestroyed;
        
        public Action<SwipeEventType> OnSwipeEvent { get; set; }

        public void Update()
        {
            if (!initiated) return;

            if (IsAnimationFinished())
                HandleDestruction();
            else
                UpdateTransform();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (shouldBeDestroyed) return;
            if (!isInQuestionMode) Destroy(gameObject.GetComponent<Animator>());

            defaultMousePosition = eventData.position;
            isDraggableCentered = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (shouldBeDestroyed) return;

            var currentMousePosition = eventData.position;
            var mousePositionDifference = currentMousePosition - defaultMousePosition;

            UpdateTargetTransform(mousePositionDifference);

            transform.position = targetPosition;
            transform.localScale = new Vector3(targetSize, targetSize, 1);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (shouldBeDestroyed) return;
            sfxManager.PlaySfx(2);

            if (transform.position.x >= Screen.width / 5f * 2f && transform.position.x <= Screen.width / 5f * 3f)
            {
                BackToCenter();
            }
            else if (transform.position.x < Screen.width / 5f * 2f)
            {
                HandleSwipeLeft();
            }
            else
            {
                HandleSwipeRight();
            }
        }

        private void HandleSwipeRight()
        {
            var diff = (targetPosition - defaultPosition) * 2.5f;
            targetSize = 1 / 5f;
            targetPosition = transform.position + diff;
            shouldBeDestroyed = true;

            if (targetPosition.x < Screen.width + Screen.width / 3f)
                targetPosition = new Vector3(Screen.width, targetPosition.y, 0);
            OnSwipeEvent?.Invoke(SwipeEventType.SwipeRight);
        }

        private void HandleSwipeLeft()
        {
            var diff = (targetPosition - defaultPosition) * 2.5f;
            targetSize = 0;
            targetPosition = transform.position + diff;
            shouldBeDestroyed = true;

            // if (targetPosition.x > -Screen.width / 3f)
            //     targetPosition = new Vector3(-Screen.width * 1.2f, targetPosition.y, 0);
            OnSwipeEvent?.Invoke(SwipeEventType.SwipeLeft);
        }

        private void BackToCenter()
        {
            targetPosition = defaultPosition;
            targetRotation = 0;
            targetSize = defaultSize;
        }


        public void InitDefaultPosition()
        {
            shouldBeDestroyed = false;
            initiated = true;
            defaultPosition = transform.position;
            defaultSize = 1;

            targetPosition = defaultPosition;
            targetSize = defaultSize;
        }

        private bool IsAnimationFinished()
        {
            var dist = Vector3.Distance(transform.position, targetPosition);
            return dist <= 0.1f;
        }

        private void HandleDestruction()
        {
            if (shouldBeDestroyed)
            {
                if (isInQuestionMode)
                    gameObject.SetActive(false);
                else
                    Destroy(gameObject);
            }
        }

        private void UpdateTransform()
        {
            var delta = Velocity / (1 - Time.deltaTime);
            UpdatePosition(delta);
            UpdateRotation(delta);
            UpdateScale(delta);
        }

        private void UpdateScale(float delta)
        {
            var newXScale = LerpTowardsTarget(transform.localScale.x, targetSize, delta);
            transform.localScale = new Vector3(newXScale, newXScale, 1);
        }

        private void UpdateRotation(float delta)
        {
            var currentAngle = transform.eulerAngles.z;
            if (currentAngle >= 200) currentAngle -= 360;
            var newZRotation = LerpTowardsTarget(currentAngle, targetRotation, delta);
            transform.rotation = Quaternion.Euler(0, 0, newZRotation);
        }

        private void UpdatePosition(float delta)
        {
            var newX = LerpTowardsTarget(transform.position.x, targetPosition.x, delta);
            var newY = LerpTowardsTarget(transform.position.y, targetPosition.y, delta);
            transform.position = new Vector3(newX, newY, 0);
        }

        private float LerpTowardsTarget(float currentValue, float targetValue, float delta)
        {
            var diff = targetValue - currentValue;
            return currentValue + diff * delta;
        }

        private void UpdateTargetTransform(Vector2 mousePositionDifference)
        {
            targetPosition = defaultPosition + new Vector3(mousePositionDifference.x, mousePositionDifference.y, 0);
            targetRotation = mousePositionDifference.y / Screen.height * 50f;

            if (transform.position.x < Screen.width / 2) targetRotation *= -1f;
            if (Mathf.Abs(transform.position.x - Screen.width / 2f) >= Screen.width / 4f)
                targetSize = map(Mathf.Abs(transform.position.x - Screen.width / 2f), Screen.width / 4f,
                    Screen.width / 2f,
                    1f, 4 / 5f);
        }

        [Button]
        public void Centerize()
        {
            targetPosition = defaultPosition;
            targetRotation = 0;
            transform.localScale = new Vector3(1, 1, 1);
            targetSize = defaultSize;

            transform.position = defaultPosition;
            transform.rotation = Quaternion.Euler(0, 0, targetRotation);
            shouldBeDestroyed = false;
            isDraggableCentered = true;
        }

        public void SwipeLeft()
        {
            targetSize = 2 / 3f;
            targetPosition = new Vector3(-1000, -500, 0);
            targetRotation = 10f;
            shouldBeDestroyed = true;

            OnSwipeEvent?.Invoke(SwipeEventType.SwipeLeft);
        }

        public void SwipeRight()
        {
            targetSize = 2 / 3f;
            targetPosition = new Vector3(2000, -500, 0);
            targetRotation = -10f;
            shouldBeDestroyed = true;

            OnSwipeEvent?.Invoke(SwipeEventType.SwipeRight);
        }

        private float map(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }
    }
}