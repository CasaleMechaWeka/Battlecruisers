using BattleCruisers.UI.BattleScene;
using BattleCruisers.Utils.BattleScene.Lifetime;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Utils.BattleScene.Lifetime
{
    public class LifetimeManagerTests
    {
        private LifetimeManager _lifetimeManager;
        private ILifetimeEventBroadcaster _lifetimeEvents;
        private IMainMenuManager _mainMenuManager;

        [SetUp]
        public void TestSetup()
        {
            _lifetimeEvents = Substitute.For<ILifetimeEventBroadcaster>();
            _mainMenuManager = Substitute.For<IMainMenuManager>();

            _lifetimeManager = new LifetimeManager(_lifetimeEvents, _mainMenuManager);
        }

        [Test]
        public void IsPaused_ValueChanged_True()
        {
            _lifetimeEvents.IsPaused.Value.Returns(true);
            _lifetimeEvents.IsPaused.ValueChanged += Raise.Event();

            _mainMenuManager.Received().ShowMenu();
        }

        [Test]
        public void IsPaused_ValueChanged_False()
        {
            _lifetimeEvents.IsPaused.Value.Returns(false);
            _lifetimeEvents.IsPaused.ValueChanged += Raise.Event();

            _mainMenuManager.DidNotReceive().ShowMenu();
        }
    }
}