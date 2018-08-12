using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.Common.Click;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Common.Click
{
    public class PlayerCruiserDoubleClickHandlerTests
    {
        private IDoubleClickHandler<ICruiser> _handler;
        private ICruiser _clickedCruiser;
        private IDroneManager _droneManager;
        private IRepairManager _repairManager;
        private IDroneConsumer _repairDroneConsumer;
        private IRepairCommand _repairCommand;

        [SetUp]
        public void TestSetup()
        {
            _handler = new PlayerCruiserDoubleClickHandler();

            _droneManager = Substitute.For<IDroneManager>();
            _repairManager = Substitute.For<IRepairManager>();
            _repairCommand = Substitute.For<IRepairCommand>();

            _clickedCruiser = Substitute.For<ICruiser>();
            _clickedCruiser.DroneManager.Returns(_droneManager);
            _clickedCruiser.RepairManager.Returns(_repairManager);
            _clickedCruiser.RepairCommand.Returns(_repairCommand);

            _repairDroneConsumer = Substitute.For<IDroneConsumer>();
            _repairManager.GetDroneConsumer(_clickedCruiser).Returns(_repairDroneConsumer);

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void OnDoubleClick_WrongFaction_Throws()
        {
            _clickedCruiser.Faction.Returns(Faction.Reds);
            Assert.Throws<UnityAsserts.AssertionException>(() => _handler.OnDoubleClick(_clickedCruiser));
        }

        [Test]
        public void OnDoubleClick_RightFaction_CanRepair_Repairs()
        {
            _clickedCruiser.Faction.Returns(Faction.Blues);
            _repairCommand.CanExecute.Returns(true);

            _handler.OnDoubleClick(_clickedCruiser);

            _droneManager.Received().ToggleDroneConsumerFocus(_repairDroneConsumer);
        }


        [Test]
        public void OnDoubleClick_RightFaction_CannotRepair_DoesNothing()
        {
            _clickedCruiser.Faction.Returns(Faction.Blues);
            _repairCommand.CanExecute.Returns(false);

            _handler.OnDoubleClick(_clickedCruiser);

            _droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(null);
        }
    }
}