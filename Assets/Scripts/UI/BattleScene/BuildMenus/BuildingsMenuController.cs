using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildingsMenuController : BuildablesMenuController<BuildingButtonController, IBuilding>
    {
        private ISingleSoundPlayer _soundPlayer;
        private IBuildingClickHandler _clickHandler;

        public BuildingCategoryButton buildingCategoryButton;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            UIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IList<IBuildableWrapper<IBuilding>> buildings,
            IBuildingClickHandler clickHandler)
        {
            // Need these for abstract method called by base.Initialise().  Codesmell :P
            Helper.AssertIsNotNull(soundPlayer, buildingCategoryButton, clickHandler);

            _soundPlayer = soundPlayer;
            _clickHandler = clickHandler;

            base.Initialise(uiManager, buttonVisibilityFilters, buildings);
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
