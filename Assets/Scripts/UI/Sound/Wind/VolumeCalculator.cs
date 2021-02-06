using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.UI.Sound.Wind
{
    public class VolumeCalculator : IVolumeCalculator
    {
        private readonly IProportionCalculator _proportionCalculator;
        private readonly IRange<float> _validOrthographicSizes;
        private readonly ISettingsManager _settingsManager;

        public VolumeCalculator(
            IProportionCalculator proportionCalculator, 
            IRange<float> validOrthographicSizes,
            ISettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(proportionCalculator, validOrthographicSizes, settingsManager);

            _proportionCalculator = proportionCalculator;
            _validOrthographicSizes = validOrthographicSizes;
            _settingsManager = settingsManager;
        }

        public float FindVolume(float cameraOrthographicSize)
        {
            float rawProportion = _proportionCalculator.FindProprtion(cameraOrthographicSize, _validOrthographicSizes);
            return rawProportion * _settingsManager.EffectVolume;
        }
    }
}