using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Sound.Players;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers
{
    public class PvPUnitClickHandler : PvPBuildableClickHandler, IPvPUnitClickHandler
    {
        private readonly IPvPPopulationLimitReachedDecider _populationLimitReachedDecider;
        private PvPCruiser _playerCruiser;

        public PvPUnitClickHandler(
            PvPCruiser playerCruiser,
            PvPUIManager uiManager,
            IPrioritisedSoundPlayer eventSoundPlayer,
            ISingleSoundPlayer uiSoundPlayer,
            IPvPPopulationLimitReachedDecider populationLimitReachedDecider)
            : base(uiManager, eventSoundPlayer, uiSoundPlayer)
        {
            Assert.IsNotNull(populationLimitReachedDecider);
            _populationLimitReachedDecider = populationLimitReachedDecider;
            _playerCruiser = playerCruiser;
        }

        public async void HandleClick(bool canAffordBuildable, IPvPBuildableWrapper<IPvPUnit> unitClicked, IPvPFactory unitFactory)
        {
            PvPHelper.AssertIsNotNull(unitClicked, unitFactory);

            _uiSoundPlayer.PlaySound(unitFactory.UnitSelectedSound);
            _uiManager.ShowUnitDetails(unitClicked.Buildable);

            if (canAffordBuildable)
            {
                //   _uiManager.ShowUnitDetails(unitClicked.Buildable);//added
                int variantIndex = await DataProvider.GameModel.PlayerLoadout.GetSelectedUnitVariantIndex(unitClicked.Buildable);
                HandleFactory(unitClicked, unitFactory, variantIndex);

                if (_populationLimitReachedDecider.ShouldPlayPopulationLimitReachedWarning(_playerCruiser, unitFactory))
                {
                    _eventSoundPlayer.PlaySound(PrioritisedSoundKeys.Events.PopulationLimitReached);
                }
            }
            else if (unitFactory.BuildableState == PvPBuildableState.Completed)
            {
                _uiManager.HideSlotsIfCannotAffordable();
                PlayUnaffordableSound();
            }
            else
            {
                _uiManager.HideSlotsIfCannotAffordable();
                _eventSoundPlayer.PlaySound(PrioritisedSoundKeys.Events.IncompleteFactory);
            }
        }

        private void HandleFactory(IPvPBuildableWrapper<IPvPUnit> unitClicked, IPvPFactory unitFactory, int variantIndex)
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
                unitFactory.StartBuildingUnit(unitClicked, variantIndex);
            }
        }

        public void HandleHover(IPvPBuildableWrapper<IPvPUnit> unitClicked)
        {
            _uiManager.PeakUnitDetails(unitClicked.Buildable);
        }

        public void HandleHoverExit()
        {
            _uiManager.UnpeakUnitDetails();
        }
    }
}