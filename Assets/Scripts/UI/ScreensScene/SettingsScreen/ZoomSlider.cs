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

        private ISettableBroadcastingProperty<float> _sliderValue;
        public IBroadcastingProperty<float> SliderValue { get; private set; }

        public void Initialise(float selectedZoomSpeed)
        {
            AssertIsValidZoomSpeed(selectedZoomSpeed);

            _sliderValue = new SettableBroadcastingProperty<float>(selectedZoomSpeed);
            SliderValue = new BroadcastingProperty<float>(_sliderValue);

            _slider = GetComponent<Slider>();
            Assert.IsNotNull(_slider);

            _slider.value = selectedZoomSpeed;
            _slider.minValue = SettingsManager.MIN_ZOOM_SPEED;
            _slider.maxValue = SettingsManager.MAX_ZOOM_SPEED;
            _slider.onValueChanged.AddListener(OnZoomSpeedChanged);
        }

        private void OnZoomSpeedChanged(float newZoomSpeed)
        {
            AssertIsValidZoomSpeed(newZoomSpeed);
            _sliderValue.Value = newZoomSpeed;
        }

        private void AssertIsValidZoomSpeed(float zoomSpeed)
        {
            Assert.IsTrue(zoomSpeed >= SettingsManager.MIN_ZOOM_SPEED);
            Assert.IsTrue(zoomSpeed <= SettingsManager.MAX_ZOOM_SPEED);
        }
    }
}