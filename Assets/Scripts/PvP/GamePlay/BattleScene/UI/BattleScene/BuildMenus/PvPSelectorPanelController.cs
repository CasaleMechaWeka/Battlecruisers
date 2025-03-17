using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound.Players;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public class PvPSelectorPanelController : SlidingPanel
    {
        public void Initialise(IPvPUIManager uiManager, IPvPButtonVisibilityFilters buttonVisibilityFilters, ISingleSoundPlayer soundPlayer)
        {
            base.Initialise();

            PvPHelper.AssertIsNotNull(uiManager, buttonVisibilityFilters);

            PvPDismissSelectorPanelButtonController dismissButton = GetComponentInChildren<PvPDismissSelectorPanelButtonController>();
            Assert.IsNotNull(dismissButton);
            dismissButton.Initialise(soundPlayer, uiManager, buttonVisibilityFilters.DismissButtonVisibilityFilter);
        }
    }
}