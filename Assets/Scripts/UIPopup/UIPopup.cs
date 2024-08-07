using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UIPopup : MonoBehaviour, IUIPopup
{
    public int id = 0;
    public int Id => id;

    private float duration = 0.5f;
    public float Duration => duration;

    public float delayToNextPanel = 3.0f;
    public float DelayToNextPopup
    {
        get => delayToNextPanel;
        set => delayToNextPanel = value;
    }

    public int nextPanelId;
    public int NextPopupId
    {
        get => nextPanelId;
        set => nextPanelId = value;
    }

    public bool enableAutoTransition = false;
    public bool EnableAutoTransition
    {
        get => enableAutoTransition;
        set => enableAutoTransition = value;
    }

    public bool useInstantShow = false;
    public bool UseInstantShow
    {
        get => useInstantShow;
        set => useInstantShow = value;
    }

    public bool hideOnTransition = true;
    public bool HideOnTransition
    {
        get => hideOnTransition;
        set => hideOnTransition = value;
    }

    private Coroutine autoTransitionCoroutine;
    private float remainingTime;

    [Space(10)]
    [SerializeField] private CanvasGroup transparentCanvas;
    [SerializeField] private Transform moveTransform;
    private RectTransform moveRectTransform;
    [SerializeField] private Vector3 moveFrom;
    [SerializeField] private Vector3 moveTo;
    [SerializeField] private Vector3 scaleFrom = Vector3.zero;
    [SerializeField] private Vector3 scaleTo = Vector3.one;
    [SerializeField] private float moveSpeed = 10f;

    public delegate void MovementCompleteHandler(UIPopup panel);
    public delegate void RequestPanelChangeHandler(int panelId);
    public event MovementCompleteHandler OnMovementComplete;
    public event RequestPanelChangeHandler OnRequestPanelChange;

    public UnityEvent onPanelShow;
    public UnityEvent OnPopupShow => onPanelShow;

    public UnityEvent onPanelHide;
    public UnityEvent OnPopupHide => onPanelHide;

    public UnityEvent OnResetAutoTransitionTimer { get; private set; }

    void Awake()
    {
        if (onPanelShow == null)
            onPanelShow = new UnityEvent();

        if (onPanelHide == null)
            onPanelHide = new UnityEvent();

        if (OnResetAutoTransitionTimer == null) // Khởi tạo sự kiện
            OnResetAutoTransitionTimer = new UnityEvent();

        if (moveTransform != null)
        {
            moveRectTransform = moveTransform.GetComponent<RectTransform>();
        }
        if (transparentCanvas == null)
        {
            transparentCanvas = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void OnEnable()
    {
        OnResetAutoTransitionTimer.AddListener(ResetAutoTransitionTimer);
    }

    void OnDisable()
    {
        OnResetAutoTransitionTimer.RemoveListener(ResetAutoTransitionTimer);
    }

    public virtual void MoveToCenter()
    {
        if (useInstantShow)
        {
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }

            transform.localPosition = Vector3.zero;
            OnMovementComplete?.Invoke(this);
            if (enableAutoTransition)
            {
                ResetAutoTransitionTimer();
            }
        }
        else
        {
            Vector3 start = new Vector3(2000, 0, 0);
            Vector3 end = Vector3.zero;
            StartCoroutine(MoveFromTo(start, end, duration));
        }
    }

    protected virtual IEnumerator ShowMove()
    {
        if (moveRectTransform != null)
        {
            float value = 0f;
            moveRectTransform.anchoredPosition = moveFrom;
            moveRectTransform.localScale = scaleFrom;
            transparentCanvas.alpha = 0f;
            while (value < 1f)
            {
                value = Mathf.Lerp(value, 1.1f, Time.deltaTime * moveSpeed);
                moveRectTransform.anchoredPosition = Vector3.Lerp(moveFrom, moveTo, value);
                moveRectTransform.localScale = Vector3.Lerp(scaleFrom, scaleTo, value);
                transparentCanvas.alpha = value;
                yield return null;
            }
            moveRectTransform.anchoredPosition = moveTo;
            moveRectTransform.localScale = scaleTo;
            transparentCanvas.alpha = 1f;
        }

        OnMovementComplete?.Invoke(this);

        if (enableAutoTransition)
        {
            ResetAutoTransitionTimer(); // Bắt đầu quá trình đếm ngược ngay lập tức
        }
    }

    protected virtual IEnumerator HideMove()
    {
        if (moveRectTransform != null)
        {
            float value = 0f;
            moveRectTransform.anchoredPosition = moveTo;
            moveRectTransform.localScale = scaleTo;
            transparentCanvas.alpha = 1f;
            while (value < 1f)
            {
                value = Mathf.Lerp(value, 1.1f, Time.deltaTime * moveSpeed);
                moveRectTransform.anchoredPosition = Vector3.Lerp(moveTo, moveFrom, value);
                moveRectTransform.localScale = Vector3.Lerp(scaleTo, scaleFrom, value);
                transparentCanvas.alpha = 1f - value;
                yield return null;
            }
            moveRectTransform.anchoredPosition = moveFrom;
            moveRectTransform.localScale = scaleFrom;
            transparentCanvas.alpha = 0f;
        }

        gameObject.SetActive(false);
    }

    protected virtual IEnumerator MoveFromTo(Vector3 pointA, Vector3 pointB, float time)
    {
        float elapsedTime = 0;
        transform.localPosition = pointA;

        while (elapsedTime < time)
        {
            float t = elapsedTime / time;
            t = t * t * (3f - 2f * t);
            transform.localPosition = Vector3.Lerp(pointA, pointB, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = pointB;
        OnMovementComplete?.Invoke(this);
        if (enableAutoTransition)
        {
            ResetAutoTransitionTimer();
        }
    }

    protected virtual IEnumerator TransitionToNextPanelAfterDelay()
    {
        remainingTime = delayToNextPanel + 1;
        while (remainingTime > 0)
        {
            remainingTime--;
            yield return new WaitForSeconds(1f);
        }
        OnRequestPanelChange?.Invoke(nextPanelId);
        Hide();
    }

    public virtual void StopAutoTransition()
    {
        if (autoTransitionCoroutine != null)
        {
            StopCoroutine(autoTransitionCoroutine);
            autoTransitionCoroutine = null;
        }
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        onPanelShow.Invoke();

        transform.localPosition = Vector3.zero;

        if (transparentCanvas != null)
            transparentCanvas.alpha = 1f;

        StartCoroutine(ShowMove());
    }

    public virtual void Hide()
    {
        Debug.Log("Hide");
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        if (transparentCanvas != null)
            transparentCanvas.alpha = 1f;
        StartCoroutine(HideMove());
    }

    public virtual void ForceHide()
    {
        onPanelHide.Invoke();
        gameObject.SetActive(false);
    }

    public virtual void InitialHide()
    {
        gameObject.SetActive(false);
    }

    public virtual void ActivateAutoTransition()
    {
        if (enableAutoTransition)
        {
            if (delayToNextPanel > 0)
            {
                remainingTime = delayToNextPanel;
                autoTransitionCoroutine = StartCoroutine(TransitionToNextPanelAfterDelay());
            }
            else
            {
                Debug.LogWarning("delayToNextPanel must greater than 0");
            }
        }
    }

    public virtual float GetRemainingTransitionTime()
    {
        if (enableAutoTransition && autoTransitionCoroutine != null)
        {
            return remainingTime;
        }
        return 0f;
    }

    public virtual void ResetAutoTransitionTimer()
    {
        if (enableAutoTransition)
        {
            StopAutoTransition();
            if (delayToNextPanel > 0)
            {
                remainingTime = delayToNextPanel;
                autoTransitionCoroutine = StartCoroutine(TransitionToNextPanelAfterDelay());
            }
            else
            {
                Debug.LogWarning("delayToNextPanel must greater than 0");
            }
        }
    }
}
