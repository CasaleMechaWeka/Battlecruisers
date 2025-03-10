using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class EdgeDetector : IEdgeDetector
    {
        private readonly IInput _input;
        private readonly IScreen _screen;
        private readonly int _edgeRegionWithInPixels;

        public EdgeDetector(IInput input, IScreen screen, int edgeRegionWithInPixels)
        {
            Helper.AssertIsNotNull(input, screen);
            Assert.IsTrue(edgeRegionWithInPixels >= 0);

            _input = input;
            _screen = screen;
            _edgeRegionWithInPixels = edgeRegionWithInPixels;
        }

        public bool IsCursorAtLeftEdge()
        {
            return _input.MousePosition.x < _edgeRegionWithInPixels;
        }

        public bool IsCursorAtRightEdge()
        {
            return _input.MousePosition.x > _screen.WidthInPixels - _edgeRegionWithInPixels;
        }
    }
}