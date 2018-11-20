using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    /// <summary>
    /// Has a filter that this class listens to, turning itself on
    /// or off accordingly.  
    /// </summary>
    public class TogglableElement : MonoBehaviour
    {
        private IBroadcastingFilter _shouldBeEnabledFilter;
        private CanvasGroup _canvasGroup;

        protected virtual bool IsEnabled
        {
            set
            {
                _canvasGroup.alpha = value ? Constants.ENABLED_UI_ALPHA : Constants.DISABLED_UI_ALPHA;
            }
        }

        public virtual void Initialise(IBroadcastingFilter shouldBeEnabledFilter)
        {
            Assert.IsNotNull(shouldBeEnabledFilter);

            _shouldBeEnabledFilter = shouldBeEnabledFilter;
            _shouldBeEnabledFilter.PotentialMatchChange += _shouldBeEnabledFilter_PotentialMatchChange;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            UpdateIsEnabled();
        }

        private void _shouldBeEnabledFilter_PotentialMatchChange(object sender, EventArgs e)
        {
            UpdateIsEnabled();
        }

        private void UpdateIsEnabled()
        {
            IsEnabled = _shouldBeEnabledFilter.IsMatch;
        }
    }
}
