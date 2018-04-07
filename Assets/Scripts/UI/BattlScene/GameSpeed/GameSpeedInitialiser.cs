using System.Collections.Generic;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class GameSpeedInitialiser : MonoBehaviour
    {
		void Start()
		{
            Text speedText = GetComponentInChildren<Text>();
            Assert.IsNotNull(speedText);

            IList<IGameSpeedModifier> gameSpeedModifiers = new List<IGameSpeedModifier>()
            {
				transform.FindNamedComponent<IGameSpeedModifier>("SlowMotionButton"),
				transform.FindNamedComponent<IGameSpeedModifier>("PlayButton"),
				transform.FindNamedComponent<IGameSpeedModifier>("FastForwardButton"),
				transform.FindNamedComponent<IGameSpeedModifier>("DoubleFastForwardButton")
            };

            new GameSpeedDisplay(speedText, gameSpeedModifiers);
		}
	}
}
