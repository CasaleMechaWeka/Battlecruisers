using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.Common.Click;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Common.Click
{
    public class PlayerBuildingDoubleClickHandlerTests
    {
        private IDoubleClickHandler<IBuilding> _handler;
        private ICruiser _parentCruiser;
        private IDroneManager _droneManager;
        private IRepairManager _repairManager;
        private IBuilding _clickedBuilding;
        private IDroneConsumer _constructionDroneConsumer, _repairDroneConsumer;
        private IRepairCommand _repairCommand;

        [SetUp]
        public void TestSetup()
        {
            _handler = new PlayerBuildingDoubleClickHandler();

            _droneManager = Substitute.For<IDroneManager>();
            _repairManager = Substitute.For<IRepairManager>();

            _parentCruiser = Substitute.For<ICruiser>();
            _parentCruiser.DroneManager.Returns(_droneManager);
            _parentCruiser.RepairManager.Returns(_repairManager);

            _constructionDroneConsumer = Substitute.For<IDroneConsumer>();
            _repairCommand = Substitute.For<IRepairCommand>();

            _clickedBuilding = Substitute.For<IBuilding>();
            _clickedBuilding.ParentCruiser.Returns(_parentCruiser);
            _clickedBuilding.RepairCommand.Returns(_repairCommand);

            _repairDroneConsumer = Substitute.For<IDroneConsumer>();
            _repairManager.GetDroneConsumer(_clickedBuilding).Returns(_repairDroneConsumer);

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void OnDoubleClick_WrongFaction_Throws()
        {
            _clickedBuilding.Faction.Returns(Faction.Reds);
            Assert.Throws<UnityAsserts.AssertionException>(() => _handler.OnDoubleClick(_clickedBuilding));
        }

        [Test]
        public void OnDoubleClick_RightFaction_HasDroneConsumer_TogglesDroneFocus()
        {
            _clickedBuilding.Faction.Returns(Faction.Blues);
            _clickedBuilding.DroneConsumer.Returns(_constructionDroneConsumer);

            _handler.OnDoubleClick(_clickedBuilding);

            _droneManager.Received().ToggleDroneConsumerFocus(_constructionDroneConsumer);
        }

        [Test]
        public void OnDoubleClick_RightFaction_NoDroneConsumer_CanRepair_TogglesRepairDrones()
        {
            _clickedBuilding.Faction.Returns(Faction.Blues);
            _clickedBuilding.DroneConsumer.Returns((IDroneConsumer)null);
            _repairCommand.CanExecute.Returns(true);

            _handler.OnDoubleClick(_clickedBuilding);

            _droneManager.Received().ToggleDroneConsumerFocus(_repairDroneConsumer);
        }

        [Test]
        public void OnDoubleClick_RightFaction_NoDroneConsumer_CannotRepair_DoesNothing()
        {
            _clickedBuilding.Faction.Returns(Faction.Blues);
            _clickedBuilding.DroneConsumer.Returns((IDroneConsumer)null);
            _repairCommand.CanExecute.Returns(false);

            _handler.OnDoubleClick(_clickedBuilding);

            _droneManager.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(null);
        }
    }
}