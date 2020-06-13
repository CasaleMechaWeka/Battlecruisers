using BattleCruisers.Utils.DataStrctures;
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
            Assert.IsTrue(cameraOrthographicSize >= _validOrthographicSizes.Min);
            Assert.IsTrue(cameraOrthographicSize <= _validOrthographicSizes.Max);

            float orthographicSizeProportion = cameraOrthographicSize / _validOrthographicSizes.Max;

            // Don't play wind if less than halfway zoomed out
            if (orthographicSizeProportion < 0.5)
            {
                return 0;
            }

            // OS proportion    Wind volume
            // 0.5              0
            // 0.75             0.5
            // 1                1
            return 2 * orthographicSizeProportion - 1;
        }
    }
}