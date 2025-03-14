using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars
{
    //need to link a script containing an image with a method that makes the image active for a second after taking damage
    public class PvPCruiserHealthBarInitialiser : MonoBehaviour
    {
        public Image _lowHealthFeedback;
        public PvPDamageTakenIndicator damageTakenIndicator;
        private IHealthStateMonitor _cruiserHealthMonitor;

        public IHighlightable Initialise(PvPCruiser cruiser)
        {
            Assert.IsNotNull(cruiser);

            return SetupHealthBar(cruiser);
        }

        private Highlightable SetupHealthBar(PvPCruiser cruiser)
        {
            Image platformFillableImage = GetComponent<Image>();
            Assert.IsNotNull(platformFillableImage);
            IFillableImage fillableImage = new FillableImage(platformFillableImage);

            IFilter<PvPTarget> visibilityFilter = new PvPStaticFilter<PvPTarget>(isMatch: true);

            IPvPHealthDial healthDial = new PvPHealthDial(fillableImage, visibilityFilter, damageTakenIndicator);
            healthDial.Damagable = cruiser;

            //_lowHealthFeedback = transform.FindNamedComponent<Image>("LowHealthFeedback");

            _cruiserHealthMonitor = new PvPHealthStateMonitor(cruiser);
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