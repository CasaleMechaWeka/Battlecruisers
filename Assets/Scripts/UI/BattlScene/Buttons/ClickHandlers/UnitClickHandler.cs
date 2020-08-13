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
        private readonly ISingleSoundPlayer _uiSoundPlayer;

        public UnitClickHandler(
            IUIManager uiManager, 
            IPrioritisedSoundPlayer eventSoundPlayer, 
            IPopulationLimitReachedDecider populationLimitReachedDecider,
            ISingleSoundPlayer uiSoundPlayer)
            : base(uiManager, eventSoundPlayer)
        {
            Helper.AssertIsNotNull(populationLimitReachedDecider, uiSoundPlayer);

            _populationLimitReachedDecider = populationLimitReachedDecider;
            _uiSoundPlayer = uiSoundPlayer;
        }

        public void HandleClick(bool canAffordBuildable, IBuildableWrapper<IUnit> unitClicked, IFactory unitFactory)
        {
            Helper.AssertIsNotNull(unitClicked, unitFactory);

            // FELIX  Update tests :)
            _uiSoundPlayer.PlaySound(unitFactory.UnitSelectedSound);

            if (canAffordBuildable)
            {
                HandleFactory(unitClicked, unitFactory);
			    _uiManager.ShowUnitDetails(unitClicked.Buildable);

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