using BattleCruisers.Targets.TargetTrackers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public interface IPvPCruiserFactory
    {
        PvPCruiser CreatePlayerACruiser(Team team);
        PvPCruiser CreatePlayerBCruiser(Team team);
        PvPCruiser CreateAIBotCruiser(Team team);

        void InitialisePlayerACruiser(
            PvPCruiser playerACruiser,
            PvPCruiser playerBCruiser,
            // ICameraFocuser cameraFocuser,
            IRankedTargetTracker userChosenTargetTracker
           /* IUserChosenTargetHelper userChosenTargetHelper*/ );

        void InitialisePlayerBCruiser(
            PvPCruiser playerBCruiser,
            PvPCruiser playerACruiser,
            //  ICameraFocuser cameraFocuser,
            IRankedTargetTracker userChosenTargetTracker
            /*IUserChosenTargetHelper userChosenTargetHelper*/);
    }
}