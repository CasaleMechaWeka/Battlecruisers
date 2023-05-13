using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Sprites;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public class PvPBuildingMenus : PvPBuildableMenus<IPvPBuilding, PvPBuildingCategory, PvPBuildingsMenuController>
    {
        private IPvPSpriteProvider _spriteProvider;
        private IPvPBuildingClickHandler _clickHandler;

        public void Initialise(
            IDictionary<PvPBuildingCategory, IList<IPvPBuildableWrapper<IPvPBuilding>>> buildings,
            IPvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IPvPBuildableSorter<IPvPBuilding> buildingSorter,
            IPvPSpriteProvider spriteProvider,
            IPvPSingleSoundPlayer soundPlayer,
            IPvPBuildingClickHandler clickHandler)
        {
            // Need these for abstract method called by base.Initialise().  Codesmell :P
            PvPHelper.AssertIsNotNull(spriteProvider, clickHandler);

            _spriteProvider = spriteProvider;
            _clickHandler = clickHandler;

            base.Initialise(buildings, uiManager, buttonVisibilityFilters, buildingSorter, soundPlayer);
        }

        protected override void InitialiseMenu(
            IPvPSingleSoundPlayer soundPlayer,
            PvPBuildingsMenuController menu,
            IPvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IList<IPvPBuildableWrapper<IPvPBuilding>> buildables)
        {
            menu.Initialise(soundPlayer, uiManager, buttonVisibilityFilters, buildables, _spriteProvider, _clickHandler);
        }
    }
}