using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Targets
{
    public class CameraTargetEqualityCalculator
    {
        private readonly float _positionEqualityMarginInM, _orthographicSizeEqualityMargin;

        public CameraTargetEqualityCalculator(float positionEqualityMarginInM, float orthographicSizeEqualityMargin)
        {
            Assert.IsTrue(positionEqualityMarginInM > 0);
            Assert.IsTrue(orthographicSizeEqualityMargin > 0);

            _positionEqualityMarginInM = positionEqualityMarginInM;
            _orthographicSizeEqualityMargin = orthographicSizeEqualityMargin;
        }

        public bool IsOnTarget(CameraTarget target, ICamera camera)
        {
            return
                camera.OrthographicSize - target.OrthographicSize < _orthographicSizeEqualityMargin
                && (camera.Position - target.Position).magnitude < _positionEqualityMarginInM;
        }
    }
}