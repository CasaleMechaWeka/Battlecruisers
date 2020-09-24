using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System.Collections.Generic;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildingsMenuController : BuildablesMenuController<BuildingButtonController, IBuilding>
	{
        private ISpriteProvider _spriteProvider;
        private IBuildingClickHandler _clickHandler;

        public BuildingCategoryButton buildingCategoryButton;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IList<IBuildableWrapper<IBuilding>> buildings,
            ISpriteProvider spriteProvider,
            IBuildingClickHandler clickHandler)
        {
            // Need these for abstract method called by base.Initialise().  Codesmell :P
            Helper.AssertIsNotNull(buildingCategoryButton, spriteProvider, clickHandler);

            _spriteProvider = spriteProvider;
            _clickHandler = clickHandler;

            base.Initialise(soundPlayer, uiManager, buttonVisibilityFilters, buildings);
            buildingCategoryButton.IsActiveFeedbackVisible = false;
        }

        protected override void InitialiseBuildableButton(BuildingButtonController button, IBuildableWrapper<IBuilding> buildableWrapper)
        {
            button.Initialise(_soundPlayer, buildableWrapper, _clickHandler, _shouldBeEnabledFilter);
        }

        public override void OnPresenting(object activationParameter)
        {
            base.OnPresenting(activationParameter);
            buildingCategoryButton.IsActiveFeedbackVisible = true;
        }

        public override void OnDismissing()
        {
            base.OnDismissing();
            buildingCategoryButton.IsActiveFeedbackVisible = false;
        }
    }
}
