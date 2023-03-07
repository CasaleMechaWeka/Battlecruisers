using BattleCruisers.Data.Models;
using BattleCruisers.Utils;
using BattleCruisers.UI.Sound.Players;


namespace BattleCruisers.UI.ScreensScene.PvP.ArenaScreen.Buttons
{

    public abstract class ArenaScreenButton : TextButton
    {
        protected IMultiplayScreen _multiplayScreen;
        protected IGameModel _gameModel;

        public virtual void Initialise(ISingleSoundPlayer soundPlayer, IMultiplayScreen multiplayScreen, IGameModel gameModel)
        {
            base.Initialise(soundPlayer, multiplayScreen);
            Helper.AssertIsNotNull(multiplayScreen, gameModel);

            _multiplayScreen = multiplayScreen;
            _gameModel = gameModel;
        }
    }
}

