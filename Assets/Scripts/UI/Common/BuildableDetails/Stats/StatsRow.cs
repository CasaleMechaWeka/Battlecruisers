using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public abstract class StatsRow : MonoBehaviour
	{
        private Image _comparisonFeedbackBackground;

        public virtual void Initialise()
        {
            _comparisonFeedbackBackground = GetComponent<Image>();
            Assert.IsNotNull(_comparisonFeedbackBackground);
        }

		public void ShowResult(ComparisonResult comparisonResult)
		{
			_comparisonFeedbackBackground.color = comparisonResult.Color;
		}
	}
}
