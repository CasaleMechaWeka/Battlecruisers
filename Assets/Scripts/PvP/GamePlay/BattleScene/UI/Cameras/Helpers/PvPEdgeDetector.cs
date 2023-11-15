using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers
{
    public class PvPEdgeDetector : IPvPEdgeDetector
    {
        private readonly IPvPInput _input;
        private readonly IPvPScreen _screen;
        private readonly int _edgeRegionWithInPixels;

        public PvPEdgeDetector(IPvPInput input, IPvPScreen screen, int edgeRegionWithInPixels)
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