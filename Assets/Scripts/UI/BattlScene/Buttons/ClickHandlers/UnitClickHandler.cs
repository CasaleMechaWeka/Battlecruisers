using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    // FELIX  Update tests :)
    public class UnitClickHandler : BuildableClickHandler, IUnitClickHandler
    {
        public UnitClickHandler(IUIManager uiManager, IPrioritisedSoundPlayer soundPlayer)
            : base(uiManager, soundPlayer)
        {
        }

        public void HandleClick(bool canAffordBuildable, IBuildableWrapper<IUnit> unitClicked, IFactory unitFactory)
        {
            Helper.AssertIsNotNull(unitClicked, unitFactory);

            if (canAffordBuildable)
            {
                bool tryingToBuildUnit = HandleFactory(unitClicked, unitFactory);
			    _uiManager.ShowUnitDetails(unitClicked.Buildable);

                if (tryingToBuildUnit)
                {
                    // FELIX  Check for population limit, play sound
                }
            }
            else if (unitFactory.BuildableState == BuildableState.Completed)
            {
                PlayUnaffordableSound();
            }
        }

        /// <returns>
        /// True if trying to build a unit, false otherwise.
        /// </returns>
        private bool HandleFactory(IBuildableWrapper<IUnit> unitClicked, IFactory unitFactory)
        {
            if (ReferenceEquals(unitFactory.UnitWrapper, unitClicked))
            {
                // Same unit
                if (unitFactory.IsUnitPaused.Value)
                {
                    unitFactory.ResumeBuildingUnit();
                    return true;
                }
                else
                {
                    unitFactory.PauseBuildingUnit();
                    return false;
                }
            }
            else
            {
                // Different unit
                unitFactory.StartBuildingUnit(unitClicked);
                return true;
            }
        }
    }
}