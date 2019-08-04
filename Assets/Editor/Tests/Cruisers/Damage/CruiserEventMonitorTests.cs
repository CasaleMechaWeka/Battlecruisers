using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.Timers;
using NSubstitute;
using NUnit.Framework;
using System;

namespace BattleCruisers.Tests.Cruisers.Damage
{
    public class CruiserEventMonitorTests
    {
#pragma warning disable CS0414  // Variable is assigned but never used
        private CruiserEventMonitor _monitor;
#pragma warning restore CS0414  // Variable is assigned but never used
        private IHealthThresholdMonitor _cruiserHealthThresholdMonitor;
        private ICruiserDamageMonitor _cruiserDamageMonitor;
        private IPrioritisedSoundPlayer _soundPlayer;
        private IDebouncer _debouncer;

        [SetUp]
        public void TestSetup()
        {
            _cruiserHealthThresholdMonitor = Substitute.For<IHealthThresholdMonitor>();
            _cruiserDamageMonitor = Substitute.For<ICruiserDamageMonitor>();
            _soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();
            _debouncer = Substitute.For<IDebouncer>();

            _monitor = new CruiserEventMonitor(_cruiserHealthThresholdMonitor, _cruiserDamageMonitor, _soundPlayer, _debouncer);
        }

        [Test]
        public void CruiserHealthThresholdReached_PlaysSound()
        {
            _cruiserHealthThresholdMonitor.ThresholdReached += Raise.Event();
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.Cruiser.SignificantlyDamaged);
        }

        [Test]
        public void CruiserDamaged_Debounces()
        {
            Action debouncedAction = null;
            _debouncer.Debounce(Arg.Do<Action>(x => debouncedAction = x));

            _cruiserDamageMonitor.CruiserOrBuildingDamaged += Raise.Event();

            Assert.IsNotNull(debouncedAction);
            debouncedAction.Invoke();
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.Cruiser.UnderAttack);
        }
    }
}