using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers.Sprites;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public class PvPBuildingMenus : PvPBuildableMenus<IPvPBuilding, PvPBuildingCategory, PvPBuildingsMenuController>
    {
        private ISpriteProvider _spriteProvider;
        private IPvPBuildingClickHandler _clickHandler;
        private bool _flipClickAndDragIcon;

        public void Initialise(
            IDictionary<PvPBuildingCategory, IList<IPvPBuildableWrapper<IPvPBuilding>>> buildings,
            IPvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IPvPBuildableSorter<IPvPBuilding> buildingSorter,
            ISpriteProvider spriteProvider,
            ISingleSoundPlayer soundPlayer,
            IPvPBuildingClickHandler clickHandler,
            bool flipClickAndDragIcon)
        {
            // Need these for abstract method called by base.Initialise().  Codesmell :P
            PvPHelper.AssertIsNotNull(spriteProvider, clickHandler);

            _spriteProvider = spriteProvider;
            _clickHandler = clickHandler;
            _flipClickAndDragIcon = flipClickAndDragIcon;

            base.Initialise(buildings, uiManager, buttonVisibilityFilters, buildingSorter, soundPlayer);
        }

        protected override void InitialiseMenu(
            ISingleSoundPlayer soundPlayer,
            PvPBuildingsMenuController menu,
            IPvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IList<IPvPBuildableWrapper<IPvPBuilding>> buildables)
        {
            menu.Initialise(soundPlayer, uiManager, buttonVisibilityFilters, buildables, _spriteProvider, _clickHandler, _flipClickAndDragIcon);
        }
    }
}