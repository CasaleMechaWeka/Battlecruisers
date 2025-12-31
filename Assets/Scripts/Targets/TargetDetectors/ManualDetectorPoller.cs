using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using System;

namespace BattleCruisers.Targets.TargetDetectors
{
    public class ManualDetectorPoller : IManagedDisposable
    {
        private readonly ManualProximityTargetDetector _manualDetector;
        private readonly IUpdater _updater;

        public ManualDetectorPoller(ManualProximityTargetDetector manualDetector, IUpdater updater)
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