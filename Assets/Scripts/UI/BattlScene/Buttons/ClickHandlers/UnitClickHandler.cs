using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    public class UnitClickHandler : BuildableClickHandler, IUnitClickHandler
    {
        private readonly IPopulationLimitReachedDecider _populationLimitReachedDecider;

        public UnitClickHandler(
            IUIManager uiManager, 
            IPrioritisedSoundPlayer eventSoundPlayer, 
            ISingleSoundPlayer uiSoundPlayer,
            IPopulationLimitReachedDecider populationLimitReachedDecider)
            : base(uiManager, eventSoundPlayer, uiSoundPlayer)
        {
            Assert.IsNotNull(populationLimitReachedDecider);
            _populationLimitReachedDecider = populationLimitReachedDecider;
        }

        public void HandleClick(bool canAffordBuildable, IBuildableWrapper<IUnit> unitClicked, IFactory unitFactory)
        {
            Helper.AssertIsNotNull(unitClicked, unitFactory);

            _uiSoundPlayer.PlaySound(unitFactory.UnitSelectedSound);
            // FELIX  Update tests :)
			_uiManager.ShowUnitDetails(unitClicked.Buildable);

            if (canAffordBuildable)
            {
                HandleFactory(unitClicked, unitFactory);

                if (_populationLimitReachedDecider.ShouldPlayPopulationLimitReachedWarning(unitFactory))
                {
                    _eventSoundPlayer.PlaySound(PrioritisedSoundKeys.Events.PopulationLimitReached);
                }
            }
            else if (unitFactory.BuildableState == BuildableState.Completed)
            {
                PlayUnaffordableSound();
            }
            else
            {
                _eventSoundPlayer.PlaySound(PrioritisedSoundKeys.Events.IncompleteFactory);
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