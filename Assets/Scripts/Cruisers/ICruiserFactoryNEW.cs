using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.BattleScene.Manager;

namespace BattleCruisers.Cruisers
{
    public interface ICruiserFactoryNEW
    {
        ICruiser CreatePlayerCruiser();
        ICruiser CreateAICruiser();

        void InitialisePlayerCruiser(
            IUIManager uiManager,
            ISlotFilter highlightableFilter,
            IBuildProgressCalculator buildProgressCalculator,
            IRankedTargetTracker userChosenTargetTracker);

        void InitialiseAICruiser(
            IUIManager uiManager, 
            ISlotFilter highlightableFilter,
            IBuildProgressCalculator buildProgressCalculator,
            IRankedTargetTracker userChosenTargetTracker,
            IUserChosenTargetHelper userChosenTargetHelper);
    }
}