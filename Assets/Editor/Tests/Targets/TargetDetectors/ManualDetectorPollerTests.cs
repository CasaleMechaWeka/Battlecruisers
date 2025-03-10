using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Utils.BattleScene.Update;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetDetectors
{
    public class ManualDetectorPollerTests
    {
        private ManualDetectorPoller _detectorPoller;
        private IManualDetector _manualDetector;
        private IUpdater _updater;

        [SetUp]
        public void TestSetup()
        {
            _manualDetector = Substitute.For<IManualDetector>();
            _updater = Substitute.For<IUpdater>();

            _detectorPoller = new ManualDetectorPoller(_manualDetector, _updater);
        }

        [Test]
        public void Updated_TriggersDetect()
        {
            _updater.Updated += Raise.Event();
            _manualDetector.Received().Detect();
        }

        [Test]
        public void DisposeManagedState()
        {
            _detectorPoller.DisposeManagedState();
            _updater.Updated += Raise.Event();
            _manualDetector.DidNotReceive().Detect();
        }
    }
}