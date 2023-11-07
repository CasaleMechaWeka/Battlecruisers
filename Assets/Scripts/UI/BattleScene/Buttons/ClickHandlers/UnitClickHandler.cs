using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Players;
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
            //_uiManager.ShowUnitDetails(unitClicked.Buildable);

            if (canAffordBuildable)
            {
                _uiManager.ShowUnitDetails(unitClicked.Buildable);//added
                //   CheckIfVariant(unitClicked.Buildable);
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


        private async void CheckIfVariant(IUnit unit)
        {
            int index = await ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.PlayerLoadout.GetSelectedUnitVariantIndex(BattleSceneGod.Instance.factoryProvider.PrefabFactory, unit);
            unit.variantIndex = index;
            _uiManager.ShowUnitDetails(unit);
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

        public void HandleHover(IBuildableWrapper<IUnit> unitClicked)
        {
            _uiManager.PeakUnitDetails(unitClicked.Buildable);
        }

        public void HandleHoverExit()
        {
            _uiManager.UnpeakUnitDetails();
        }
    }
}