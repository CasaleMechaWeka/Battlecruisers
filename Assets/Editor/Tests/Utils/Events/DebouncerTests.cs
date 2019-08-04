using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Utils.Events;
using NSubstitute;
using NUnit.Framework;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.Tests.Utils.Events
{
    public class DebouncerTests
    {
        private Debouncer<TargetEventArgs> _debouncer;
        private IDebouncable<TargetEventArgs> _debouncable;
        private ITime _time;
        private float _debounceTimeInS = 12.3f;
        private TargetEventArgs _eventArgs;

        [SetUp]
        public void TestSetup()
        {
            _debouncable = Substitute.For<IDebouncable<TargetEventArgs>>();
            _time = Substitute.For<ITime>();

            _debouncer = new Debouncer<TargetEventArgs>(_debouncable, _time, _debounceTimeInS);

            _eventArgs = new TargetEventArgs(null);
        }

        [Test]
        public void UndebouncedEvent_FirstTime_EmitsDebouncedEvent()
        {
            _time.TimeSinceGameStartInS.Returns(0);
            _debouncable.UndebouncedEvent += Raise.EventWith(_eventArgs);
            _debouncable.Received().EmitDebouncedEvent(_eventArgs);
        }

        [Test]
        public void UndebouncedEvent_WithinDebounceTime_DoesNotEmitDebouncedEvent()
        {
            // First event
            _time.TimeSinceGameStartInS.Returns(0);
            _debouncable.UndebouncedEvent += Raise.EventWith(_eventArgs);
            _debouncable.Received().EmitDebouncedEvent(_eventArgs);

            // Second event within debounce time
            _debouncable.ClearReceivedCalls();
            _time.TimeSinceGameStartInS.Returns(_debounceTimeInS - 1);
            _debouncable.UndebouncedEvent += Raise.EventWith(_eventArgs);
            _debouncable.DidNotReceiveWithAnyArgs().EmitDebouncedEvent(null);
        }

        [Test]
        public void UndebouncedEvent_OutsideDebounceTime_EmitsDebouncedEvent()
        {
            // First event
            _time.TimeSinceGameStartInS.Returns(0);
            _debouncable.UndebouncedEvent += Raise.EventWith(_eventArgs);
            _debouncable.Received().EmitDebouncedEvent(_eventArgs);

            // Second event outside of debounce time
            _debouncable.ClearReceivedCalls();
            _time.TimeSinceGameStartInS.Returns(_debounceTimeInS + 1);
            _debouncable.UndebouncedEvent += Raise.EventWith(_eventArgs);
            _debouncable.Received().EmitDebouncedEvent(_eventArgs);
        }
    }
}