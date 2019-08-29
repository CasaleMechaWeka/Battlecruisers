using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    public abstract class BuildableClickHandler
    {
        protected readonly IPrioritisedSoundPlayer _soundPlayer;
        protected readonly IUIManager _uiManager;

        public BuildableClickHandler(IUIManager uiManager, IPrioritisedSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(uiManager, soundPlayer);

            _uiManager = uiManager;
            _soundPlayer = soundPlayer;
        }

        protected void PlayUnaffordableSound()
        {
            _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToBuild);
        }
    }
}