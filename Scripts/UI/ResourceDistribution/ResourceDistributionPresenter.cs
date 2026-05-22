using UnityEngine;

public class ResourceDistributionPresenter
{
    private readonly IResourceDistributionView _view;

    public ResourceDistributionPresenter(IResourceDistributionView view)
    {
        _view = view;
    }

    public void Initialize()
    {
        _view.SetDivisionVisible(false);

        if (DayManager.Instance != null)
        {
            DayManager.Instance.OnPhaseChange += OnPhaseChanged;
        }

        if (ShipManager.Instance != null)
        {
            ShipManager.Instance.OnResourceChanged += OnResourceChanged;
        }

        Debug.Log("[ResourceDistributionPresenter] Initialized.");
    }

    public void Dispose()
    {
        if (DayManager.Instance != null)
        {
            DayManager.Instance.OnPhaseChange -= OnPhaseChanged;
        }

        if (ShipManager.Instance != null)
        {
            ShipManager.Instance.OnResourceChanged -= OnResourceChanged;
        }
    }

    public void Show()
    {
        if (CrewManager.Instance == null)
        {
            Debug.LogError("[ResourceDistributionPresenter] CrewManager.Instance is null.");
            return;
        }

        if (ResourceDatabaseManager.Instance == null || ResourceDatabaseManager.Instance.Database == null)
        {
            Debug.LogError("[ResourceDistributionPresenter] ResourceDatabaseManager is missing.");
            return;
        }

        _view.RenderCrewSlots(CrewManager.Instance.CrewMembers);
        _view.RenderInventorySlots(ResourceDatabaseManager.Instance.Database.allResources);
        _view.ClearAssignments();
    }

    public Sprite GetResourceIcon(ResourceType type)
    {
        if (ResourceDatabaseManager.Instance == null || ResourceDatabaseManager.Instance.Database == null)
        {
            Debug.LogWarning("[ResourceDistributionPresenter] ResourceDatabaseManager is missing.");
            return null;
        }

        return ResourceDatabaseManager.Instance.Database.GetIcon(type);
    }

    public void CompleteEvening()
    {
        _view.CloseDistributionPopup();

        if (FadeManager.Instance == null)
            return;

        FadeManager.Instance.FadeOutToBlack(2.5f, () =>
        {
            if (DayManager.Instance == null)
                return;

            DayManager.Instance.CompleteEvening();

            if (!DayManager.Instance.IsGameOver)
            {
                FadeManager.Instance.FadeIn(3f);
            }
        });
    }

    private void OnResourceChanged(ResourceType type, int amount)
    {
        _view.RefreshResourceSlot(type);
    }

    private void OnPhaseChanged(DayPhase phase)
    {
        bool isEvening = phase == DayPhase.Evening;
        _view.SetDivisionVisible(isEvening);

        if (isEvening)
        {
            Show();
        }
    }
}
