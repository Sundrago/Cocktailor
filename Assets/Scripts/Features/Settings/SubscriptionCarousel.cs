using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SubscriptionCarousel : MonoBehaviour
{
    [SerializeField] private GameObject scrollbar;
    [SerializeField] private Image[] previewImage;
    private float beginScrollValue, endScrollValue;
    private int currentIndex;
    private bool isAnimationOn;
    private bool isDragStarted;
    private float[] positions;

    private Scrollbar scrollbarComponent;
    private float scrollbarValue;
    private float targetPos;

    private void Start()
    {
        scrollbarComponent = scrollbar.GetComponent<Scrollbar>();
        HighlightImage(0);
    }

    private void Update()
    {
        positions = CalculatePositions();

        var distance = 1f / (positions.Length - 1);

        if (Input.GetMouseButton(0))
        {
            scrollbarValue = scrollbarComponent.value;
            HandleMouseDrag();
        }
        else
        {
            ProcessEndDrag(distance);
        }

        UpdateTransform(distance);
    }

    public void HighlightImage(int idx)
    {
        currentIndex = idx;
        for (var i = 0; i < previewImage.Length; i++)
            if (i != idx) previewImage[i].DOFade(0.3f, 0.5f);
            else previewImage[i].DOFade(1f, 0.5f);
    }

    private float[] CalculatePositions()
    {
        return Enumerable.Range(0, previewImage.Length).Select(i => 1f / (previewImage.Length - 1) * i).ToArray();
    }

    private void HandleMouseDrag()
    {
        if (!isDragStarted)
        {
            beginScrollValue = scrollbarComponent.value;
            isDragStarted = true;
        }
    }

    private void ProcessEndDrag(float distance)
    {
        if (isDragStarted)
        {
            endScrollValue = scrollbarComponent.value;
            isDragStarted = false;
            isAnimationOn = true;

            var dist = beginScrollValue - endScrollValue;
            if (Mathf.Abs(dist) < distance / 2f)
            {
                if (dist < 0) currentIndex += 1;
                else currentIndex -= 1;

                currentIndex = Mathf.Min(transform.childCount - 1, currentIndex);
                currentIndex = Mathf.Max(0, currentIndex);
            }
            else
            {
                for (var i = 0; i < positions.Length; i++)
                    if (scrollbarValue < positions[i] + distance / 2 && scrollbarValue > positions[i] - distance / 2)
                        currentIndex = i;
            }
        }

        HighlightImage(currentIndex);
        scrollbarComponent.value = Mathf.Lerp(scrollbarComponent.value, positions[currentIndex], 0.1f);
    }

    private void UpdateTransform(float distance)
    {
        if (!isAnimationOn) return;
        scrollbarValue = scrollbarComponent.value;

        for (var i = 0; i < positions.Length; i++)
            if (scrollbarValue < positions[i] + distance / 2 && scrollbarValue > positions[i] - distance / 2)
                SetChildTransform(i);
    }

    private void SetChildTransform(int i)
    {
        transform.GetChild(i).localScale =
            Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
        for (var j = 0; j < positions.Length; j++)
            if (j != i)
                transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale,
                    new Vector2(0.8f, 0.8f), 0.1f);
    }
}