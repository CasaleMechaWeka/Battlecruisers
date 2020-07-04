using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.Wind
{
    public class VolumeCalculator : IVolumeCalculator
    {
        private readonly IRange<float> _validOrthographicSizes;

        public VolumeCalculator(IRange<float> validOrthographicSizes)
        {
            Assert.IsNotNull(validOrthographicSizes);
            _validOrthographicSizes = validOrthographicSizes;
        }

        public float FindVolume(float cameraOrthographicSize)
        {
            cameraOrthographicSize = Mathf.Clamp(cameraOrthographicSize, _validOrthographicSizes.Min, _validOrthographicSizes.Max);

            float sizeAboveMin = cameraOrthographicSize - _validOrthographicSizes.Min;
            float range = _validOrthographicSizes.Max - _validOrthographicSizes.Min;

            // OS proportion    Wind volume
            // 0                0
            // 0.5              0.5
            // 1                1
            return sizeAboveMin / range;
        }
    }
}