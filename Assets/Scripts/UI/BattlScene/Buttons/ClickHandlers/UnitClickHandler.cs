using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
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
                HandleFactory(unitClicked, unitFactory);
			    _uiManager.ShowUnitDetails(unitClicked.Buildable);
            }
            else if (unitFactory.BuildableState == BuildableState.Completed)
            {
                PlayUnaffordableSound();
            }
        }

        private void HandleFactory(IBuildableWrapper<IUnit> unitClicked, IFactory unitFactory)
        {
            if (ReferenceEquals(unitFactory.UnitWrapper, unitClicked))
            {
                // Same unit
                if (unitFactory.IsUnitPaused.Value)
                {
                    unitFactory.ResumeBuildingUnit();
                }
                else
                {
                    unitFactory.PauseBuildingUnit();
                }
            }
            else
            {
                // Different unit
                unitFactory.StartBuildingUnit(unitClicked);
            }
        }
    }
}