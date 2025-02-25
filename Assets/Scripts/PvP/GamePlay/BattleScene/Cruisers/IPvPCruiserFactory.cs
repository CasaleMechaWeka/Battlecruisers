using BattleCruisers.Targets.TargetTrackers;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public interface IPvPCruiserFactory
    {
        Task<PvPCruiser> CreatePlayerACruiser(Team team);
        Task<PvPCruiser> CreatePlayerBCruiser(Team team);
        Task<PvPCruiser> CreateAIBotCruiser(Team team);

        void InitialisePlayerACruiser(
            PvPCruiser playerACruiser,
            PvPCruiser playerBCruiser,
            // IPvPCameraFocuser cameraFocuser,
            IRankedTargetTracker userChosenTargetTracker
           /* IPvPUserChosenTargetHelper userChosenTargetHelper*/ );

        void InitialisePlayerBCruiser(
            PvPCruiser playerBCruiser,
            PvPCruiser playerACruiser,
            //  IPvPCameraFocuser cameraFocuser,
            IRankedTargetTracker userChosenTargetTracker
            /*IPvPUserChosenTargetHelper userChosenTargetHelper*/);
    }
}