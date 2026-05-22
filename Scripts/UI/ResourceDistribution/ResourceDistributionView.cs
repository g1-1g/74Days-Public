using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ResourceDistributionView : MonoBehaviour, IResourceDistributionView
{
    public static ResourceDistributionView Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private RectTransform divisionUI;
    [SerializeField] private Transform crewsParent;
    [SerializeField] private Transform boxElementParent;
    [SerializeField] private GameObject _divisionTable;
    [SerializeField] private GameObject _divisionGuide;
    [SerializeField] private TextMeshProUGUI titleText;

    [SerializeField] private UIPopupController _distributeUI;

    private readonly List<CrewResourceItem> crewItems = new List<CrewResourceItem>();
    private readonly List<InventorySlotUI> inventorySlots = new List<InventorySlotUI>();
    private ResourceDistributionPresenter _presenter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _presenter = new ResourceDistributionPresenter(this);
        _presenter.Initialize();
    }

    private void OnDestroy()
    {
        _presenter?.Dispose();
    }

    public void ShowUI()
    {
        _presenter?.Show();
    }

    public void SetDivisionVisible(bool visible)
    {
        if (_divisionTable != null)
        {
            _divisionTable.SetActive(visible);
        }

        if (_divisionGuide != null)
        {
            _divisionGuide.SetActive(visible);
        }
    }

    public void RenderCrewSlots(IReadOnlyList<CrewMember> crewMembers)
    {
        crewItems.Clear();

        if (crewsParent == null)
        {
            Debug.LogError("[ResourceDistributionView] crewsParent is null.");
            return;
        }

        var existingCrews = crewsParent.GetComponentsInChildren<CrewResourceItem>(true);

        for (int i = 0; i < existingCrews.Length; i++)
        {
            var crew = crewMembers.FirstOrDefault(c => c != null && c.CrewID == i);

            if (crew != null && crew.IsAlive)
            {
                existingCrews[i].gameObject.SetActive(true);
                existingCrews[i].Initialize(crew);
                crewItems.Add(existingCrews[i]);
            }
            else
            {
                existingCrews[i].gameObject.SetActive(false);
            }
        }
    }

    public void RenderInventorySlots(IReadOnlyList<ResourceMetaData> resources)
    {
        inventorySlots.Clear();

        if (boxElementParent == null)
        {
            Debug.LogError("[ResourceDistributionView] boxElementParent is null.");
            return;
        }

        for (int i = 0; i < resources.Count && i < boxElementParent.childCount; i++)
        {
            Transform slotTransform = boxElementParent.GetChild(i);
            var slot = slotTransform.GetComponent<InventorySlotUI>();

            if (slot == null)
            {
                slot = slotTransform.gameObject.AddComponent<InventorySlotUI>();
            }

            ResourceMetaData resourceData = resources[i];
            if (resourceData == null)
                continue;

            slot.Initialize(resourceData.resourceType);
            inventorySlots.Add(slot);
        }
    }

    public void ClearAssignments()
    {
        foreach (var crewItem in crewItems)
        {
            crewItem?.ClearAssignedResources();
        }

        foreach (var slot in inventorySlots)
        {
            slot?.ResetTemporaryReservations();
        }
    }

    public void RefreshResourceSlot(ResourceType type)
    {
        var slot = inventorySlots.Find(s => s.ResourceType == type);
        if (slot != null)
        {
            slot.UpdateAmount();
        }
    }

    public void RefreshAllResourceSlots()
    {
        foreach (var slot in inventorySlots)
        {
            slot.UpdateAmount();
        }
    }

    public InventorySlotUI GetInventorySlot(ResourceType type)
    {
        return inventorySlots.Find(s => s.ResourceType == type);
    }

    public Sprite GetResourceIcon(ResourceType type)
    {
        return _presenter?.GetResourceIcon(type);
    }

    public void OnCompleteButtonClicked()
    {
        _presenter?.CompleteEvening();
    }

    public void CloseDistributionPopup()
    {
        _distributeUI?.Close();
    }
}
