using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    public interface IPvPBuildProgressFeedback
    {
        void ShowBuildProgress(IPvPBuildable buildable, IPvPFactory buildableFactory);
        void HideBuildProgress();
    }
}