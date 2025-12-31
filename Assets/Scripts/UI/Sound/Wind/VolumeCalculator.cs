using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.Wind
{
    public class VolumeCalculator
    {
        private readonly IRange<float> _validOrthographicSizes;
        private readonly SettingsManager _settingsManager;

        public VolumeCalculator(
            IRange<float> validOrthographicSizes,
            SettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(validOrthographicSizes, settingsManager);

            _validOrthographicSizes = validOrthographicSizes;
            _settingsManager = settingsManager;
        }

        //note, this is used for wind sound effects
        public float FindVolume(float cameraOrthographicSize)
        {
            float rawProportion = FindProportion(cameraOrthographicSize, _validOrthographicSizes);
            return rawProportion * _settingsManager.AmbientVolume * _settingsManager.MasterVolume;
        }

        float FindProportion(float value, IRange<float> range)
        {
            Assert.IsNotNull(range);

            value = Mathf.Clamp(value, range.Min, range.Max);

            float sizeAboveMin = value - range.Min;
            float rangeValue = range.Max - range.Min;

            return sizeAboveMin / rangeValue;
        }
    }
}