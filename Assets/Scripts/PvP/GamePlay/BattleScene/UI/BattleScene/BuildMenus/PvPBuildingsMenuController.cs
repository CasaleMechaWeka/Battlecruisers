using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Sprites;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public class PvPBuildingsMenuController : PvPBuildablesMenuController<PvPBuildingButtonController, IPvPBuilding>
    {
        private IPvPSingleSoundPlayer _soundPlayer;
        private IPvPSpriteProvider _spriteProvider;
        private IPvPBuildingClickHandler _clickHandler;

        public PvPBuildingCategoryButton buildingCategoryButton;

        public void Initialise(
            IPvPSingleSoundPlayer soundPlayer,
            IPvPUIManager uiManager,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IList<IPvPBuildableWrapper<IPvPBuilding>> buildings,
            IPvPSpriteProvider spriteProvider,
            IPvPBuildingClickHandler clickHandler)
        {
            // Need these for abstract method called by base.Initialise().  Codesmell :P
            PvPHelper.AssertIsNotNull(soundPlayer, buildingCategoryButton, spriteProvider, clickHandler);

            _soundPlayer = soundPlayer;
            _spriteProvider = spriteProvider;
            _clickHandler = clickHandler;

            base.Initialise(uiManager, buttonVisibilityFilters, buildings);
            buildingCategoryButton.IsActiveFeedbackVisible = false;
        }

        protected override void InitialiseBuildableButton(PvPBuildingButtonController button, IPvPBuildableWrapper<IPvPBuilding> buildableWrapper)
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
