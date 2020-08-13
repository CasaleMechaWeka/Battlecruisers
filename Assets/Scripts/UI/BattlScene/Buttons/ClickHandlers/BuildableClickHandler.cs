using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    public abstract class BuildableClickHandler
    {
        protected readonly IPrioritisedSoundPlayer _eventSoundPlayer;
        protected readonly IUIManager _uiManager;

        public BuildableClickHandler(IUIManager uiManager, IPrioritisedSoundPlayer eventSoundPlayer)
        {
            Helper.AssertIsNotNull(uiManager, eventSoundPlayer);

            _uiManager = uiManager;
            _eventSoundPlayer = eventSoundPlayer;
        }

        protected void PlayUnaffordableSound()
        {
            _eventSoundPlayer.PlaySound(PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToBuild);
        }
    }
}