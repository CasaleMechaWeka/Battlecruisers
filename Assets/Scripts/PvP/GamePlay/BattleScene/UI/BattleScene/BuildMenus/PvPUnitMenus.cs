using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting;
using BattleCruisers.UI.Sound.Players;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public class PvPUnitMenus : PvPBuildableMenus<IPvPUnit, UnitCategory, PvPUnitsMenuController>
    {
        private PvPUnitClickHandler _clickHandler;

        public void Initialise(
            IDictionary<UnitCategory, IList<IPvPBuildableWrapper<IPvPUnit>>> buildables,
            PvPUIManager uiManager,
            PvPButtonVisibilityFilters buttonVisibilityFilters,
            IPvPBuildableSorter<IPvPUnit> buildableSorter,
            SingleSoundPlayer soundPlayer,
            PvPUnitClickHandler clickHandler)
        {
            // Need this for abstract method called by base.Initialise().  Codesmell :P
            Assert.IsNotNull(clickHandler);
            _clickHandler = clickHandler;

            base.Initialise(buildables, uiManager, buttonVisibilityFilters, buildableSorter, soundPlayer);
        }

        protected override void InitialiseMenu(
            SingleSoundPlayer soundPlayer,
            PvPUnitsMenuController menu,
            PvPUIManager uiManager,
            PvPButtonVisibilityFilters buttonVisibilityFilters,
            IList<IPvPBuildableWrapper<IPvPUnit>> buildables)
        {
            menu.Initialise(soundPlayer, uiManager, buttonVisibilityFilters, buildables, _clickHandler);
        }
    }
}