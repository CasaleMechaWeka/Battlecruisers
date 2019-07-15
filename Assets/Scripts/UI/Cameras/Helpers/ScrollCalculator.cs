using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityCommon.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers
{
    // FELIX  Test :)
    public class ScrollCalculator : IScrollCalculator
    {
        //private readonly ICamera _camera;
        private readonly IDeltaTimeProvider _deltaTimeProvider;
        // FELIX  Respect setting :)
        //private readonly ISettingsManager _settingsManager;
        //private readonly IScrollConverter _zoomConverter;

        public const float SCROLL_SCALE = 4;

        // FELIX :P
        public ScrollCalculator(
            IDeltaTimeProvider deltaTimeProvider
            //ICamera camera, 
            //ISettingsManager settingsManager,
            //IScrollConverter zoomConverter
            )
        {
            //Helper.AssertIsNotNull(camera, deltaTimeProvider, validOrthographicSizes, settingsManager, zoomConverter);
            Assert.IsNotNull(deltaTimeProvider);

            _deltaTimeProvider = deltaTimeProvider;
            //_camera = camera;
            //_validOrthographicSizes = validOrthographicSizes;
            //_settingsManager = settingsManager;
            //_zoomConverter = zoomConverter;
        }

        public float FindScrollDelta(float swipeDeltaX)
        {
            return
                // swiping left should move the screen right => invert
                -1 *
                swipeDeltaX *
                SCROLL_SCALE *
                _deltaTimeProvider.UnscaledDeltaTime;
                //_zoomConverter.LevelToSpeed(_settingsManager.ScrollSpeedLevel);
        }
    }
}