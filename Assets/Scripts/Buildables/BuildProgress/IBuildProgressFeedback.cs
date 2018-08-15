namespace BattleCruisers.Buildables.BuildProgress
{
    public interface IBuildProgressFeedback
    {
        void ShowBuildProgress(IBuildable buildable);
        void HideBuildProgress();
    }
}