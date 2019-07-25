using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using System;

namespace BattleCruisers.Targets.TargetDetectors
{
    public class ManualDetectorPoller
    {
        private readonly IManualDetector _manualDetector;
        private readonly IUpdater _updater;

        public ManualDetectorPoller(IManualDetector manualDetector, IUpdater updater)
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
    }
}