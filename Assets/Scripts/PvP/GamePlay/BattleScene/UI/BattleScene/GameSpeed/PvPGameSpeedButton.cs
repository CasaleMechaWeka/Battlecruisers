using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed
{
    public class PvPGameSpeedButton : PvPCanvasGroupButton, IPvPGameSpeedButton
    {
        private IPvPTime _time;
        private PvPFilterToggler _isEnabledToggler;

        private const float DEFAULT_TIME_SCALE = 1;

        public float timeScale;

        public Image selectedFeedback;
        protected override MaskableGraphic Graphic => selectedFeedback;

        public bool IsSelected
        {
            set
            {
                selectedFeedback.gameObject.SetActive(value);

                _time.TimeScale = value ? timeScale : DEFAULT_TIME_SCALE;
            }
            get
            {
                return selectedFeedback.gameObject.activeInHierarchy;
            }
        }

        public void Initialise(IPvPSingleSoundPlayer soundPlayer, IPvPBroadcastingFilter shouldBeEnabledFilter, IPvPTime time)
        {
            base.Initialise(soundPlayer);

            Assert.IsTrue(timeScale >= 0);
            PvPHelper.AssertIsNotNull(selectedFeedback, shouldBeEnabledFilter, time);

            _time = time;
            IsSelected = false;
            _isEnabledToggler = new PvPFilterToggler(shouldBeEnabledFilter, this);
        }

        public void TriggerClick()
        {
            OnPointerClick(eventData: null);
        }
    }
}