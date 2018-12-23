using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public abstract class HealthDialInitialiserBase<TDamagable> : MonoBehaviour where TDamagable : IDamagable
    {
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private FilterToggler _helpLabelsVisibilityToggler;
#pragma warning restore CS0414  // Variable is assigned but never used

        public IHealthDial<TDamagable> Initialise(IBroadcastingFilter helpLabelVisibilityFilter)
        {
            Assert.IsNotNull(helpLabelVisibilityFilter);

            HelpLabel helpLabel = GetComponentInChildren<HelpLabel>();
            Assert.IsNotNull(helpLabel);
            helpLabel.Initialise();
            _helpLabelsVisibilityToggler = new FilterToggler(helpLabel, helpLabelVisibilityFilter);

            Image platformFillableImage = GetComponent<Image>();
            Assert.IsNotNull(platformFillableImage);
            IFillableImage fillableImage = new FillableImage(platformFillableImage);

            IFilter<TDamagable> visibilityFilter = CreateVisibilityFilter();

            return new HealthDial<TDamagable>(fillableImage, visibilityFilter);
        }

        protected abstract IFilter<TDamagable> CreateVisibilityFilter();
    }
}