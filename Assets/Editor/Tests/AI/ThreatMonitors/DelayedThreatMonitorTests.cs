using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using BattleCruisers.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Tests.AI.ThreatMonitors
{
    public class DelayedThreatMonitorTests
    {
        private IThreatMonitor _delayedMonitor, _coreMonitor;
        private ITime _time;
        private IDeferrer _deferrer;
        private float _delayInS;
        private int _threatChangeCount;
        private IList<Action> _deferredActions;

        [SetUp]
        public void TestSetup()
        {
            _coreMonitor = Substitute.For<IThreatMonitor>();
            _time = Substitute.For<ITime>();
            _deferrer = Substitute.For<IDeferrer>();
            _delayInS = 4.32f;

            _delayedMonitor = new DelayedThreatMonitor(_coreMonitor, _time, _deferrer, _delayInS);

            _threatChangeCount = 0;
            _delayedMonitor.ThreatLevelChanged += (sender, e) => _threatChangeCount++;

            _deferredActions = new List<Action>();
            _deferrer.Defer(Arg.Do<Action>(action => _deferredActions.Add(action)), _delayInS);
        }

        [Test]
        public void ThreatChange_SameAfterDelay_Propagates()
        {
            TriggerCoreThreatChange(ThreatLevel.Low);

            // Waiting, nothing has changed yet
            Assert.AreEqual(0, _threatChangeCount);
            Assert.AreEqual(ThreatLevel.None, _delayedMonitor.CurrentThreatLevel);

            Assert.AreEqual(1, _deferredActions.Count);
            _deferredActions[0].Invoke();

            // Now should have changed
            Assert.AreEqual(1, _threatChangeCount);
            Assert.AreEqual(ThreatLevel.Low, _delayedMonitor.CurrentThreatLevel);
        }

        [Test]
        public void ThreatChange_DifferentAfterDelay_DoesNotPropagate()
        {
            // First threat level change
            TriggerCoreThreatChange(ThreatLevel.Low);
            Assert.AreEqual(1, _deferredActions.Count);

            // Second threat level change, before specified delayed time is up
            TriggerCoreThreatChange(ThreatLevel.High);
            Assert.AreEqual(2, _deferredActions.Count);

            // Exectue first deferred evaluation
            _deferredActions[0].Invoke();

            // Should not have changed, because threat level changed before time was up
            Assert.AreEqual(0, _threatChangeCount);
            Assert.AreEqual(ThreatLevel.None, _delayedMonitor.CurrentThreatLevel);

            // Execute second deferred evaluation
            _deferredActions[1].Invoke();

            // Should have changed, because threat level did not change before time was up
            Assert.AreEqual(1, _threatChangeCount);
            Assert.AreEqual(ThreatLevel.High, _delayedMonitor.CurrentThreatLevel);
        }

        private void TriggerCoreThreatChange(ThreatLevel threatLevel)
        {
            _coreMonitor.CurrentThreatLevel.Returns(threatLevel);
            _coreMonitor.ThreatLevelChanged += Raise.Event();
        }
    }
}