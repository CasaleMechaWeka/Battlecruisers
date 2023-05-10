using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public interface IPvPCameraTargets
    {
        IPvPCameraTarget PlayerCruiserTarget { get; }
        IPvPCameraTarget PlayerCruiserDeathTarget { get; }
        IPvPCameraTarget PlayerCruiserNukedTarget { get; }
        IPvPCameraTarget PlayerNavalFactoryTarget { get; }

        IPvPCameraTarget AICruiserTarget { get; }
        IPvPCameraTarget AICruiserDeathTarget { get; }
        IPvPCameraTarget AICruiserNukedTarget { get; }
        IPvPCameraTarget AINavalFactoryTarget { get; }

        IPvPCameraTarget MidLeftTarget { get; }
        IPvPCameraTarget OverviewTarget { get; }
    }
}