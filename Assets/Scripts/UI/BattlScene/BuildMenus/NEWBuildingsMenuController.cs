using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class NEWBuildingsMenuController : NEWBuildablesMenuController<BuildingButtonController, IBuilding>
	{
        private ISpriteProvider _spriteProvider;

        public void Initialise(
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IList<IBuildableWrapper<IBuilding>> buildings,
            ISpriteProvider spriteProvider)
        {
            // Need _spriteProvider for abstract method called by base.Initialise().  Codesmell :P
            Assert.IsNotNull(spriteProvider);
            _spriteProvider = spriteProvider;

            base.Initialise(uiManager, buttonVisibilityFilters, buildings);
        }

        protected override void InitialiseBuildableButton(BuildingButtonController button, IBuildableWrapper<IBuilding> buildable)
        {
            Sprite slotSprite = _spriteProvider.GetSlotSprite(buildable.Buildable.SlotType).Sprite;
            button.Initialise(buildable, _uiManager, _shouldBeEnabledFilter, slotSprite);
        }
    }
}
