using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;

namespace BattleCruisers.Cruisers
{
    public interface ICruiserFactory
    {
        void InitialisePlayerCruiser(
            IUIManager uiManager,
            ICruiserHelper helper,
            ISlotFilter highlightableFilter,
            IBuildProgressCalculator buildProgressCalculator,
            IRankedTargetTracker userChosenTargetTracker);

        void InitialiseAICruiser(
            IUIManager uiManager, 
            ICruiserHelper helper,
            ISlotFilter highlightableFilter,
            IBuildProgressCalculator buildProgressCalculator,
            IRankedTargetTracker userChosenTargetTracker,
            IUserChosenTargetHelper userChosenTargetHelper);
        
		ICruiserHelper CreatePlayerHelper(IUIManager uiManager, ICameraController camera);
		ICruiserHelper CreateAIHelper(IUIManager uiIManager, ICameraController camera);
    }
}