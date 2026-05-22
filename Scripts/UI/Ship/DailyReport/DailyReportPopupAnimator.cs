using DG.Tweening;
using UnityEngine;

public class DailyReportPopupAnimator : MonoBehaviour, IUIPopupAnimator
{
    private RectTransform _reportUI;
    private RectTransform _crewsUI;
    private Vector2 _openPos;
    private Vector2 _closePos;

    public void Initialize(RectTransform crewsUI, Vector2 openPos, Vector2 closePos)
    {
        _reportUI = GetComponent<RectTransform>();
        _crewsUI = crewsUI;
        _openPos = openPos;
        _closePos = closePos;
    }

    public void PlayOpen()
    {
        EnsureInitialized();
        KillTweens();

        _crewsUI.DOAnchorPos(_closePos, 0.3f).SetEase(Ease.InSine).OnComplete(() =>
        {
            _reportUI.DOAnchorPos(_openPos, 1f).SetEase(Ease.OutBack);
        });
    }

    public void PlayClose()
    {
        EnsureInitialized();
        KillTweens();

        _reportUI.DOAnchorPos(_closePos, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            _crewsUI.DOAnchorPos(_openPos, 0.3f).SetEase(Ease.OutSine);
        });
    }

    public void SetClosedImmediate()
    {
        EnsureInitialized();
        KillTweens();

        _reportUI.anchoredPosition = _closePos;
        if (_crewsUI != null)
        {
            _crewsUI.anchoredPosition = _openPos;
        }
    }

    private void EnsureInitialized()
    {
        if (_reportUI == null)
        {
            _reportUI = GetComponent<RectTransform>();
        }
    }

    private void KillTweens()
    {
        if (_reportUI != null)
        {
            _reportUI.DOKill();
        }

        if (_crewsUI != null)
        {
            _crewsUI.DOKill();
        }
    }
}
