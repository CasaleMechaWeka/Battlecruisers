using System.Collections.Generic;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class GameSpeedInitialiser : UIElement
    {
		void Start()
		{
            base.Initialise();

            ISpeedButtonManager speedButtonManager = new SpeedButtonManager();

            GameSpeedButton playButton = transform.FindNamedComponent<GameSpeedButton>("NormalSpeed");

            IList<GameSpeedButton> gameSpeedButtons = new List<GameSpeedButton>()
            {
                transform.FindNamedComponent<GameSpeedButton>("SlowMotion"),
                playButton,
                transform.FindNamedComponent<GameSpeedButton>("FastForward"),
                transform.FindNamedComponent<GameSpeedButton>("DoubleFastForward")
            };

            foreach (GameSpeedButton speedButton in gameSpeedButtons)
            {
                speedButton.Initialise(speedButtonManager);
            }

            speedButtonManager.SelectButton(playButton);
		}
	}
}
