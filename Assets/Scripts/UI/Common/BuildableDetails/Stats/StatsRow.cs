using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public abstract class StatsRow : MonoBehaviour
	{
        private Image _comparisonFeedbackBackground;

		public void Iniitalise(ComparisonResult comparisonResult)
		{
            _comparisonFeedbackBackground = GetComponent<Image>();
            Assert.IsNotNull(_comparisonFeedbackBackground);
			_comparisonFeedbackBackground.color = comparisonResult.Color;
		}
	}
}
