using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors
{
    /// <summary>
    /// Manual target detection requires two components that always work together.
    /// This class groups those two components.
    /// </summary>
    public class PvPManualDetectorProvider : IPvPManagedDisposable
    {
        private readonly PvPManualDetectorPoller _detectorPoller;

        public IPvPManualProximityTargetDetector TargetDetector { get; }

        public PvPManualDetectorProvider(PvPManualDetectorPoller detectorPoller, IPvPManualProximityTargetDetector targetDetector)
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