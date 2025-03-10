using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats
{
    public abstract class PvPStatsValue : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public Image border, icon;
        // Optional
        public Text label;

        public virtual void Initialise()
        {
            PvPHelper.AssertIsNotNull(canvasGroup, border, icon);
        }

        public void ShowResult(PvPComparisonResult comparisonResult)
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
