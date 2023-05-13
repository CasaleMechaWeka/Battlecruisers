using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Panels;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public class PvPSelectorPanelController : PvPSlidingPanel
    {
        public void Initialise(IPvPUIManager uiManager, IPvPButtonVisibilityFilters buttonVisibilityFilters, IPvPSingleSoundPlayer soundPlayer)
        {
            base.Initialise();

            PvPHelper.AssertIsNotNull(uiManager, buttonVisibilityFilters);

            PvPDismissSelectorPanelButtonController dismissButton = GetComponentInChildren<PvPDismissSelectorPanelButtonController>();
            Assert.IsNotNull(dismissButton);
            dismissButton.Initialise(soundPlayer, uiManager, buttonVisibilityFilters.DismissButtonVisibilityFilter);
        }
    }
}