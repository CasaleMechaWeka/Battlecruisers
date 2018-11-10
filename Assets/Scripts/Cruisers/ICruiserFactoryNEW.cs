using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.BattleScene.Manager;

namespace BattleCruisers.Cruisers
{
    public interface ICruiserFactoryNEW
    {
        ICruiser CreatePlayerCruiser();
        ICruiser CreateAICruiser();

        void InitialisePlayerCruiser(IUIManager uiManager, IRankedTargetTracker userChosenTargetTracker);

        void InitialiseAICruiser(
            IUIManager uiManager, 
            IRankedTargetTracker userChosenTargetTracker,
            IUserChosenTargetHelper userChosenTargetHelper);
    }
}