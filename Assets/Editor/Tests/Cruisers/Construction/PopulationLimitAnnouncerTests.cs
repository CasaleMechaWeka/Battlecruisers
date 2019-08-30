using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.Timers;
using NSubstitute;
using NUnit.Framework;
using System;

namespace BattleCruisers.Tests.Cruisers.Construction
{
    public class PopulationLimitAnnouncerTests
    {
        private PopulationLimitAnnouncer _populationLimitAnnouncer;
        private IPrioritisedSoundPlayer _soundPlayer;
        private IDebouncer _debouncer;
        private IPopulationLimitMonitor _populationLimitMonitor;

        [SetUp]
        public void TestSetup()
        {
            _soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();
            _debouncer = Substitute.For<IDebouncer>();
            _populationLimitMonitor = Substitute.For<IPopulationLimitMonitor>();

            _populationLimitAnnouncer = new PopulationLimitAnnouncer(_soundPlayer, _debouncer, _populationLimitMonitor);
        }

        [Test]
        public void PopulationLimitReached()
        {
            Action debouncedAction = null;
            _debouncer.Debounce(Arg.Do<Action>(x => debouncedAction = x));

            _populationLimitMonitor.PopulationLimitReached += Raise.Event();

            Assert.IsNotNull(debouncedAction);
            debouncedAction.Invoke();
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.PopulationLimitReached);
        }
    }
}