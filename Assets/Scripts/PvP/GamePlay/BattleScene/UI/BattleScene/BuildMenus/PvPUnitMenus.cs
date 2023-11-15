using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public class PvPUnitMenus : PvPBuildableMenus<IPvPUnit, PvPUnitCategory, PvPUnitsMenuController>
    {
        private IPvPUnitClickHandler _clickHandler;

        public void Initialise(
            IDictionary<PvPUnitCategory, IList<IPvPBuildableWrapper<IPvPUnit>>> buildables,
            IPvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IPvPBuildableSorter<IPvPUnit> buildableSorter,
            IPvPSingleSoundPlayer soundPlayer,
            IPvPUnitClickHandler clickHandler)
        {
            // Need this for abstract method called by base.Initialise().  Codesmell :P
            Assert.IsNotNull(clickHandler);
            _clickHandler = clickHandler;

            base.Initialise(buildables, uiManager, buttonVisibilityFilters, buildableSorter, soundPlayer);
        }

        protected override void InitialiseMenu(
            IPvPSingleSoundPlayer soundPlayer,
            PvPUnitsMenuController menu,
            IPvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IList<IPvPBuildableWrapper<IPvPUnit>> buildables)
        {
            menu.Initialise(soundPlayer, uiManager, buttonVisibilityFilters, buildables, _clickHandler);
        }
    }
}