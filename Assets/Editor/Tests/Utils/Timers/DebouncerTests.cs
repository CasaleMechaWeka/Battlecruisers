using BattleCruisers.Utils.Timers;
using NSubstitute;
using NUnit.Framework;
using BattleCruisers.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Tests.Utils.Timers
{
    public class DebouncerTests
    {
        private IDebouncer _debouncer;
        private ITimeSinceGameStartProvider _time;
        private float _debounceTimeInS = 12.3f;
        private bool _wasActionCalled = false;

        [SetUp]
        public void TestSetup()
        {
            _time = Substitute.For<ITimeSinceGameStartProvider>();
            _debouncer = new Debouncer(_time, _debounceTimeInS);
        }

        [Test]
        public void Debounce_FirstTime_InvokesAction()
        {
            _time.TimeSinceGameStartInS.Returns(0);
            _debouncer.Debounce(() => _wasActionCalled = true);
            Assert.IsTrue(_wasActionCalled);
        }

        [Test]
        public void UndebouncedEvent_WithinDebounceTime_DoesNotInvokeAction()
        {
            // First time
            _time.TimeSinceGameStartInS.Returns(0);
            _debouncer.Debounce(() => _wasActionCalled = true);
            Assert.IsTrue(_wasActionCalled);

            // Second time within debounce time
            _wasActionCalled = false;
            _time.TimeSinceGameStartInS.Returns(_debounceTimeInS - 1);
            _debouncer.Debounce(() => _wasActionCalled = true);
            Assert.IsFalse(_wasActionCalled);
        }

        [Test]
        public void UndebouncedEvent_OutsideDebounceTime_InvokesAction()
        {
            // First time
            _time.TimeSinceGameStartInS.Returns(0);
            _debouncer.Debounce(() => _wasActionCalled = true);
            Assert.IsTrue(_wasActionCalled);

            // Second time outside of debounce time
            _wasActionCalled = false;
            _time.TimeSinceGameStartInS.Returns(_debounceTimeInS + 1);
            _debouncer.Debounce(() => _wasActionCalled = true);
            Assert.IsTrue(_wasActionCalled);
        }
    }
}