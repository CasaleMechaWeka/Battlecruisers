using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.Filters;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.Buttons.Filters
{
    public class AffordableBuildableFilterTests
    {
        private IBroadcastingFilter<IBuildable> _filter;

        private IDroneManager _droneManager;
        private IBuildable _buildable;

        [SetUp]
        public void SetuUp()
        {
            _droneManager = Substitute.For<IDroneManager>();
            _filter = new AffordableBuildableFilter(_droneManager);

            _buildable = Substitute.For<IBuildable>();
            _buildable.NumOfDronesRequired.Returns(4);
        }

        [Test]
        public void DroneNumChanged_TriggersPotentialMatchChange()
        {
            int eventCounter = 0;

            _filter.PotentialMatchChange += (sender, e) => eventCounter++;

            _droneManager.DroneNumChanged += Raise.EventWith(new DroneNumChangedEventArgs(newNumOfDrones: 7));

            Assert.AreEqual(1, eventCounter);
        }

        [Test]
        public void ShouldBeEnabled_True()
        {
            _droneManager.CanSupportDroneConsumer(_buildable.NumOfDronesRequired).Returns(true);
            Assert.IsTrue(_filter.IsMatch(_buildable));
            _droneManager.Received().CanSupportDroneConsumer(_buildable.NumOfDronesRequired);
        }

        [Test]
        public void ShouldBeEnabled_False()
        {
            _droneManager.CanSupportDroneConsumer(_buildable.NumOfDronesRequired).Returns(false);
            Assert.IsFalse(_filter.IsMatch(_buildable));
            _droneManager.Received().CanSupportDroneConsumer(_buildable.NumOfDronesRequired);
        }
    }
}
