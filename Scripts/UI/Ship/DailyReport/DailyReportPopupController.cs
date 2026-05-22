using UnityEngine;

public class DailyReportPopupController : MonoBehaviour, IPopupUI
{
    private IUIPopupAnimator _animator;

    [SerializeField]
    private RectTransform _crewsUI;

    private Vector2 _openPos = new Vector2(0, 0);
    private Vector2 _closePos = new Vector2(0, -1200);

    private bool _isOpen = false;
    public bool IsOpen => _isOpen;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip _closeSound;

    private void Start()
    {
        _animator = GetOrCreateAnimator();
        _animator.SetClosedImmediate();

        if (DayManager.Instance != null)
        {
            DayManager.Instance.OnPhaseChange += OnPhaseChanged;
        }
    }

    private void OnDestroy()
    {
        if (DayManager.Instance != null)
        {
            DayManager.Instance.OnPhaseChange -= OnPhaseChanged;
        }

        CloseInternal();
    }

    private void OnPhaseChanged(DayPhase phase)
    {
        if (DayManager.Instance.CurrentDay != 1 && phase == DayPhase.Morning)
        {
            Open();
            Debug.Log("[DailyReportPopupController] Morning phase started. Daily report opened automatically.");
        }
    }

    public void Open()
    {
        if (_isOpen) return;

        if (PopupManager.Instance == null)
        {
            Debug.LogWarning("[DailyReportPopupController] PopupManager does not exist. Open failed.");
            return;
        }

        if (!PopupManager.Instance.RegisterOpen(this))
        {
            return;
        }

        _isOpen = true;
        _animator.PlayOpen();
    }

    public void Close()
    {
        if (!_isOpen) return;

        _isOpen = false;
        PopupManager.Instance?.RegisterClosed(this);

        if (_closeSound != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(_closeSound);
        }

        _animator.PlayClose();
    }

    private void CloseInternal()
    {
        if (!_isOpen) return;

        _isOpen = false;
        PopupManager.Instance?.RegisterClosed(this);
        _animator?.SetClosedImmediate();
    }

    private IUIPopupAnimator GetOrCreateAnimator()
    {
        IUIPopupAnimator animator = GetComponent<IUIPopupAnimator>();
        if (animator is DailyReportPopupAnimator reportAnimator)
        {
            reportAnimator.Initialize(_crewsUI, _openPos, _closePos);
            return reportAnimator;
        }

        if (animator != null)
        {
            return animator;
        }

        DailyReportPopupAnimator newReportAnimator = gameObject.AddComponent<DailyReportPopupAnimator>();
        newReportAnimator.Initialize(_crewsUI, _openPos, _closePos);
        return newReportAnimator;
    }
}
