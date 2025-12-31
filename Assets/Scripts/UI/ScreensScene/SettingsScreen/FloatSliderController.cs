using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class FloatSliderController : MonoBehaviour
    {
        private IRange<float> _validRange;
        private Slider _slider;
        
        private ISettableBroadcastingProperty<float> _sliderValue;
        public IBroadcastingProperty<float> SliderValue { get; private set; }

        public GameObject sliderTextLabel;
        private Text textSliderValue;

        public void Initialise(float selectedValue, IRange<float> validRange)
        {
            Assert.IsNotNull(validRange);

            _validRange = validRange;

            AssertIsValidValue(selectedValue);

            _sliderValue = new SettableBroadcastingProperty<float>(selectedValue);
            SliderValue = new BroadcastingProperty<float>(_sliderValue);

            _slider = GetComponent<Slider>();
            Assert.IsNotNull(_slider);

            _slider.value = selectedValue;
            _slider.minValue = validRange.Min;
            _slider.maxValue = validRange.Max;
            _slider.onValueChanged.AddListener(OnValueChanged);

            //Debug.Log(Math.Round(_sliderValue.Value * 100, 0, MidpointRounding.ToEven) + "%");


            textSliderValue = sliderTextLabel.GetComponent<Text>();
            textSliderValue.text = "" + Math.Round(_sliderValue.Value * 100, 0, MidpointRounding.ToEven) + "%";
        }

        private void OnValueChanged(float newValue)
        {
            AssertIsValidValue(newValue);
            _sliderValue.Value = newValue;
            //Debug.Log(Math.Round(_sliderValue.Value*100, 0, MidpointRounding.ToEven) + "%");
            textSliderValue.text = "" + Math.Round(_sliderValue.Value * 100, 0, MidpointRounding.ToEven) + "%";
        }

        private void AssertIsValidValue(float value)
        {
            Assert.IsTrue(value >= _validRange.Min);
            Assert.IsTrue(value <= _validRange.Max);
        }

        public void ResetToDefaults(float defaultValue)
        {
            _slider.value = defaultValue;
        }
    }
}