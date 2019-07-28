using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetDetectors
{
    /// <summary>
    /// Manual target detection requires two components that always work together.
    /// This class groups those two components.
    /// </summary>
    public class ManualDetectorProvider
    {
        // Hold reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private readonly ManualDetectorPoller _detectorPoller;
#pragma warning restore CS0414  // Variable is assigned but never used

        public IManualProximityTargetDetector TargetDetector { get; }

        public ManualDetectorProvider(ManualDetectorPoller detectorPoller, IManualProximityTargetDetector targetDetector)
        {
            Helper.AssertIsNotNull(detectorPoller, targetDetector);

            _detectorPoller = detectorPoller;
            TargetDetector = targetDetector;
        }
    }
}