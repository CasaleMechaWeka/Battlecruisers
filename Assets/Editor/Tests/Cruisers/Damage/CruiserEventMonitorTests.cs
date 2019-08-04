using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using NSubstitute;
using NUnit.Framework;

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

        [SetUp]
        public void TestSetup()
        {
            _cruiserHealthThresholdMonitor = Substitute.For<IHealthThresholdMonitor>();
            _cruiserDamageMonitor = Substitute.For<ICruiserDamageMonitor>();
            _soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();

            // FELIX  Fix :P
            _monitor = new CruiserEventMonitor(_cruiserHealthThresholdMonitor, _cruiserDamageMonitor, _soundPlayer, null);
        }

        [Test]
        public void CruiserHealthThresholdReached_PlaysSound()
        {
            _cruiserHealthThresholdMonitor.ThresholdReached += Raise.Event();
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.Cruiser.SignificantlyDamaged);
        }

        [Test]
        public void CruiserDamaged_PlaysSound()
        {
            _cruiserDamageMonitor.CruiserOrBuildingDamaged += Raise.Event();
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.Cruiser.UnderAttack);
        }
    }
}