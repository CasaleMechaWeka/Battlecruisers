using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class GameSpeedWrapper : MonoBehaviour, IGameSpeedWrapper
    {
        public IButton SlowMotionButton { get; private set; }
        public IButton PlayButton { get; private set; }
        public IButton FastForwardButton { get; private set; }
        public IButton DoubleFastForwardButton { get; private set; }

        public void Initialise()
		{
            ISpeedButtonManager speedButtonManager = new SpeedButtonManager();
			
			GameSpeedButton slowMotionButton = transform.FindNamedComponent<GameSpeedButton>("SlowMotion");
			slowMotionButton.Initialise(speedButtonManager);
			SlowMotionButton = slowMotionButton;

            GameSpeedButton playButton = transform.FindNamedComponent<GameSpeedButton>("NormalSpeed");
            playButton.Initialise(speedButtonManager);
            PlayButton = playButton;

            GameSpeedButton fastForwardButton = transform.FindNamedComponent<GameSpeedButton>("FastForward");
            fastForwardButton.Initialise(speedButtonManager);
            FastForwardButton = fastForwardButton;

            GameSpeedButton doubleFastForwardButton = transform.FindNamedComponent<GameSpeedButton>("DoubleFastForward");
            doubleFastForwardButton.Initialise(speedButtonManager);
            DoubleFastForwardButton = doubleFastForwardButton;

            speedButtonManager.SelectButton(playButton);
		}
	}
}
