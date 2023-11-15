using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats
{
    public class PvPStarsStatValue : PvPStatsValue
    {
        public Image activeDiceStars;
        public List<Sprite> diceStars;

        private const int EXPECTED_NUM_OF_DICE_STARS = 5;

        public override void Initialise()
        {
            base.Initialise();

            Assert.IsNotNull(activeDiceStars);
            Assert.AreEqual(EXPECTED_NUM_OF_DICE_STARS, diceStars.Count);
        }

        public void ShowResult(int statRating, PvPComparisonResult comparisonResult)
        {
            base.ShowResult(comparisonResult);

            int index = statRating - 1;
            Assert.IsTrue(index >= 0);
            Assert.IsTrue(index < EXPECTED_NUM_OF_DICE_STARS);

            activeDiceStars.sprite = diceStars[index];
            activeDiceStars.color = comparisonResult.RowColour;
        }
    }
}