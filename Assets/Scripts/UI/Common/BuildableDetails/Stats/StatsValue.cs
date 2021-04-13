using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public abstract class StatsValue : MonoBehaviour
	{
        public CanvasGroup canvasGroup;
        public Image border, icon;
        // Optional
        public Text label;

        public virtual void Initialise()
        {
            Helper.AssertIsNotNull(canvasGroup, border, icon);
        }

		public void ShowResult(ComparisonResult comparisonResult)
		{
            canvasGroup.alpha = comparisonResult.RowAlpha;

            border.color = comparisonResult.RowColour;
            icon.color = comparisonResult.RowColour;
            
            if (label != null)
            {
                label.color = comparisonResult.RowColour;
            }
		}
	}
}
