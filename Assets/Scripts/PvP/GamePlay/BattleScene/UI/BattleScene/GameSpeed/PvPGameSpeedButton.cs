using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Players;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BattleCruisers.UI.BattleScene.GameSpeed;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed
{
    public class PvPGameSpeedButton : PvPCanvasGroupButton, IGameSpeedButton
    {
        private ITime _time;
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

        public void Initialise(ISingleSoundPlayer soundPlayer, IBroadcastingFilter shouldBeEnabledFilter, ITime time)
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