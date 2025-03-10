using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.BuildProgress
{
    public class UnitBuildProgressTriggerTests
    {
        private IUnitBuildProgressTrigger _unitBuildProgressTrigger;
        private IUnitBuildProgress _unitBuildProgress;
        private IFactory _factory;

        [SetUp]
        public void TestSetup()
        {
            _unitBuildProgress = Substitute.For<IUnitBuildProgress>();
            _unitBuildProgressTrigger = new UnitBuildProgressTrigger(_unitBuildProgress);

            _factory = Substitute.For<IFactory>();
        }

        [Test]
        public void SetFactory_Null()
        {
            _unitBuildProgressTrigger.Factory = null;
            _unitBuildProgress.DidNotReceiveWithAnyArgs().ShowBuildProgressIfNecessary(default, default);
        }

        [Test]
        public void SetFactory_NonNull()
        {
            _unitBuildProgressTrigger.Factory = _factory;
            _unitBuildProgress.Received().ShowBuildProgressIfNecessary(_factory.UnitUnderConstruction, _factory);
        }

        [Test]
        public void SetFactory_Unsubscribes()
        {
            // Subcribe to factory events
            _unitBuildProgressTrigger.Factory = _factory;
            _unitBuildProgress.ClearReceivedCalls();

            // Unsubcribe from factory events
            _unitBuildProgressTrigger.Factory = null;
            
            _factory.NewUnitChosen += Raise.Event();

            _unitBuildProgress.DidNotReceiveWithAnyArgs().ShowBuildProgressIfNecessary(default, default);
        }

        [Test]
        public void _factory_UnitStarted()
        {
            IUnit startedUnit = Substitute.For<IUnit>();
            _unitBuildProgressTrigger.Factory = _factory;
            _unitBuildProgress.ClearReceivedCalls();

            _factory.UnitStarted += Raise.EventWith(new UnitStartedEventArgs(startedUnit));

            _unitBuildProgress.Received().ShowBuildProgressIfNecessary(startedUnit, _factory);
        }

        [Test]
        public void _factory_NewUnitChosen()
        {
            _unitBuildProgressTrigger.Factory = _factory;
            _unitBuildProgress.ClearReceivedCalls();

            _factory.NewUnitChosen += Raise.Event();

            _unitBuildProgress.Received().ShowBuildProgressIfNecessary(_factory.UnitWrapper.Buildable, _factory);
        }

        [Test]
        public void _factory_NewUnitChosen_NullUnitWrapper()
        {
            _unitBuildProgressTrigger.Factory = _factory;
            _unitBuildProgress.ClearReceivedCalls();
            _factory.UnitWrapper.Buildable.Returns((IUnit)null);

            _factory.NewUnitChosen += Raise.Event();

            _unitBuildProgress.Received().ShowBuildProgressIfNecessary(null, _factory);
        }

        [Test]
        public void _factory_UnitUnderConstructionDestroyed()
        {
            _unitBuildProgressTrigger.Factory = _factory;
            _unitBuildProgress.ClearReceivedCalls();

            _factory.UnitUnderConstructionDestroyed += Raise.Event();

            _unitBuildProgress.Received().ShowBuildProgressIfNecessary(_factory.UnitWrapper.Buildable, _factory);
        }

        [Test]
        public void _factory_UnitUnderConstructionDestroyed_NullUnitWrapper()
        {
            _unitBuildProgressTrigger.Factory = _factory;
            _unitBuildProgress.ClearReceivedCalls();
            _factory.UnitWrapper.Buildable.Returns((IUnit)null);

            _factory.UnitUnderConstructionDestroyed += Raise.Event();

            _unitBuildProgress.Received().ShowBuildProgressIfNecessary(null, _factory);
        }
    }
}