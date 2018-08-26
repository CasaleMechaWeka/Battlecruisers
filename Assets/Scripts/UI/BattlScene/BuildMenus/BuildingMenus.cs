using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildingMenus : BuildableMenus<IBuilding, BuildingCategory, NEWBuildingsMenuController>
    {
        private ISpriteProvider _spriteProvider;

        public void Initialise(
            IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> buildings,
            IUIManager uiManager,
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter,
            IBuildableSorter<IBuilding> buildingSorter,
            ISpriteProvider spriteProvider)
        {
            // Need spriteProvider in abstract method called from parent class Initialise().  Codesmell :(
            Assert.IsNotNull(spriteProvider);
            _spriteProvider = spriteProvider;

            base.Initialise(buildings, uiManager, shouldBeEnabledFilter, buildingSorter);
        }

        protected override void InitialiseMenu(
            NEWBuildingsMenuController menu, 
            IList<IBuildableWrapper<IBuilding>> buildables, 
            IUIManager uiManager, 
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter)
        {
            menu.Initialise(buildables, uiManager, shouldBeEnabledFilter, _spriteProvider);
        }
    }
}