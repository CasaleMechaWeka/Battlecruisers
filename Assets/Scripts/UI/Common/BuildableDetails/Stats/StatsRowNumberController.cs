using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public class StatsRowNumberController : StatsRow
	{
		public Text rowValue;
		
		public void Initialise(int value, ComparisonResult comparisonResult)
		{
			Initialise(value.ToString(), comparisonResult);
		}

		public void Initialise(float value, ComparisonResult comparisonResult)
		{
			Initialise(value.ToString(), comparisonResult);
		}

		public void Initialise(string value, ComparisonResult comparisonResult)
		{
			base.Iniitalise(comparisonResult);
			rowValue.text = value;
		}
	}
}
