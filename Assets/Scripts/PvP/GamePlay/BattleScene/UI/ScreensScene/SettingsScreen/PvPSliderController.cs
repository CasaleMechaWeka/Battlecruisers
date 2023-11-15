using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.ScreensScene.SettingsScreen
{
    public class PvPSliderController : MonoBehaviour
    {
        private IPvPRange<int> _validRange;
        private Slider _slider;

        private IPvPSettableBroadcastingProperty<int> _sliderValue;
        public IPvPBroadcastingProperty<int> SliderValue { get; private set; }
        public GameObject sliderTextLabel;
        private Text textSliderValue;

        public void Initialise(int selectedValue, IPvPRange<int> validRange)
        {
            Assert.IsNotNull(validRange);

            _validRange = validRange;

            AssertIsValidValue(selectedValue);

            _sliderValue = new PvPSettableBroadcastingProperty<int>(selectedValue);
            SliderValue = new PvPBroadcastingProperty<int>(_sliderValue);

            _slider = GetComponent<Slider>();
            Assert.IsNotNull(_slider);

            _slider.value = selectedValue;
            _slider.minValue = validRange.Min;
            _slider.maxValue = validRange.Max;
            _slider.onValueChanged.AddListener(OnValueChanged);

            textSliderValue = sliderTextLabel.GetComponent<Text>();
            textSliderValue.text = _sliderValue.Value + "";
        }

        private void OnValueChanged(float newValue)
        {
            int valueAsInt = Mathf.RoundToInt(newValue);
            AssertIsValidValue(valueAsInt);
            _sliderValue.Value = valueAsInt;
            textSliderValue.text = _sliderValue.Value + "";
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