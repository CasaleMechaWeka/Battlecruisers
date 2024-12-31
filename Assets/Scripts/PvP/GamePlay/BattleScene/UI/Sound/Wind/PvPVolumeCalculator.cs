using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Wind
{
    public class PvPVolumeCalculator : IPvPVolumeCalculator
    {
        private readonly IProportionCalculator _proportionCalculator;
        private readonly IRange<float> _validOrthographicSizes;
        private readonly ISettingsManager _settingsManager;

        public PvPVolumeCalculator(
            IProportionCalculator proportionCalculator,
            IRange<float> validOrthographicSizes,
            ISettingsManager settingsManager)
        {
            PvPHelper.AssertIsNotNull(proportionCalculator, validOrthographicSizes, settingsManager);

            _proportionCalculator = proportionCalculator;
            _validOrthographicSizes = validOrthographicSizes;
            _settingsManager = settingsManager;
        }

        //note, this is used for wind sound effects
        public float FindVolume(float cameraOrthographicSize)
        {
            float rawProportion = _proportionCalculator.FindProportion(cameraOrthographicSize, _validOrthographicSizes);
            return rawProportion * _settingsManager.AmbientVolume * _settingsManager.MasterVolume;
        }
    }
}