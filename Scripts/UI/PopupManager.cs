using System.Collections.Generic;
using UnityEngine;

public interface IPopupUI
{
    bool IsOpen { get; }
    void Open();
    void Close();
}

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    private readonly List<IPopupUI> _openedPopups = new List<IPopupUI>();

    public bool IsOpened
    {
        get
        {
            CleanDestroyedPopups();
            return _openedPopups.Count > 0;
        }
        set
        {
            if (!value)
            {
                _openedPopups.Clear();
                return;
            }

            Debug.LogWarning("[PopupManager] IsOpened direct set is kept only for legacy code. Use RegisterOpen/RegisterClosed instead.");
        }
    }

    public int OpenedPopupCount
    {
        get
        {
            CleanDestroyedPopups();
            return _openedPopups.Count;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool RegisterOpen(IPopupUI popup)
    {
        if (popup == null) return false;

        CleanDestroyedPopups();

        bool isAlreadyOpen = _openedPopups.Contains(popup);

        if (!isAlreadyOpen)
        {
            CloseOtherPopups(popup);

            _openedPopups.Add(popup);
        }

        return true;
    }

    public void RegisterClosed(IPopupUI popup)
    {
        if (popup == null) return;

        _openedPopups.Remove(popup);
    }

    public bool IsPopupOpen(IPopupUI popup)
    {
        if (popup == null) return false;

        CleanDestroyedPopups();
        return _openedPopups.Contains(popup);
    }

    private void CleanDestroyedPopups()
    {
        _openedPopups.RemoveAll(IsDestroyed);
    }

    private void CloseOtherPopups(IPopupUI popupToKeep)
    {
        for (int i = _openedPopups.Count - 1; i >= 0; i--)
        {
            IPopupUI openedPopup = _openedPopups[i];
            if (openedPopup == null || openedPopup == popupToKeep)
                continue;

            openedPopup.Close();
        }

        CleanDestroyedPopups();
    }

    private bool IsDestroyed(IPopupUI popup)
    {
        return popup == null || popup is Object unityObject && unityObject == null;
    }
}
