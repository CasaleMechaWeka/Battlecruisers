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
            _populationLimitMonitor = Substitute.For<IPopulationLimitMonitor>();
            _soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();
            _debouncer = Substitute.For<IDebouncer>();

            // FELIX  Fix :)
            _populationLimitAnnouncer = new PopulationLimitAnnouncer(_populationLimitMonitor, _soundPlayer, _debouncer, null);
        }

        [Test]
        public void PopulationLimitReached()
        {
            Action debouncedAction = null;
            _debouncer.Debounce(Arg.Do<Action>(x => debouncedAction = x));

            // FELIX  Fix :)
            //_populationLimitMonitor.PopulationLimitReached += Raise.Event();

            Assert.IsNotNull(debouncedAction);
            debouncedAction.Invoke();
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.PopulationLimitReached);
        }
    }
}