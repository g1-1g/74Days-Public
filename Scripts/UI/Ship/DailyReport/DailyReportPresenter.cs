using System.Collections.Generic;
using UnityEngine;

public class DailyReportPresenter
{
    private readonly IDailyReportView _view;

    public DailyReportPresenter(IDailyReportView view)
    {
        _view = view;
    }

    public void Initialize()
    {
        if (DayManager.Instance != null)
        {
            DayManager.Instance.OnDayStart += OnDayStart;
            DayManager.Instance.OnPhaseChange += OnPhaseChanged;
        }
        else
        {
            Debug.LogError("[DailyReportPresenter] DayManager.Instance is null.");
        }
    }

    public void Dispose()
    {
        if (DayManager.Instance != null)
        {
            DayManager.Instance.OnDayStart -= OnDayStart;
            DayManager.Instance.OnPhaseChange -= OnPhaseChanged;
        }
    }

    public void Refresh()
    {
        string dayTitle = DayManager.Instance != null
            ? $"Day {DayManager.Instance.CurrentDay} Report"
            : "Day ? Report";

        var crewReports = new List<DailyReportCrewViewData>();

        if (CrewManager.Instance == null)
        {
            Debug.LogWarning("[DailyReportPresenter] CrewManager.Instance is null.");
            _view.Render(new DailyReportViewData(dayTitle, crewReports));
            return;
        }

        foreach (var crew in CrewManager.Instance.CrewMembers)
        {
            if (crew == null)
                continue;

            crewReports.Add(CreateCrewViewData(crew));
        }

        _view.Render(new DailyReportViewData(dayTitle, crewReports));
    }

    private DailyReportCrewViewData CreateCrewViewData(CrewMember crew)
    {
        string nameText = crew.IsAlive ? $"{crew.CrewName} : " : $"{crew.CrewName} (사망)";
        Sprite sprite = crew.IsAlive ? crew.AliveSprite : crew.DeadSprite;

        return new DailyReportCrewViewData(
            nameText,
            GetStatusComment(crew),
            crew.Hunger,
            crew.Thirst,
            crew.Temperature,
            sprite);
    }

    private string GetStatusComment(CrewMember crew)
    {
        if (crew == null)
            return "";

        if (System.Type.GetType("CrewDialogues") != null)
        {
            return CrewDialogues.GetRandomDialogue(crew);
        }

        return "";
    }

    private void OnDayStart(int day)
    {
        Refresh();
    }

    private void OnPhaseChanged(DayPhase phase)
    {
        if (phase == DayPhase.Morning || phase == DayPhase.Evening)
        {
            Refresh();
        }
    }
}
