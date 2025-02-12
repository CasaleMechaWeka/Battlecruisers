using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors
{
    /// <summary>
    /// Manual target detection requires two components that always work together.
    /// This class groups those two components.
    /// </summary>
    public class PvPManualDetectorProvider : IManagedDisposable
    {
        private readonly PvPManualDetectorPoller _detectorPoller;

        public IManualProximityTargetDetector TargetDetector { get; }

        public PvPManualDetectorProvider(PvPManualDetectorPoller detectorPoller, IManualProximityTargetDetector targetDetector)
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