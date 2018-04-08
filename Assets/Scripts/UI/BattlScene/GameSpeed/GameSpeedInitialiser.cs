using System.Collections.Generic;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class GameSpeedInitialiser : MonoBehaviour
    {
		void Start()
		{
            ISpeedButtonManager speedButtonManager = new SpeedButtonManager();

            GameSpeedButton playButton = transform.FindNamedComponent<GameSpeedButton>("PlayButton");

            IList<GameSpeedButton> gameSpeedButtons = new List<GameSpeedButton>()
            {
                transform.FindNamedComponent<GameSpeedButton>("SlowMotionButton"),
                playButton,
                transform.FindNamedComponent<GameSpeedButton>("FastForwardButton"),
                transform.FindNamedComponent<GameSpeedButton>("DoubleFastForwardButton")
            };

            foreach (GameSpeedButton speedButton in gameSpeedButtons)
            {
                speedButton.Initialise(speedButtonManager);
            }

            speedButtonManager.SelectButton(playButton);
		}
	}
}
