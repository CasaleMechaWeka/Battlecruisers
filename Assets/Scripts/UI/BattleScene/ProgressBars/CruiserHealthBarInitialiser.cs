using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    //need to link a script containing an image with a method that makes the image active for a second after taking damage
    public class CruiserHealthBarInitialiser : MonoBehaviour
    {
        public Image _lowHealthFeedback;
        public DamageTakenIndicator damageTakenIndicator;
        private IHealthStateMonitor _cruiserHealthMonitor;

        public IHighlightable Initialise(ICruiser cruiser)
        {
            Assert.IsNotNull(cruiser);

            return SetupHealthBar(cruiser);
        }

        private Highlightable SetupHealthBar(ICruiser cruiser)
        {
            Image platformFillableImage = GetComponent<Image>();
            Assert.IsNotNull(platformFillableImage);
            IFillableImage fillableImage = new FillableImage(platformFillableImage);

            IFilter<ICruiser> visibilityFilter = new StaticFilter<ICruiser>(isMatch: true);

            IHealthDial<ICruiser> healthDial = new HealthDial<ICruiser>(fillableImage, visibilityFilter, damageTakenIndicator);
            healthDial.Damagable = cruiser;

            //_lowHealthFeedback = transform.FindNamedComponent<Image>("LowHealthFeedback");

            _lowHealthFeedback.enabled = false;

            _cruiserHealthMonitor = new HealthStateMonitor(cruiser);
            _cruiserHealthMonitor.HealthStateChanged += CruiserHealthMonitor_HealthStateChanged;

            Highlightable highlightable = GetComponent<Highlightable>();
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