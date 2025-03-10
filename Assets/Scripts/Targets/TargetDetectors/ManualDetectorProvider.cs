using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetDetectors
{
    /// <summary>
    /// Manual target detection requires two components that always work together.
    /// This class groups those two components.
    /// </summary>
    public class ManualDetectorProvider : IManagedDisposable
    {
        private readonly ManualDetectorPoller _detectorPoller;

        public IManualProximityTargetDetector TargetDetector { get; }

        public ManualDetectorProvider(ManualDetectorPoller detectorPoller, IManualProximityTargetDetector targetDetector)
        {
            Helper.AssertIsNotNull(detectorPoller, targetDetector);

            _detectorPoller = detectorPoller;
            TargetDetector = targetDetector;
        }

        public void DisposeManagedState()
        {
            _detectorPoller.DisposeManagedState();
        }
    }
}