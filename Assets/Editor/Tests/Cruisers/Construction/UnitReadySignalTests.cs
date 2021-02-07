using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Data.Settings;
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
        private ISettingsManager _settingsManager;

        [SetUp]
        public void TestSetup()
        {
            _unitMonitor = Substitute.For<ICruiserUnitMonitor>();
            _navalAudioSource = Substitute.For<IAudioSource>();
            _aircraftAudioSource = Substitute.For<IAudioSource>();
            _settingsManager = Substitute.For<ISettingsManager>();

            _settingsManager.EffectVolume.Returns(0.37f);

            _signal = new UnitReadySignal(_unitMonitor, _navalAudioSource, _aircraftAudioSource, _settingsManager);

            _completedUnit = Substitute.For<IUnit>();
        }

        [Test]
        public void InitialState()
        {
            CheckSetVolume();
        }

        [Test]
        public void _settingsManager_SettingsSaved()
        {
            ClearAudioSources();

            _settingsManager.SettingsSaved += Raise.Event();

            CheckSetVolume();
        }

        private void CheckSetVolume()
        {
            _navalAudioSource.Received().Volume = _settingsManager.EffectVolume;
            _aircraftAudioSource.Received().Volume = _settingsManager.EffectVolume;
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

            ClearAudioSources();
            _settingsManager.SettingsSaved += Raise.Event();
            _navalAudioSource.DidNotReceiveWithAnyArgs().Volume = default;
            _aircraftAudioSource.DidNotReceiveWithAnyArgs().Volume = default;
        }

        private void ClearAudioSources()
        {
            _navalAudioSource.ClearReceivedCalls();
            _aircraftAudioSource.ClearReceivedCalls();
        }
    }
}