using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Calculators
{
    public class PvPScrollCalculator : IPvPScrollCalculator
    {
        private readonly IPvPCamera _camera;
        private readonly IPvPTime _time;
        private readonly IPvPRange<float> _validOrthographicSizes;
        private readonly ISettingsManager _settingsManager;
        private readonly IPvPLevelToMultiplierConverter _scrollLevelConverter;
        private readonly float _scrollMultiplier;

        public PvPScrollCalculator(
            IPvPCamera camera,
            IPvPTime time,
            IPvPRange<float> validOrthographicSizes,
            ISettingsManager settingsManager,
            IPvPLevelToMultiplierConverter scrollLevelConverter,
            float scrollMultiplier)
        {
            PvPHelper.AssertIsNotNull(camera, time, validOrthographicSizes, settingsManager, scrollLevelConverter);
            Assert.IsTrue(scrollMultiplier > 0);

            _camera = camera;
            _time = time;
            _validOrthographicSizes = validOrthographicSizes;
            _settingsManager = settingsManager;
            _scrollLevelConverter = scrollLevelConverter;
            _scrollMultiplier = scrollMultiplier;
        }

        public float FindScrollDelta(float swipeDeltaX)
        {
            // The more zoomed out the camera is, the greater our delta should be
            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;
            // Direction should be inverted, so swiping left should move the screen right
            float directionMultiplier = -1;

            return
                swipeDeltaX *
                directionMultiplier *
                orthographicProportion *
                _scrollMultiplier *
                _time.UnscaledDeltaTime *
                _scrollLevelConverter.LevelToMultiplier(_settingsManager.ScrollSpeedLevel);
        }
    }
}