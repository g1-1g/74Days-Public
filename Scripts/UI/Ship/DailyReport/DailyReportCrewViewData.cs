using UnityEngine;

public class DailyReportCrewViewData
{
    public string NameText { get; }
    public string CommentText { get; }
    public float Hunger { get; }
    public float Thirst { get; }
    public float Temperature { get; }
    public Sprite CrewSprite { get; }

    public DailyReportCrewViewData(
        string nameText,
        string commentText,
        float hunger,
        float thirst,
        float temperature,
        Sprite crewSprite)
    {
        NameText = nameText;
        CommentText = commentText;
        Hunger = hunger;
        Thirst = thirst;
        Temperature = temperature;
        CrewSprite = crewSprite;
    }
}
