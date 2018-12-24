namespace BattleCruisers.UI.Cameras.Targets.Finders
{
    public interface ICornerCutoffProvider
    {
        float PlayerCruiserCornerXPositionCutoff { get; }
        float AICruiserCornerXPositionCutoff { get; }
        float OverviewOrthographicSizeCutoff { get; }
    }
}