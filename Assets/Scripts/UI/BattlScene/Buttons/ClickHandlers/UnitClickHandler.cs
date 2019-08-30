using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    // FELIX  Update tests :)
    public class UnitClickHandler : BuildableClickHandler, IUnitClickHandler
    {
        private readonly IPopulationLimitMonitor _populationLimitMonitor;

        public UnitClickHandler(IUIManager uiManager, IPrioritisedSoundPlayer soundPlayer, IPopulationLimitMonitor populationLimitMonitor)
            : base(uiManager, soundPlayer)
        {
            Assert.IsNotNull(populationLimitMonitor);
            _populationLimitMonitor = populationLimitMonitor;
        }

        public void HandleClick(bool canAffordBuildable, IBuildableWrapper<IUnit> unitClicked, IFactory unitFactory)
        {
            Helper.AssertIsNotNull(unitClicked, unitFactory);

            if (canAffordBuildable)
            {
                HandleFactory(unitClicked, unitFactory);
			    _uiManager.ShowUnitDetails(unitClicked.Buildable);

                bool tryingToBuildUnit
                    = unitFactory.UnitUnderConstruction == null
                        && !unitFactory.IsUnitPaused.Value;

                if (tryingToBuildUnit
                    && _populationLimitMonitor.IsPopulationLimitReached)
                {
                    _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.PopulationLimitReached);
                }
            }
            else if (unitFactory.BuildableState == BuildableState.Completed)
            {
                PlayUnaffordableSound();
            }
            else
            {
                _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.IncompleteFactory);
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