using DG.Tweening;
using UnityEngine;

public class UIPopupSlideAnimator : MonoBehaviour, IUIPopupAnimator
{
    private RectTransform _ui;
    private Vector2 _openPos;
    private Vector2 _closePos;
    private float _openDuration;
    private float _closeDuration;
    private Ease _openEase;
    private Ease _closeEase;

    public void Initialize(Vector2 openPos, Vector2 closePos, float openDuration = 0.5f, float closeDuration = 0.5f)
    {
        _ui = GetComponent<RectTransform>();
        _openPos = openPos;
        _closePos = closePos;
        _openDuration = openDuration;
        _closeDuration = closeDuration;
        _openEase = Ease.OutBack;
        _closeEase = Ease.InBack;
    }

    public void PlayOpen()
    {
        EnsureInitialized();
        _ui.DOKill();
        _ui.DOAnchorPos(_openPos, _openDuration).SetEase(_openEase);
    }

    public void PlayClose()
    {
        EnsureInitialized();
        _ui.DOKill();
        _ui.DOAnchorPos(_closePos, _closeDuration).SetEase(_closeEase);
    }

    public void SetClosedImmediate()
    {
        EnsureInitialized();
        _ui.DOKill();
        _ui.anchoredPosition = _closePos;
    }

    private void EnsureInitialized()
    {
        if (_ui == null)
        {
            _ui = GetComponent<RectTransform>();
        }
    }
}
