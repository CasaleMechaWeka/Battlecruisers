using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Update;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors
{
    public class PvPManualDetectorPoller : IManagedDisposable
    {
        private readonly IPvPManualDetector _manualDetector;
        private readonly IPvPUpdater _updater;

        public PvPManualDetectorPoller(IPvPManualDetector manualDetector, IPvPUpdater updater)
        {
            Helper.AssertIsNotNull(manualDetector, updater);

            _manualDetector = manualDetector;
            _updater = updater;

            _updater.Updated += _updater_Updated;
        }

        private void _updater_Updated(object sender, EventArgs e)
        {
            _manualDetector.Detect();
        }

        public void DisposeManagedState()
        {
            _updater.Updated -= _updater_Updated;
        }
    }
}
