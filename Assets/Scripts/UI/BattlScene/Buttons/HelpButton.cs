using BattleCruisers.UI.Filters;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class HelpButton : MonoBehaviour, IPointerClickHandler
    {
        private BroadcastingFilter _helpLabelsVisibilityFilter;

        public void Initialise(BroadcastingFilter helpLabelsVisibilityFilter)
        {
            Assert.IsNotNull(helpLabelsVisibilityFilter);
            _helpLabelsVisibilityFilter = helpLabelsVisibilityFilter;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _helpLabelsVisibilityFilter.IsMatch = !_helpLabelsVisibilityFilter.IsMatch;
        }
    }
}