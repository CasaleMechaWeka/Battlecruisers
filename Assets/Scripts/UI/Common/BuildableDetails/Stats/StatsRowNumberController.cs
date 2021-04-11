using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public class StatsRowNumberController : StatsRow
	{
		public Text rowValue;

        public override void Initialise()
        {
            base.Initialise();
			Assert.IsNotNull(rowValue);
        }

        public void ShowResult(int value, ComparisonResult comparisonResult)
		{
			ShowResult(value.ToString(), comparisonResult);
		}

		public void ShowResult(float value, ComparisonResult comparisonResult)
		{
			ShowResult(value.ToString(), comparisonResult);
		}

		public void ShowResult(string value, ComparisonResult comparisonResult)
		{
			base.ShowResult(comparisonResult);
			rowValue.text = value;
		}
	}
}
