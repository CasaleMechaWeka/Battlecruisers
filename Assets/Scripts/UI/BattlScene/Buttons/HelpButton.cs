using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class HelpButton : MonoBehaviour, IPointerClickHandler
    {
        private BroadcastingFilter _helpLabelsVisibilityFilter;
        private Image _leverOff, _leverOn;

        public void Initialise(BroadcastingFilter helpLabelsVisibilityFilter)
        {
            Assert.IsNotNull(helpLabelsVisibilityFilter);
            _helpLabelsVisibilityFilter = helpLabelsVisibilityFilter;

            _leverOff = transform.FindNamedComponent<Image>("LeverOff");
            _leverOn = transform.FindNamedComponent<Image>("LeverOn");

            UpdateLeverVisibility(_helpLabelsVisibilityFilter.IsMatch);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
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