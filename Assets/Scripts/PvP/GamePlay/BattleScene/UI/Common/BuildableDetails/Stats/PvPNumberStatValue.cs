using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats
{
    public class PvPNumberStatValue : PvPStatsValue
    {
        public Text rowValue;

        public override void Initialise()
        {
            base.Initialise();
            Assert.IsNotNull(rowValue);
        }

        public void ShowResult(int value, PvPComparisonResult comparisonResult)
        {
            ShowResult(value.ToString(), comparisonResult);
        }

        public void ShowResult(string value, PvPComparisonResult comparisonResult)
        {
            base.ShowResult(comparisonResult);

            rowValue.text = value;
            rowValue.color = comparisonResult.RowColour;
        }
    }
}
