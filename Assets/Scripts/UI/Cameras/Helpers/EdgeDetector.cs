using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class EdgeDetector
    {
        private readonly IInput _input;
        private readonly int _edgeRegionWithInPixels;

        public EdgeDetector(IInput input, int edgeRegionWithInPixels)
        {
            Helper.AssertIsNotNull(input);
            Assert.IsTrue(edgeRegionWithInPixels >= 0);

            _input = input;
            _edgeRegionWithInPixels = edgeRegionWithInPixels;
        }

        public bool IsCursorAtLeftEdge()
        {
            return _input.MousePosition.x < _edgeRegionWithInPixels;
        }

        public bool IsCursorAtRightEdge()
        {
            return _input.MousePosition.x > Screen.width - _edgeRegionWithInPixels;
        }
    }
}