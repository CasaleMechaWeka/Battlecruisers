using System.Collections.Generic;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class GameSpeedDisplay
    {
        private readonly Text _speedText;

        private const string SPEED_PREFIX = "x";
        
        public GameSpeedDisplay(Text speedText, IList<IGameSpeedModifier> gameSpeedModifiers)
		{
            Helper.AssertIsNotNull(speedText, gameSpeedModifiers);
            Assert.IsTrue(gameSpeedModifiers.Count > 0);

            _speedText = speedText;

            foreach (IGameSpeedModifier speedModifier in gameSpeedModifiers)
            {
                speedModifier.GameSpeedChanged += SpeedModifier_GameSpeedChanged;
            }
		}

        private void SpeedModifier_GameSpeedChanged(object sender, GameSpeedChangedEventArgs e)
        {
            _speedText.text = SPEED_PREFIX + e.NewSpeed;
        }
	}
}
