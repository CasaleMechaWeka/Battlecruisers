using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public class PlayerCruiserHealthDialInitialiser : MonoBehaviour
    {
        private Image _lowHealthFeedback;
        private IHealthStateMonitor _cruiserHealthMonitor;

            // FELIX  Remove :D
        private const float MIN_PROPORTION = 0.12f;
        private const float MAX_PROPORTION = 0.77f;

        public IMaskHighlightable Initialise(ICruiser playerCruiser)
        {
            Assert.IsNotNull(playerCruiser);

            Image platformFillableImage = GetComponent<Image>();
            Assert.IsNotNull(platformFillableImage);
            IFillableImage fillableImage = new FillableImage(platformFillableImage);
            // FELIX  Remove :D
            //IFillCalculator fillCalculator = new FillCalculator(MIN_PROPORTION, MAX_PROPORTION);
            //IFillableImage subsetFillableImage = new SubsetFillableImage(coreFillableImage, fillCalculator);

            IFilter<ICruiser> visibilityFilter = new StaticFilter<ICruiser>(isMatch: true);

            IHealthDial<ICruiser> healthDial = new HealthDial<ICruiser>(fillableImage, visibilityFilter);
            healthDial.Damagable = playerCruiser;

            _lowHealthFeedback = transform.FindNamedComponent<Image>("LowHealthFeedback");

            _cruiserHealthMonitor = new HealthStateMonitor(playerCruiser);
            _cruiserHealthMonitor.HealthStateChanged += CruiserHealthMonitor_HealthStateChanged;

            MaskHighlightable highlightable = GetComponent<MaskHighlightable>();
            Assert.IsNotNull(highlightable);
            highlightable.Initialise();
            return highlightable;
        }

        private void CruiserHealthMonitor_HealthStateChanged(object sender, EventArgs e)
        {
            _lowHealthFeedback.enabled = _cruiserHealthMonitor.HealthState == HealthState.SeverelyDamaged;
        }
    }
}