using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Cruisers.Damage
{
    public class CruiserEventMonitorTests
    {
        private CruiserEventMonitor _monitor;
        private IHealthThresholdMonitor _cruiserHealthThresholdMonitor;
        private ICruiserDamageMonitor _cruiserDamageMonitor;
        private ISoundPlayer _soundPlayer;

        [SetUp]
        public void TestSetup()
        {
            _cruiserHealthThresholdMonitor = Substitute.For<IHealthThresholdMonitor>();
            _cruiserDamageMonitor = Substitute.For<ICruiserDamageMonitor>();
            _soundPlayer = Substitute.For<ISoundPlayer>();

            _monitor = new CruiserEventMonitor(_cruiserHealthThresholdMonitor, _cruiserDamageMonitor, _soundPlayer);
        }

        [Test]
        public void CruiserHealthThresholdReached_PlaysSound()
        {
            _cruiserHealthThresholdMonitor.ThresholdReached += Raise.Event();
            _soundPlayer.Received().PlaySound(SoundKeys.Events.CruiserSignificantlyDamaged);
        }

        [Test]
        public void CruiserDamaged_PlaysSound()
        {
            _cruiserDamageMonitor.CruiserOrBuildingDamaged += Raise.Event();
            _soundPlayer.Received().PlaySound(SoundKeys.Events.CruiserUnderAttack);
        }
    }
}