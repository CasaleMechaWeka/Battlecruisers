using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets
{
    public class PvPCameraTargetEqualityCalculator : IPvPCameraTargetEqualityCalculator
    {
        private readonly float _positionEqualityMarginInM, _orthographicSizeEqualityMargin;

        public PvPCameraTargetEqualityCalculator(float positionEqualityMarginInM, float orthographicSizeEqualityMargin)
        {
            Assert.IsTrue(positionEqualityMarginInM > 0);
            Assert.IsTrue(orthographicSizeEqualityMargin > 0);

            _positionEqualityMarginInM = positionEqualityMarginInM;
            _orthographicSizeEqualityMargin = orthographicSizeEqualityMargin;
        }

        public bool IsOnTarget(IPvPCameraTarget target, IPvPCamera camera)
        {
            return
                camera.OrthographicSize - target.OrthographicSize < _orthographicSizeEqualityMargin
                && (camera.Position - target.Position).magnitude < _positionEqualityMarginInM;
        }
    }
}