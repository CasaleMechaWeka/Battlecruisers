using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers.Sprites;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public class PvPBuildingsMenuController : PvPBuildablesMenuController<PvPBuildingButtonController, IPvPBuilding>
    {
        private ISingleSoundPlayer _soundPlayer;
        private SpriteProvider _spriteProvider;
        private IPvPBuildingClickHandler _clickHandler;
        private bool _flipClickAndDragIcon;

        public PvPBuildingCategoryButton buildingCategoryButton;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IPvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IList<IPvPBuildableWrapper<IPvPBuilding>> buildings,
SpriteProvider spriteProvider,
            IPvPBuildingClickHandler clickHandler,
            bool flipClickAndDragIcon)
        {
            // Need these for abstract method called by base.Initialise().  Codesmell :P
            PvPHelper.AssertIsNotNull(soundPlayer, buildingCategoryButton, spriteProvider, clickHandler);

            _soundPlayer = soundPlayer;
            _spriteProvider = spriteProvider;
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
