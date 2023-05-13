using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.ScreensScene.SettingsScreen
{
    public class PvPFloatSliderController : MonoBehaviour
    {
        private IPvPRange<float> _validRange;
        private Slider _slider;

        private IPvPSettableBroadcastingProperty<float> _sliderValue;
        public IPvPBroadcastingProperty<float> SliderValue { get; private set; }

        public GameObject sliderTextLabel;
        private Text textSliderValue;

        public void Initialise(float selectedValue, IPvPRange<float> validRange)
        {
            Assert.IsNotNull(validRange);

            _validRange = validRange;

            AssertIsValidValue(selectedValue);

            _sliderValue = new PvPSettableBroadcastingProperty<float>(selectedValue);
            SliderValue = new PvPBroadcastingProperty<float>(_sliderValue);

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