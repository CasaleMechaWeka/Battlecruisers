using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public interface IPvPCruiserFactory
    {
        PvPCruiser CreatePlayerCruiser();
        PvPCruiser CreateAICruiser(IPvPPrefabKey aiCruiserKey);

        void InitialisePlayerCruiser(
            PvPCruiser playerCruiser,
            PvPCruiser aiCruiser,
            IPvPCameraFocuser cameraFocuser,
            IPvPRankedTargetTracker userChosenTargetTracker);

        void InitialiseAICruiser(
            PvPCruiser playerCruiser,
            PvPCruiser aiCruiser,
            IPvPCameraFocuser cameraFocuser,
            IPvPRankedTargetTracker userChosenTargetTracker,
            IPvPUserChosenTargetHelper userChosenTargetHelper);
    }
}