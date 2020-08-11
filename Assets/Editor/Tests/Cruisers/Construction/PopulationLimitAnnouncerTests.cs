using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions;
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
        private IGameObject _popLimitReachedFeedback;
        private Action _debouncedAction;

        [SetUp]
        public void TestSetup()
        {
            _populationLimitMonitor = Substitute.For<IPopulationLimitMonitor>();
            _soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();
            _debouncer = Substitute.For<IDebouncer>();
            _popLimitReachedFeedback = Substitute.For<IGameObject>();

            _populationLimitAnnouncer = new PopulationLimitAnnouncer(_populationLimitMonitor, _soundPlayer, _debouncer, _popLimitReachedFeedback);

            _debouncer.Debounce(Arg.Do<Action>(x => _debouncedAction = x));
        }

        [Test]
        public void IsPopulationLimitReached_ValueChanged_True()
        {
            _populationLimitMonitor.IsPopulationLimitReached.Value.Returns(true);

            _populationLimitMonitor.IsPopulationLimitReached.ValueChanged += Raise.Event();

            _popLimitReachedFeedback.Received().IsVisible = true;
            Assert.IsNotNull(_debouncedAction);
            _debouncedAction.Invoke();
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.PopulationLimitReached);
        }

        [Test]
        public void IsPopulationLimitReached_ValueChanged_False()
        {
            _populationLimitMonitor.IsPopulationLimitReached.Value.Returns(false);

            _populationLimitMonitor.IsPopulationLimitReached.ValueChanged += Raise.Event();

            _popLimitReachedFeedback.Received().IsVisible = false;
            Assert.IsNull(_debouncedAction);
        }
    }
}