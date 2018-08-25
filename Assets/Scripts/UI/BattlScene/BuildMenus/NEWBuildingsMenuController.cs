using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
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
            IList<IBuildableWrapper<IBuilding>> buildings,
            IUIManager uiManager,
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter,
            ISpriteProvider spriteProvider)
        {
            base.Initialise(buildings, uiManager, shouldBeEnabledFilter);

            Assert.IsNotNull(spriteProvider);
            _spriteProvider = spriteProvider;
        }

        protected override void InitialiseBuildableButton(BuildingButtonController button, IBuildableWrapper<IBuilding> buildable)
        {
            Sprite slotSprite = _spriteProvider.GetSlotSprite(buildable.Buildable.SlotType).Sprite;
            button.Initialise(buildable, _uiManager, _shouldBeEnabledFilter, slotSprite);
        }
    }
}
