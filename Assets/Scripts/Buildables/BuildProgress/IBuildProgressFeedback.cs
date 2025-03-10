using BattleCruisers.Buildables.Buildings.Factories;

namespace BattleCruisers.Buildables.BuildProgress
{
    public interface IBuildProgressFeedback
    {
        void ShowBuildProgress(IBuildable buildable, IFactory buildableFactory);
        void HideBuildProgress();
    }
}