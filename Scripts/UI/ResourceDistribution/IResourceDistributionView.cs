using System.Collections.Generic;

public interface IResourceDistributionView
{
    void SetDivisionVisible(bool visible);
    void RenderCrewSlots(IReadOnlyList<CrewMember> crewMembers);
    void RenderInventorySlots(IReadOnlyList<ResourceMetaData> resources);
    void ClearAssignments();
    void RefreshResourceSlot(ResourceType type);
    void CloseDistributionPopup();
}
