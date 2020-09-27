using BattleCruisers.Utils.Threading;
using BattleCruisers.Utils.Timers;
using NSubstitute;
using NUnit.Framework;
using System;
using UnityCommon.PlatformAbstractions.Time;

namespace BattleCruisers.Tests.Utils.Timers
{
    public class DeferredDebouncerTests
    {
        private IDebouncer _debouncer;
        private ITimeSinceGameStartProvider _time;
        private IDeferrer _deferrer;
        private float _debounceTimeInS;
        private bool _wasActionCalled;
        private Action _deferredAction;

        [SetUp]
        public void TestSetup()
        {
            _time = Substitute.For<ITimeSinceGameStartProvider>();
            _deferrer = Substitute.For<IDeferrer>();
            _debounceTimeInS = 12.3f;

            _debouncer = new DeferredDebouncer(_time, _deferrer, _debounceTimeInS);

            _deferrer.Defer(Arg.Do<Action>(action => _deferredAction = action), _debounceTimeInS);
            _wasActionCalled = false;
        }

        [Test]
        public void Debounce_RecentActionsWhenExecuted_DoNotExecute()
        {
            // Debounce
            _time.TimeSinceGameStartInS.Returns(0);
            _debouncer.Debounce(() => _wasActionCalled = true);
            Assert.IsFalse(_wasActionCalled);
            Assert.IsNotNull(_deferredAction);

            // Deferred action executed too soon
            _time.TimeSinceGameStartInS.Returns(_debounceTimeInS - 0.1f);
            _deferredAction.Invoke();
            Assert.IsFalse(_wasActionCalled);
        }


        [Test]
        public void Debounce_NoRecentActionsWhenExecuted_Execute()
        {
            // Debounce
            _time.TimeSinceGameStartInS.Returns(0);
            _debouncer.Debounce(() => _wasActionCalled = true);
            Assert.IsFalse(_wasActionCalled);
            Assert.IsNotNull(_deferredAction);

            // Deferred action executed after appropriate wait time
            _time.TimeSinceGameStartInS.Returns(_debounceTimeInS + 0.1f);
            _deferredAction.Invoke();
            Assert.IsTrue(_wasActionCalled);
        }
    }
}