using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting;
using BattleCruisers.UI.Sound.Players;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public class PvPBuildingMenus : PvPBuildableMenus<IPvPBuilding, BuildingCategory, PvPBuildingsMenuController>
    {
        private IPvPBuildingClickHandler _clickHandler;
        private bool _flipClickAndDragIcon;

        public void Initialise(
            IDictionary<BuildingCategory, IList<IPvPBuildableWrapper<IPvPBuilding>>> buildings,
            PvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IPvPBuildableSorter<IPvPBuilding> buildingSorter,
            ISingleSoundPlayer soundPlayer,
            IPvPBuildingClickHandler clickHandler,
            bool flipClickAndDragIcon)
        {
            // Need these for abstract method called by base.Initialise().  Codesmell :P
            PvPHelper.AssertIsNotNull(clickHandler);

            _clickHandler = clickHandler;
            _flipClickAndDragIcon = flipClickAndDragIcon;

            base.Initialise(buildings, uiManager, buttonVisibilityFilters, buildingSorter, soundPlayer);
        }

        protected override void InitialiseMenu(
            ISingleSoundPlayer soundPlayer,
            PvPBuildingsMenuController menu,
            PvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IList<IPvPBuildableWrapper<IPvPBuilding>> buildables)
        {
            menu.Initialise(soundPlayer, uiManager, buttonVisibilityFilters, buildables, _clickHandler, _flipClickAndDragIcon);
        }
    }
}