using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public abstract class StatsRow : MonoBehaviour
	{
        private Image _comparisonFeedbackBackground;
        private Text _rowLabel;

        public virtual void Initialise()
        {
            _comparisonFeedbackBackground = GetComponent<Image>();
            Assert.IsNotNull(_comparisonFeedbackBackground);

            _rowLabel = transform.FindNamedComponent<Text>("RowLabel");
        }

		public void ShowResult(ComparisonResult comparisonResult)
		{
			_comparisonFeedbackBackground.color = comparisonResult.BackgroundColor;
            _rowLabel.color = comparisonResult.ForegroundColor;
		}
	}
}
