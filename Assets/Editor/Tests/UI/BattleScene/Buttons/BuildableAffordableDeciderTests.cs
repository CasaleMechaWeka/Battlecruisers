using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.BattleScene.Buttons;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.Buttons
{
    public class BuildableAffordabeDeciderTests
    {
        private IActivenessDecider<IBuildable> _decider;

        private IDroneManager _droneManager;
        private IBuildable _buildable;

        [SetUp]
        public void SetuUp()
        {
            _droneManager = Substitute.For<IDroneManager>();
            _decider = new BuildableAffordableDecider(_droneManager);

            _buildable = Substitute.For<IBuildable>();
            _buildable.NumOfDronesRequired.Returns(4);
        }

        [Test]
        public void DroneNumChanged_TriggersPotentialActivenessChange()
        {
            int eventCounter = 0;

            _decider.PotentialActivenessChange += (sender, e) => eventCounter++;

            _droneManager.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(newNumOfDrones: 7));

            Assert.AreEqual(1, eventCounter);
        }

        [Test]
        public void ShouldBeEnabled_True()
        {
            _droneManager.CanSupportDroneConsumer(_buildable.NumOfDronesRequired).Returns(true);
            Assert.IsTrue(_decider.ShouldBeEnabled(_buildable));
            _droneManager.Received().CanSupportDroneConsumer(_buildable.NumOfDronesRequired);
        }

        [Test]
        public void ShouldBeEnabled_False()
        {
            _droneManager.CanSupportDroneConsumer(_buildable.NumOfDronesRequired).Returns(false);
            Assert.IsFalse(_decider.ShouldBeEnabled(_buildable));
            _droneManager.Received().CanSupportDroneConsumer(_buildable.NumOfDronesRequired);
        }
    }
}
