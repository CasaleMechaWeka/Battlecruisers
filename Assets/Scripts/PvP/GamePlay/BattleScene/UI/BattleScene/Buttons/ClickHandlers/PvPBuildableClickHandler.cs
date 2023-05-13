using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers
{
    public abstract class PvPBuildableClickHandler
    {
        protected readonly IPvPPrioritisedSoundPlayer _eventSoundPlayer;
        protected readonly IPvPSingleSoundPlayer _uiSoundPlayer;
        protected readonly IPvPUIManager _uiManager;

        public PvPBuildableClickHandler(IPvPUIManager uiManager, IPvPPrioritisedSoundPlayer eventSoundPlayer, IPvPSingleSoundPlayer uiSoundPlayer)
        {
            PvPHelper.AssertIsNotNull(uiManager, eventSoundPlayer, uiSoundPlayer);

            _uiManager = uiManager;
            _eventSoundPlayer = eventSoundPlayer;
            _uiSoundPlayer = uiSoundPlayer;
        }

        protected void PlayUnaffordableSound()
        {
            _eventSoundPlayer.PlaySound(PvPPrioritisedSoundKeys.PvPEvents.PvPDrones.NotEnoughDronesToBuild);
        }
    }
}