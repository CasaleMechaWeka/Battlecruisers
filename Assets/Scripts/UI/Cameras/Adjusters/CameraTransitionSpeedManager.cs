using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Adjusters
{
    public class CameraTransitionSpeedManager : ICameraTransitionSpeedManager, ICameraSmoothTimeProvider
    {
        private readonly float _normalSmoothTime, _slowSmoothTime;
        private const float MIN_SMOOTH_TIME = 0;

        public float SmoothTime { get; private set; }

        public CameraTransitionSpeedManager(float normalSmoothTime, float slowSmoothTime)
        {
            Assert.IsTrue(normalSmoothTime > MIN_SMOOTH_TIME);
            Assert.IsTrue(slowSmoothTime > MIN_SMOOTH_TIME);
            Assert.IsTrue(normalSmoothTime < slowSmoothTime);

            _normalSmoothTime = normalSmoothTime;
            _slowSmoothTime = slowSmoothTime;

            SmoothTime = _normalSmoothTime;
        }

        public void SetNormalTransitionSpeed()
        {
            SmoothTime = _normalSmoothTime;
        }

        public void SetSlowTransitionSpeed()
        {
            SmoothTime = _slowSmoothTime;
        }
    }
}