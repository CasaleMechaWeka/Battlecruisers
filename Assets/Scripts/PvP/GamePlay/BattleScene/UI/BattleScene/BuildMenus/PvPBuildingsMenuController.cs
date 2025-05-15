using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Sound.Players;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public class PvPBuildingsMenuController : PvPBuildablesMenuController<PvPBuildingButtonController, IPvPBuilding>
    {
        private SingleSoundPlayer _soundPlayer;
        private IPvPBuildingClickHandler _clickHandler;
        private bool _flipClickAndDragIcon;

        public PvPBuildingCategoryButton buildingCategoryButton;

        public void Initialise(
            SingleSoundPlayer soundPlayer,
            PvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IList<IPvPBuildableWrapper<IPvPBuilding>> buildings,
            IPvPBuildingClickHandler clickHandler,
            bool flipClickAndDragIcon)
        {
            // Need these for abstract method called by base.Initialise().  Codesmell :P
            PvPHelper.AssertIsNotNull(soundPlayer, buildingCategoryButton, clickHandler);

            _soundPlayer = soundPlayer;
            _clickHandler = clickHandler;
            _flipClickAndDragIcon = flipClickAndDragIcon;

            base.Initialise(uiManager, buttonVisibilityFilters, buildings);
            buildingCategoryButton.IsActiveFeedbackVisible = false;
        }

        protected override void InitialiseBuildableButton(PvPBuildingButtonController button, IPvPBuildableWrapper<IPvPBuilding> buildableWrapper)
        {
            button.Initialise(_soundPlayer, buildableWrapper, _clickHandler, _shouldBeEnabledFilter, _flipClickAndDragIcon);
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
