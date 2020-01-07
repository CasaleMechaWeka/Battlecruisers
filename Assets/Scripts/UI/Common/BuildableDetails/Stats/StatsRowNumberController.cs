using BattleCruisers.Utils;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public class StatsRowNumberController : StatsRow
	{
		private Text _rowValue;

        public override void Initialise()
        {
            base.Initialise();
            _rowValue = transform.FindNamedComponent<Text>("RowValue");
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
			_rowValue.text = value;
		}
	}
}
