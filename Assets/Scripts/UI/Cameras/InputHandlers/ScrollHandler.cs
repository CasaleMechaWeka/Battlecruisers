using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils;
using UnityEngine;
using BattleCruisers.UI.Cameras.Helpers;

namespace BattleCruisers.UI.Cameras.InputHandlers
{
    /// <summary>
    /// When the mouse touches the edge of the screen we want the screen to scroll
    /// in that direction.  Similar to real time strategy games like Age of Empires.
    /// </summary>
    /// FELIX  Use ScreenEdgePositionFinder :)
    /// FELIX  Update tests :)
    public class ScrollHandler : IScrollHandler
	{
		private readonly ICameraCalculator _calculator;
		private readonly IPositionClamper _clamper;
        private readonly IDeltaTimeProvider _deltaTimeProvider;
        private readonly IScrollPositionFinder _scrollPositionFinder;

        // Cannot have a scroll boundary of 0, otherwise only left and upwards
        // scroll work on Mac.  Right and downwards do not.
        private const float SCROLL_BOUNDARY_IN_PIXELS = 2;

        public ScrollHandler(
            ICameraCalculator calculator, 
            IPositionClamper clamper, 
            IDeltaTimeProvider deltaTimeProvider, 
            IScrollPositionFinder scrollPositionFinder)
		{
            Helper.AssertIsNotNull(calculator, clamper, deltaTimeProvider, scrollPositionFinder);

			_calculator = calculator;
			_clamper = clamper;
            _deltaTimeProvider = deltaTimeProvider;
            _scrollPositionFinder = scrollPositionFinder;
		}

		public Vector3 FindCameraPosition(float cameraOrthographicSize, Vector3 cameraPosition, Vector3 mousePosition)
		{
            float scrollSpeed = _calculator.FindScrollSpeed(cameraOrthographicSize, _deltaTimeProvider.UnscaledDeltaTime);
            Vector3 desiredPosition = _scrollPositionFinder.FindDesiredPosition(cameraPosition, mousePosition, scrollSpeed);
			return _clamper.Clamp(desiredPosition);
		}
	}
}
