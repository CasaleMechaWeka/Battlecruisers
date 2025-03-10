using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    public abstract class BuildableClickHandler
    {
        protected readonly IPrioritisedSoundPlayer _eventSoundPlayer;
        protected readonly ISingleSoundPlayer _uiSoundPlayer;
        protected readonly IUIManager _uiManager;

        public BuildableClickHandler(IUIManager uiManager, IPrioritisedSoundPlayer eventSoundPlayer, ISingleSoundPlayer uiSoundPlayer)
        {
            Helper.AssertIsNotNull(uiManager, eventSoundPlayer, uiSoundPlayer);

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