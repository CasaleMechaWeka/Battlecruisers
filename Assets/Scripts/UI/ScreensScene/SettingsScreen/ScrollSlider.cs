using BattleCruisers.Data.Settings;
using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    // FELIX  Avoid duplicate code with ScrollSlider :)
    public class ScrollSlider : MonoBehaviour
    {
        private Slider _slider;

        private ISettableBroadcastingProperty<int> _sliderValue;
        public IBroadcastingProperty<int> SliderValue { get; private set; }

        public void Initialise(int selectedScrollSpeed)
        {
            AssertIsValidScrollSpeed(selectedScrollSpeed);

            _sliderValue = new SettableBroadcastingProperty<int>(selectedScrollSpeed);
            SliderValue = new BroadcastingProperty<int>(_sliderValue);

            _slider = GetComponent<Slider>();
            Assert.IsNotNull(_slider);

            _slider.value = selectedScrollSpeed;
            _slider.minValue = SettingsManager.MIN_SCROLL_SPEED_LEVEL;
            _slider.maxValue = SettingsManager.MAX_SCROLL_SPEED_LEVEL;
            _slider.onValueChanged.AddListener(OnScrollSpeedChanged);
        }

        private void OnScrollSpeedChanged(float newScrollSpeed)
        {
            int scrollSpeedAsInt = Mathf.RoundToInt(newScrollSpeed);
            AssertIsValidScrollSpeed(scrollSpeedAsInt);
            _sliderValue.Value = scrollSpeedAsInt;
        }

        private void AssertIsValidScrollSpeed(int scrollSpeed)
        {
            Assert.IsTrue(scrollSpeed >= SettingsManager.MIN_SCROLL_SPEED_LEVEL);
            Assert.IsTrue(scrollSpeed <= SettingsManager.MAX_SCROLL_SPEED_LEVEL);
        }
    }
}