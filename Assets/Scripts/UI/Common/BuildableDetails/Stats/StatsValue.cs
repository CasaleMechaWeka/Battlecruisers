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
            // Log the comparison result for debugging
            // Debug.Log($"ShowResult called with comparisonResult: {comparisonResult}");

            // Check for null comparisonResult
            if (comparisonResult == null)
            {
                Debug.LogError("comparisonResult is null");
                return;
            }

            // Validate UI elements
            if (canvasGroup == null || border == null || icon == null)
            {
                Debug.LogError("UI elements are not properly initialized");
                return;
            }

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
