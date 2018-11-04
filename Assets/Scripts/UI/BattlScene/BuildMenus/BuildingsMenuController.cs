using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildingsMenuController : BuildablesMenuController<BuildingButtonController, IBuilding>
	{
        private ISpriteProvider _spriteProvider;
        private IBuildingClickHandler _clickHandler;

        public GameObject isPresentedFeedback;

        public void Initialise(
            IUIManager uiManager,
            IButtonVisibilityFilters buttonVisibilityFilters,
            IList<IBuildableWrapper<IBuilding>> buildings,
            ISpriteProvider spriteProvider,
            IBuildingClickHandler clickHandler)
        {
            // Need these for abstract method called by base.Initialise().  Codesmell :P
            // NEWUI  Switch back
            Helper.AssertIsNotNull(spriteProvider, clickHandler);
            //Helper.AssertIsNotNull(isPresentedFeedback, spriteProvider, clickHandler);

            _spriteProvider = spriteProvider;
            _clickHandler = clickHandler;

            base.Initialise(uiManager, buttonVisibilityFilters, buildings);

            // NEWUI  Remove null check
            if (isPresentedFeedback != null)
            {
                isPresentedFeedback.SetActive(false);
            }
        }

        protected override void InitialiseBuildableButton(BuildingButtonController button, IBuildableWrapper<IBuilding> buildableWrapper)
        {
            button.Initialise(buildableWrapper, _clickHandler, _shouldBeEnabledFilter);
        }

        public override void OnPresenting(object activationParameter)
        {
            base.OnPresenting(activationParameter);

            // NEWUI  Remove null check
            if (isPresentedFeedback != null)
            {
                isPresentedFeedback.SetActive(true);
            }
        }

        public override void OnDismissing()
        {
            base.OnDismissing();
    
            // NEWUI  Remove null check
            if (isPresentedFeedback != null)
            {
                isPresentedFeedback.SetActive(false);
            }
        }
    }
}
