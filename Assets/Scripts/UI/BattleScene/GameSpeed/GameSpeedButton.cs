using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class GameSpeedButton : CanvasGroupButton, IGameSpeedButton
    {
        private ITime _time;
        private FilterToggler _isEnabledToggler;

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
        }

        public void Initialise(ISingleSoundPlayer soundPlayer, IBroadcastingFilter shouldBeEnabledFilter, ITime time)
        {
            base.Initialise(soundPlayer);

            Assert.IsTrue(timeScale >= 0);
            Helper.AssertIsNotNull(selectedFeedback, shouldBeEnabledFilter, time);

            _time = time;
            IsSelected = false;
            _isEnabledToggler = new FilterToggler(shouldBeEnabledFilter, this);
        }

        public void TriggerClick()
        {
            OnPointerClick(eventData: null);
        }
    }
}