using System.Collections.Generic;

public class DailyReportViewData
{
    public string DayTitle { get; }
    public IReadOnlyList<DailyReportCrewViewData> Crews { get; }

    public DailyReportViewData(string dayTitle, IReadOnlyList<DailyReportCrewViewData> crews)
    {
        DayTitle = dayTitle;
        Crews = crews;
    }
}
