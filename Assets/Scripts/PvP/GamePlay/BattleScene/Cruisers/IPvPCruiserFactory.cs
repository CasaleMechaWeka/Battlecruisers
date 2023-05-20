using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public interface IPvPCruiserFactory
    {
        PvPCruiser CreatePlayerACruiser();
        PvPCruiser CreatePlayerBCruiser();

        void InitialisePlayerACruiser(
            PvPCruiser playerACruiser,
            PvPCruiser playerBCruiser,
            // IPvPCameraFocuser cameraFocuser,
            IPvPRankedTargetTracker userChosenTargetTracker
           /* IPvPUserChosenTargetHelper userChosenTargetHelper*/ );

        void InitialisePlayerBCruiser(
            PvPCruiser playerBCruiser,
            PvPCruiser playerACruiser,
            // IPvPCameraFocuser cameraFocuser,
            IPvPRankedTargetTracker userChosenTargetTracker,
            IPvPUserChosenTargetHelper userChosenTargetHelper);
    }
}