using UnityEngine;

public class UIPopupController : MonoBehaviour, IPopupUI
{
    private IUIPopupAnimator _animator;

    [SerializeField]
    private Vector2 _openPos = new Vector2(0, 50);

    [SerializeField]
    private Vector2 _closePos = new Vector2(0, -900);
    public Vector2 ClosePos => _closePos;

    [SerializeField] private bool _isOpenDefault = false;

    private bool _isOpen = false;
    public bool IsOpen => _isOpen;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip _closeSound;

    private void Start()
    {
        _animator = GetOrCreateAnimator();

        if (_isOpenDefault)
        {
            RegisterDefaultOpenState();
            if (!_isOpen)
            {
                _animator.SetClosedImmediate();
            }
            return;
        }

        _animator.SetClosedImmediate();
    }

    public void Open()
    {
        if (_isOpen) return;

        if (PopupManager.Instance == null)
        {
            Debug.LogWarning("[UIPopupController] PopupManager does not exist. Open failed.");
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

    private void RegisterDefaultOpenState()
    {
        if (PopupManager.Instance == null)
        {
            _isOpen = true;
            return;
        }

        _isOpen = PopupManager.Instance.RegisterOpen(this);
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
        if (animator is UIPopupSlideAnimator slideAnimator)
        {
            slideAnimator.Initialize(_openPos, _closePos);
            return slideAnimator;
        }

        if (animator != null)
        {
            return animator;
        }

        UIPopupSlideAnimator newSlideAnimator = gameObject.AddComponent<UIPopupSlideAnimator>();
        newSlideAnimator.Initialize(_openPos, _closePos);
        return newSlideAnimator;
    }

    private void OnDestroy()
    {
        CloseInternal();
    }
}
