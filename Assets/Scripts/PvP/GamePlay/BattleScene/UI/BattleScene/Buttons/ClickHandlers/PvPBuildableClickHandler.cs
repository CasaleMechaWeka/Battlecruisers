using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Sound.Players;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers
{
    public abstract class PvPBuildableClickHandler
    {
        protected readonly IPrioritisedSoundPlayer _eventSoundPlayer;
        protected readonly ISingleSoundPlayer _uiSoundPlayer;
        protected readonly IPvPUIManager _uiManager;

        public PvPBuildableClickHandler(IPvPUIManager uiManager, IPrioritisedSoundPlayer eventSoundPlayer, ISingleSoundPlayer uiSoundPlayer)
        {
            PvPHelper.AssertIsNotNull(uiManager, eventSoundPlayer, uiSoundPlayer);

            _uiManager = uiManager;
            _eventSoundPlayer = eventSoundPlayer;
            _uiSoundPlayer = uiSoundPlayer;
        }

        protected void PlayUnaffordableSound()
        {
            _eventSoundPlayer.PlaySound(PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToBuild);
        }
    }
}