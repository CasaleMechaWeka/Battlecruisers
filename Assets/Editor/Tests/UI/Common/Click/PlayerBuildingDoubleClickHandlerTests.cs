using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.Commands;
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
        private IDroneFocuser _droneFocuser;
        private IRepairManager _repairManager;
        private IBuilding _clickedBuilding;
        private IDroneConsumer _repairDroneConsumer;
        private IRepairCommand _repairCommand;
        private ICommand _toggleDroneFocusCommand;

        [SetUp]
        public void TestSetup()
        {
            _handler = new PlayerBuildingDoubleClickHandler();

            _droneFocuser = Substitute.For<IDroneFocuser>();
            _repairManager = Substitute.For<IRepairManager>();

            _parentCruiser = Substitute.For<ICruiser>();
            _parentCruiser.DroneFocuser.Returns(_droneFocuser);
            _parentCruiser.RepairManager.Returns(_repairManager);

            _repairCommand = Substitute.For<IRepairCommand>();
            _toggleDroneFocusCommand = Substitute.For<ICommand>();

            _clickedBuilding = Substitute.For<IBuilding>();
            _clickedBuilding.ParentCruiser.Returns(_parentCruiser);
            _clickedBuilding.RepairCommand.Returns(_repairCommand);
            _clickedBuilding.ToggleDroneConsumerFocusCommand.Returns(_toggleDroneFocusCommand);

            _repairDroneConsumer = Substitute.For<IDroneConsumer>();
            _repairManager.GetDroneConsumer(_clickedBuilding).Returns(_repairDroneConsumer);
        }

        [Test]
        public void OnDoubleClick_WrongFaction_Throws()
        {
            _clickedBuilding.Faction.Returns(Faction.Reds);
            Assert.Throws<UnityAsserts.AssertionException>(() => _handler.OnDoubleClick(_clickedBuilding));
        }

        [Test]
        public void OnDoubleClick_RightFaction_CanToggleDroneFocus_TogglesDroneFocus()
        {
            _clickedBuilding.Faction.Returns(Faction.Blues);
            _toggleDroneFocusCommand.CanExecute.Returns(true);

            _handler.OnDoubleClick(_clickedBuilding);

            _toggleDroneFocusCommand.Received().Execute();
        }

        [Test]
        public void OnDoubleClick_RightFaction_CannotToggleDroneFocus_CanRepair_TogglesRepairDrones()
        {
            _clickedBuilding.Faction.Returns(Faction.Blues);
            _toggleDroneFocusCommand.CanExecute.Returns(false);
            _repairCommand.CanExecute.Returns(true);

            _handler.OnDoubleClick(_clickedBuilding);

            _droneFocuser.Received().ToggleDroneConsumerFocus(_repairDroneConsumer, isTriggeredByPlayer: true);
        }

        [Test]
        public void OnDoubleClick_RightFaction_CannotToggleDroneFocus_CannotRepair_DoesNothing()
        {
            _clickedBuilding.Faction.Returns(Faction.Blues);
            _toggleDroneFocusCommand.CanExecute.Returns(false);
            _repairCommand.CanExecute.Returns(false);

            _handler.OnDoubleClick(_clickedBuilding);

            _droneFocuser.DidNotReceiveWithAnyArgs().ToggleDroneConsumerFocus(null, default);
        }
    }
}