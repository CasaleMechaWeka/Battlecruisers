using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Calculators
{
    public class PvPEdgeScrollCalculator : IPvPEdgeScrollCalculator
    {
        private readonly IPvPTime _time;
        private readonly ISettingsManager _settingsManager;
        private readonly IPvPLevelToMultiplierConverter _scrollLevelConverter;
        private readonly IPvPCamera _camera;
        private readonly IPvPRange<float> _validOrthographicSizes;
        private readonly float _scrollMultiplier;

        public PvPEdgeScrollCalculator(
            IPvPTime time,
            ISettingsManager settingsManager,
            IPvPLevelToMultiplierConverter scrollLevelConverter,
            IPvPCamera camera,
            IPvPRange<float> validOrthographicSizes,
            float scrollMultiplier)
        {
            PvPHelper.AssertIsNotNull(time, settingsManager, scrollLevelConverter, camera, validOrthographicSizes);
            Assert.IsTrue(scrollMultiplier > 0);

            _time = time;
            _settingsManager = settingsManager;
            _scrollLevelConverter = scrollLevelConverter;
            _camera = camera;
            _validOrthographicSizes = validOrthographicSizes;
            _scrollMultiplier = scrollMultiplier;
        }

        public float FindCameraPositionDeltaMagnituteInM()
        {
            // The more zoomed out the camera is, the greater our delta should be
            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;

            return
                _scrollMultiplier *
                _time.UnscaledDeltaTime *
                orthographicProportion *
                _scrollLevelConverter.LevelToMultiplier(_settingsManager.ScrollSpeedLevel);
        }
    }
}