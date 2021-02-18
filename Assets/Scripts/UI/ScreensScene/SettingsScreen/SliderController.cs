using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class SliderController : MonoBehaviour
    {
        private IRange<int> _validRange;
        private Slider _slider;

        private ISettableBroadcastingProperty<int> _sliderValue;
        public IBroadcastingProperty<int> SliderValue { get; private set; }

        public void Initialise(int selectedValue, IRange<int> validRange)
        {
            Assert.IsNotNull(validRange);

            _validRange = validRange;

            AssertIsValidValue(selectedValue);

            _sliderValue = new SettableBroadcastingProperty<int>(selectedValue);
            SliderValue = new BroadcastingProperty<int>(_sliderValue);

            _slider = GetComponent<Slider>();
            Assert.IsNotNull(_slider);

            _slider.value = selectedValue;
            _slider.minValue = validRange.Min;
            _slider.maxValue = validRange.Max;
            _slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(float newValue)
        {
            int valueAsInt = Mathf.RoundToInt(newValue);
            AssertIsValidValue(valueAsInt);
            _sliderValue.Value = valueAsInt;
        }

        private void AssertIsValidValue(int value)
        {
            Assert.IsTrue(value >= _validRange.Min);
            Assert.IsTrue(value <= _validRange.Max);
        }

        public void ResetToDefaults(int defaultValue)
        {
            _slider.value = defaultValue;
        }
    }
}