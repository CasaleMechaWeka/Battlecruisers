using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class HelpButton : CanvasGroupButton
    {
        private BroadcastingFilter _helpLabelsVisibilityFilter;
        private Image _leverOff, _leverOn;

        public void Initialise(ISingleSoundPlayer soundPlayer, BroadcastingFilter helpLabelsVisibilityFilter)
        {
            base.Initialise(soundPlayer);

            Assert.IsNotNull(helpLabelsVisibilityFilter);
            _helpLabelsVisibilityFilter = helpLabelsVisibilityFilter;

            _leverOff = transform.FindNamedComponent<Image>("LeverOff");
            _leverOn = transform.FindNamedComponent<Image>("LeverOn");

            UpdateLeverVisibility(_helpLabelsVisibilityFilter.IsMatch);
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            _helpLabelsVisibilityFilter.IsMatch = !_helpLabelsVisibilityFilter.IsMatch;
            UpdateLeverVisibility(_helpLabelsVisibilityFilter.IsMatch);
        }

        private void UpdateLeverVisibility(bool areHelpLabelsVisible)
        {
            _leverOff.enabled = !_helpLabelsVisibilityFilter.IsMatch;
            _leverOn.enabled = _helpLabelsVisibilityFilter.IsMatch;
        }
    }
}