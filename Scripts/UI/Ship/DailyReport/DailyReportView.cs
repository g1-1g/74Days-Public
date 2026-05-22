using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyReportView : MonoBehaviour, IDailyReportView
{
    public static DailyReportView Instance { get; private set; }

    [Header("Day Info")]
    [SerializeField] private TextMeshProUGUI _dayReportText;

    [Header("Crew Report Items")]
    [SerializeField] private CrewReportItem[] _crewReportItems = new CrewReportItem[3];

    [Header("Panel")]
    [SerializeField] private GameObject _panelRoot;

    private DailyReportPresenter _presenter;

    [System.Serializable]
    public class CrewReportItem
    {
        public GameObject crewObject;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI mentText;
        public SliderUpdate stateSliders;
        public Image crewImage;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _presenter = new DailyReportPresenter(this);
        _presenter.Initialize();
    }

    private void Start()
    {
        _presenter?.Refresh();
    }

    private void OnDestroy()
    {
        _presenter?.Dispose();
    }

    public void Render(DailyReportViewData model)
    {
        if (_dayReportText != null)
        {
            _dayReportText.text = model.DayTitle;
        }

        for (int i = 0; i < _crewReportItems.Length; i++)
        {
            CrewReportItem item = _crewReportItems[i];
            if (item.crewObject == null)
                continue;

            if (i < model.Crews.Count)
            {
                item.crewObject.SetActive(true);
                RenderCrewReportItem(item, model.Crews[i]);
            }
            else
            {
                item.crewObject.SetActive(false);
            }
        }
    }

    public void UpdateAllInfo()
    {
        _presenter?.Refresh();
    }

    [ContextMenu("Test - Update Report")]
    public void TestUpdate()
    {
        UpdateAllInfo();
    }

    private void RenderCrewReportItem(CrewReportItem item, DailyReportCrewViewData crew)
    {
        if (item.nameText != null)
        {
            item.nameText.text = crew.NameText;
        }

        if (item.mentText != null)
        {
            item.mentText.text = crew.CommentText;
        }

        if (item.stateSliders != null)
        {
            item.stateSliders.HpSliderUpdate(crew.Hunger);
            item.stateSliders.WaterSliderUpdate(crew.Thirst);
            item.stateSliders.WarmSliderUpdate(crew.Temperature);
        }

        if (item.crewImage != null)
        {
            item.crewImage.sprite = crew.CrewSprite;
        }
    }
}
