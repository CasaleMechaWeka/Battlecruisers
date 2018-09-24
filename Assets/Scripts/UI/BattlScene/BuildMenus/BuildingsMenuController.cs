using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildingsMenuController : BuildablesMenuController<BuildingButtonController, IBuilding>
	{
        private ISpriteProvider _spriteProvider;
        private IPlayerCruiserFocusHelper _playerCruiserFocusHelper;

        public void Initialise(
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IList<IBuildableWrapper<IBuilding>> buildings,
            ISpriteProvider spriteProvider,
            IPlayerCruiserFocusHelper playerCruiserFocusHelper)
        {
            // Need these for abstract method called by base.Initialise().  Codesmell :P
            Helper.AssertIsNotNull(spriteProvider, playerCruiserFocusHelper);

            _spriteProvider = spriteProvider;
            _playerCruiserFocusHelper = playerCruiserFocusHelper;

            base.Initialise(uiManager, buttonVisibilityFilters, buildings);
        }

        protected override void InitialiseBuildableButton(BuildingButtonController button, IBuildableWrapper<IBuilding> buildableWrapper)
        {
            Sprite slotSprite = _spriteProvider.GetSlotSprite(buildableWrapper.Buildable.SlotType).Sprite;
            button.Initialise(buildableWrapper, _uiManager, _shouldBeEnabledFilter, _playerCruiserFocusHelper, slotSprite);
        }
    }
}
