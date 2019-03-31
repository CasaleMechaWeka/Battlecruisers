using BattleCruisers.Data.Settings;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class ZoomSlider : MonoBehaviour
    {
        private Slider _slider;

        private ISettableBroadcastingProperty<int> _sliderValue;
        public IBroadcastingProperty<int> SliderValue { get; private set; }

        public void Initialise(int selectedZoomSpeed)
        {
            AssertIsValidZoomSpeed(selectedZoomSpeed);

            _sliderValue = new SettableBroadcastingProperty<int>(selectedZoomSpeed);
            SliderValue = new BroadcastingProperty<int>(_sliderValue);

            _slider = GetComponent<Slider>();
            Assert.IsNotNull(_slider);

            _slider.value = selectedZoomSpeed;
            _slider.minValue = SettingsManager.MIN_ZOOM_SPEED_LEVEL;
            _slider.maxValue = SettingsManager.MAX_ZOOM_SPEED_LEVEL;
            _slider.onValueChanged.AddListener(OnZoomSpeedChanged);
        }

        private void OnZoomSpeedChanged(float newZoomSpeed)
        {
            int zoomSpeedAsInt = Mathf.RoundToInt(newZoomSpeed);
            AssertIsValidZoomSpeed(zoomSpeedAsInt);
            _sliderValue.Value = zoomSpeedAsInt;
        }

        private void AssertIsValidZoomSpeed(int zoomSpeed)
        {
            Assert.IsTrue(zoomSpeed >= SettingsManager.MIN_ZOOM_SPEED_LEVEL);
            Assert.IsTrue(zoomSpeed <= SettingsManager.MAX_ZOOM_SPEED_LEVEL);
        }
    }
}