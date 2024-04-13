using System;
using Features.Quize;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum SwipeEventType
{
    SwipeLeft,
    SwipeRight
}

public class DragEventManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private SfxManager sfxManager;
    // [SerializeField] private MainControl mainControl;
    // [SerializeField] private QuizManager quizManager;
    
    public Action<SwipeEventType> OnSwipeEvent { get; set; }
    
    private Vector3 defaultPosition;
    private Vector3 targetPosition;
    private float targetRotation;
    private Vector2 defaultMousePosition;
    private readonly float velocity = 0.125f;

    private float defaultSize;
    private float targetSize;

    [FormerlySerializedAs("destroySelf")] private bool shouldBeDestroyed;
    [FormerlySerializedAs("searchMode")] public bool isInSearchMode;
    [FormerlySerializedAs("qMode")] public bool isInQuestionMode;
    [FormerlySerializedAs("ansMode")] public bool isInAnswerMode;
    
    private bool isDraggableCentered;
    private bool initiated;


    public void InitDefaultPosition()
    {
        shouldBeDestroyed = false;
        initiated = true;
        defaultPosition = transform.position;
        defaultSize = 1;
        
        targetPosition = defaultPosition;
        targetSize = defaultSize;
    }
    
    public void Update()
    {
        if(!initiated) return;
        
        if (IsAnimationFinished())
        {
            HandleDestruction();
        }
        else
        {
            UpdateTransform();
        }
    }

    private bool IsAnimationFinished()
    {
        var dist = Vector3.Distance(transform.position, targetPosition);
        return (dist <= 1.1f);
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
        var delta = velocity / (1 - Time.deltaTime);

        var newX = LerpTowardsTarget(transform.position.x, targetPosition.x, delta);
        var newY = LerpTowardsTarget(transform.position.y, targetPosition.y, delta);
        transform.position = new Vector3(newX, newY, 0);

        var currentAngle = transform.eulerAngles.z;
        if (currentAngle >= 200) currentAngle -= 360;
        var newZRotation = LerpTowardsTarget(currentAngle, targetRotation, delta);
        transform.rotation = Quaternion.Euler(0, 0, newZRotation);

        var newXScale = LerpTowardsTarget(transform.localScale.x, targetSize, delta);
        transform.localScale = new Vector3(newXScale, newXScale, 1);
    }
    
    private float LerpTowardsTarget(float currentValue, float targetValue, float delta)
    {
        var diff = targetValue - currentValue;
        return currentValue + diff * delta;
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

    private void UpdateTargetTransform(Vector2 mousePositionDifference)
    {
        targetPosition = defaultPosition + new Vector3(mousePositionDifference.x, mousePositionDifference.y, 0);
        targetRotation = mousePositionDifference.y / Screen.height * 50f;

        if (transform.position.x < Screen.width / 2) targetRotation *= -1f;
        if (Mathf.Abs(transform.position.x - Screen.width / 2f) >= Screen.width / 4f)
            targetSize = map(Mathf.Abs(transform.position.x - Screen.width / 2f), Screen.width / 4f, Screen.width / 2f,
                1f, 4 / 5f);
    }
    
    [Sirenix.OdinInspector.Button]
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

    public void OnEndDrag(PointerEventData eventData)
    {
        if (shouldBeDestroyed) return;
        sfxManager.PlaySfx(2);

        if (transform.position.x >= Screen.width / 5f * 2f && transform.position.x <= Screen.width / 5f * 3f)
        {
            //Center
            targetPosition = defaultPosition;
            targetRotation = 0;
            targetSize = defaultSize;
        }
        else if (transform.position.x < Screen.width / 5f * 2f)
        {
            // Left
            var diff = (targetPosition - defaultPosition) * 2.5f;
            targetSize = 2 / 3f;
            targetPosition = transform.position + diff;
            shouldBeDestroyed = true;
            
            // quizManager.AnswerClosed();
            // if (!isInSearchMode)
            // {
            //     onSwipeLeft?.Invoke();
            //     mainControl.Left(isInQuestionMode);
            // }

            if (targetPosition.x > -Screen.width / 3f)
                targetPosition = new Vector3(-Screen.width / 3f, targetPosition.y, 0);
            OnSwipeEvent?.Invoke(SwipeEventType.SwipeLeft);
        }
        else
        {
            //Right
            var diff = (targetPosition - defaultPosition) * 2.5f;
            targetSize = 2 / 3f;
            targetPosition = transform.position + diff;
            shouldBeDestroyed = true;
            
            // quizManager.AnswerClosed();
            // if (!isInSearchMode)
            // {
            //     mainControl.Right(isInQuestionMode);
            // }

            if (targetPosition.x < Screen.width + Screen.width / 3f)
                targetPosition = new Vector3(Screen.width + Screen.width / 3f, targetPosition.y, 0);
            OnSwipeEvent?.Invoke(SwipeEventType.SwipeRight);
        }
    }

    public void SwipeLeft()
    {
        targetSize = 2 / 3f;
        targetPosition = new Vector3(-1000, -500, 0);
        targetRotation = 10f;
        shouldBeDestroyed = true;
        
        OnSwipeEvent?.Invoke(SwipeEventType.SwipeLeft);
        // quizManager.AnswerClosed();
        // if (!isInSearchMode)
        // {
        //     mainControl.Left(isInQuestionMode);
        // }
    }

    public void SwipeRight()
    {
        targetSize = 2 / 3f;
        targetPosition = new Vector3(2000, -500, 0);
        targetRotation = -10f;
        shouldBeDestroyed = true;
        
        OnSwipeEvent?.Invoke(SwipeEventType.SwipeRight);
        // mainControl.Right(isInQuestionMode);
    }

    private float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}