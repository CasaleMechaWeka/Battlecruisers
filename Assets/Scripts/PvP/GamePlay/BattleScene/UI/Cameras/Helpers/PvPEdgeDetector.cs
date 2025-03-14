using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers
{
    public class PvPEdgeDetector : IEdgeDetector
    {
        private readonly IInput _input;
        private readonly IScreen _screen;
        private readonly int _edgeRegionWithInPixels;

        public PvPEdgeDetector(IInput input, IScreen screen, int edgeRegionWithInPixels)
        {
            PvPHelper.AssertIsNotNull(input, screen);
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