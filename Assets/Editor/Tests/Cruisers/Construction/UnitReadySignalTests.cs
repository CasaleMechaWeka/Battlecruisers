using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using NUnit.Framework;
using System;

namespace BattleCruisers.Tests.Cruisers.Construction
{
    public class UnitReadySignalTests
    {
        private IManagedDisposable _signal;
        private ICruiserUnitMonitor _unitMonitor;
        private IAudioSource _navalAudioSource, _aircraftAudioSource;
        private IUnit _completedUnit;

        [SetUp]
        public void TestSetup()
        {
            _unitMonitor = Substitute.For<ICruiserUnitMonitor>();
            _navalAudioSource = Substitute.For<IAudioSource>();
            _aircraftAudioSource = Substitute.For<IAudioSource>();

            _signal = new UnitReadySignal(_unitMonitor, _navalAudioSource, _aircraftAudioSource);

            _completedUnit = Substitute.For<IUnit>();
        }

        [Test]
        public void UnitCompleted_Aircraft()
        {
            _completedUnit.TargetType.Returns(TargetType.Aircraft);
            _unitMonitor.UnitCompleted += Raise.EventWith(new UnitCompletedEventArgs(_completedUnit));
            _aircraftAudioSource.Received().Play();
        }

        [Test]
        public void UnitCompleted_Naval()
        {
            _completedUnit.TargetType.Returns(TargetType.Ships);
            _unitMonitor.UnitCompleted += Raise.EventWith(new UnitCompletedEventArgs(_completedUnit));
            _navalAudioSource.Received().Play();
        }

        [Test]
        public void UnitCompleted_Ohter_Throws()
        {
            _completedUnit.TargetType.Returns(TargetType.Cruiser);
            Assert.Throws<ArgumentException>(() => _unitMonitor.UnitCompleted += Raise.EventWith(new UnitCompletedEventArgs(_completedUnit)));
        }

        [Test]
        public void DisposeManagedState_Unsubscribes()
        {
            _signal.DisposeManagedState();

            _completedUnit.TargetType.Returns(TargetType.Aircraft);
            _unitMonitor.UnitCompleted += Raise.EventWith(new UnitCompletedEventArgs(_completedUnit));
            _aircraftAudioSource.DidNotReceive().Play();
        }
    }
}