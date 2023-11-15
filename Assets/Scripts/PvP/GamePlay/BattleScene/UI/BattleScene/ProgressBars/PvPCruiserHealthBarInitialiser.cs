using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
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
        private IPvPHealthStateMonitor _cruiserHealthMonitor;

        public IPvPHighlightable Initialise(PvPCruiser cruiser)
        {
            Assert.IsNotNull(cruiser);

            return SetupHealthBar(cruiser);
        }

        private PvPHighlightable SetupHealthBar(PvPCruiser cruiser)
        {
            Image platformFillableImage = GetComponent<Image>();
            Assert.IsNotNull(platformFillableImage);
            IPvPFillableImage fillableImage = new PvPFillableImage(platformFillableImage);

            IPvPFilter<PvPTarget> visibilityFilter = new PvPStaticFilter<PvPTarget>(isMatch: true);

            IPvPHealthDial healthDial = new PvPHealthDial(fillableImage, visibilityFilter, damageTakenIndicator);
            healthDial.Damagable = cruiser;

            //_lowHealthFeedback = transform.FindNamedComponent<Image>("LowHealthFeedback");

            _cruiserHealthMonitor = new PvPHealthStateMonitor(cruiser);
            _cruiserHealthMonitor.HealthStateChanged += CruiserHealthMonitor_HealthStateChanged;

            PvPHighlightable highlightable = GetComponent<PvPHighlightable>();
            Assert.IsNotNull(highlightable);
            highlightable.Initialise();
            return highlightable;
        }

        private void CruiserHealthMonitor_HealthStateChanged(object sender, EventArgs e)
        {
            _lowHealthFeedback.enabled = _cruiserHealthMonitor.HealthState == PvPHealthState.SeverelyDamaged;
        }
    }
}