using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers
{
    public class PvPUnitClickHandler : PvPBuildableClickHandler, IPvPUnitClickHandler
    {
        private readonly IPvPPopulationLimitReachedDecider _populationLimitReachedDecider;
        private PvPCruiser _playerCruiser;

        public PvPUnitClickHandler(
            PvPCruiser playerCruiser,
            IPvPUIManager uiManager,
            IPvPPrioritisedSoundPlayer eventSoundPlayer,
            IPvPSingleSoundPlayer uiSoundPlayer,
            IPvPPopulationLimitReachedDecider populationLimitReachedDecider)
            : base(uiManager, eventSoundPlayer, uiSoundPlayer)
        {
            Assert.IsNotNull(populationLimitReachedDecider);
            _populationLimitReachedDecider = populationLimitReachedDecider;
            _playerCruiser = playerCruiser;
        }

        public void HandleClick(bool canAffordBuildable, IPvPBuildableWrapper<IPvPUnit> unitClicked, IPvPFactory unitFactory)
        {
            PvPHelper.AssertIsNotNull(unitClicked, unitFactory);

            _uiSoundPlayer.PlaySound(unitFactory.UnitSelectedSound);
            //_uiManager.ShowUnitDetails(unitClicked.Buildable);

            if (canAffordBuildable)
            {
                _uiManager.ShowUnitDetails(unitClicked.Buildable);//added
                HandleFactory(unitClicked, unitFactory);

                if (_populationLimitReachedDecider.ShouldPlayPopulationLimitReachedWarning(_playerCruiser, unitFactory))
                {
                    _eventSoundPlayer.PlaySound(PvPPrioritisedSoundKeys.PvPEvents.PopulationLimitReached);
                }
            }
            else if (unitFactory.BuildableState == PvPBuildableState.Completed)
            {
                PlayUnaffordableSound();
            }
            else
            {
                _eventSoundPlayer.PlaySound(PvPPrioritisedSoundKeys.PvPEvents.IncompleteFactory);
            }
        }

        private void HandleFactory(IPvPBuildableWrapper<IPvPUnit> unitClicked, IPvPFactory unitFactory)
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