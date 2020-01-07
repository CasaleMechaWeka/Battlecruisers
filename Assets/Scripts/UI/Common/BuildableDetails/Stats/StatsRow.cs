using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public abstract class StatsRow : MonoBehaviour
	{
        private CanvasGroup _canvasGroup;

        public virtual void Initialise()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);
        }

		public void ShowResult(ComparisonResult comparisonResult)
		{
            _canvasGroup.alpha = comparisonResult.RowAlpha;
		}
	}
}
