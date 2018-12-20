using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Navigation;

namespace BattleCruisers.Cruisers
{
    public interface ICruiserFactory
    {
        Cruiser CreatePlayerCruiser();
        Cruiser CreateAICruiser();

        void InitialisePlayerCruiser(
            Cruiser playerCruiser, 
            Cruiser aiCruiser, 
            ICameraFocuser cameraFocuser, 
            IRankedTargetTracker userChosenTargetTracker);

        void InitialiseAICruiser(
            Cruiser playerCruiser, 
            Cruiser aiCruiser, 
            ICameraFocuser cameraFocuser,
            IRankedTargetTracker userChosenTargetTracker,
            IUserChosenTargetHelper userChosenTargetHelper);
    }
}