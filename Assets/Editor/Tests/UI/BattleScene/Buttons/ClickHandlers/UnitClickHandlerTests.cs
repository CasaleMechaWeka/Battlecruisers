using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.Buttons.ClickHandlers
{
    public class UnitClickHandlerTests
    {
        private IUnitClickHandler _clickHandler;
        private IUIManager _uiManager;
        private IPrioritisedSoundPlayer _soundPlayer;
        private IBuildableWrapper<IUnit> _unitWrapper;
        private IUnit _unit;
        private IFactory _factory;

        [SetUp]
        public void TestSetup()
        {
            _uiManager = Substitute.For<IUIManager>();
            _soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();
            // FELIX  fIX :P
            _clickHandler = new UnitClickHandler(_uiManager, _soundPlayer, null);

            _unit = Substitute.For<IUnit>();
            _unitWrapper = Substitute.For<IBuildableWrapper<IUnit>>();
            _unitWrapper.Buildable.Returns(_unit);
            _factory = Substitute.For<IFactory>();
        }

        [Test]
        public void HandleUnitClick_CanAffordUnit_SameUnit_IsPaused_ResumesUnit()
        {
            _factory.UnitWrapper.Returns(_unitWrapper);
            _factory.IsUnitPaused.Value.Returns(true);

            bool canAffordUnit = true;
            _clickHandler.HandleClick(canAffordUnit, _unitWrapper, _factory);

            _factory.Received().ResumeBuildingUnit();
        }

        [Test]
        public void HandleUnitClick_CanAffordUnit_SameUnit_IsNotPaused_PausesUnit()
        {
            _factory.UnitWrapper.Returns(_unitWrapper);
            _factory.IsUnitPaused.Value.Returns(false);

            bool canAffordUnit = true;
            _clickHandler.HandleClick(canAffordUnit, _unitWrapper, _factory);

            _factory.Received().PauseBuildingUnit();
        }

        [Test]
        public void HandleUnitClick_CanAffordUnit_NotSameUnit_StartsNewUnit()
        {
            _factory.UnitWrapper.Returns((IBuildableWrapper<IUnit>)null);

            bool canAffordUnit = true;
            _clickHandler.HandleClick(canAffordUnit, _unitWrapper, _factory);

            _factory.Received().StartBuildingUnit(_unitWrapper);
        }

        [Test]
        public void HandleUnitClick_CanAffordUnit_ShowsUnitDetails()
        {
            bool canAffordUnit = true;
            _clickHandler.HandleClick(canAffordUnit, _unitWrapper, _factory);
            _uiManager.Received().ShowUnitDetails(_unit);
        }

        [Test]
        public void HandleUnitClick_CannotAffordUnit_FactoryCompleted_PlaysInsufficientFundsSound()
        {
            bool canAffordUnit = false;
            _factory.BuildableState.Returns(BuildableState.Completed);

            _clickHandler.HandleClick(canAffordUnit, _unitWrapper, _factory);

            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToBuild);
            _uiManager.DidNotReceiveWithAnyArgs().ShowUnitDetails(null);
            _factory.DidNotReceive().StartBuildingUnit(null);
            _factory.DidNotReceive().PauseBuildingUnit();
            _factory.DidNotReceive().ResumeBuildingUnit();
        }

        [Test]
        public void HandleUnitClick_CannotAffordUnit_FactoryNotCompleted_DoesNothing()
        {
            bool canAffordUnit = false;
            _factory.BuildableState.Returns(BuildableState.InProgress);

            _clickHandler.HandleClick(canAffordUnit, _unitWrapper, _factory);

            _soundPlayer.DidNotReceiveWithAnyArgs().PlaySound(null);
            _uiManager.DidNotReceiveWithAnyArgs().ShowUnitDetails(null);
            _factory.DidNotReceive().StartBuildingUnit(null);
            _factory.DidNotReceive().PauseBuildingUnit();
            _factory.DidNotReceive().ResumeBuildingUnit();
        }
    }
}