using System.Collections.Generic;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Tutorial.Steps.BoostSteps;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Tutorial.Steps.Factories.EnemyUnit
{
    public class EnemyBomberStepsFactory : EnemyUnitDefenceStepsFactory
    {
        private readonly ICruiser _aiCruiser;
        private readonly IVariableDelayDeferrer _deferrer;

        protected override IPrefabKey FactoryKey { get { return StaticPrefabKeys.Buildings.AirFactory; } }
        protected override CameraFocuserTarget UnitCameraFocusTarget { get { return CameraFocuserTarget.AICruiser; } }

        private readonly BuildableInfo _unitToBuild;
        protected override BuildableInfo UnitToBuild { get { return _unitToBuild; } }

        private readonly ISingleBuildableProvider _unitBuiltProvider;
        protected override ISingleBuildableProvider UnitBuiltProvider { get { return _unitBuiltProvider; } }

        private readonly BuildableInfo _defenceToBuild;
        protected override BuildableInfo DefenceToBuild { get { return _defenceToBuild; } }

        private readonly SlotSpecification _slotSpecification;
        protected override SlotSpecification SlotSpecification { get { return _slotSpecification; } }

        public EnemyBomberStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            EnemyUnitArgs enemyUnitArgs,
            ICruiser aiCruiser,
            IVariableDelayDeferrer deferrer,
            ISingleBuildableProvider unitBuiltProvider)
            : base(argsFactory, enemyUnitArgs)
        {
            Helper.AssertIsNotNull(aiCruiser, deferrer, unitBuiltProvider);

            _aiCruiser = aiCruiser;
            _deferrer = deferrer;
            _unitBuiltProvider = unitBuiltProvider;
            _unitToBuild = new BuildableInfo(StaticPrefabKeys.Units.Bomber, "bomber");
            _defenceToBuild = new BuildableInfo(StaticPrefabKeys.Buildings.AntiAirTurret, "anti-air turret");
            _slotSpecification = new SlotSpecification(SlotType.Deck, BuildingFunction.AntiAir, preferCruiserFront: true);
        }

        protected override IList<ITutorialStep> CreateSpeedBoostSteps(string unitUpcomingText)
        {
            IBoostProvider boostProvider = new BoostProvider(boostMultiplier: 8);

            return new List<ITutorialStep>()
            {
                new AddAircraftBoostStep(
                    _argsFactory.CreateTutorialStepArgs(unitUpcomingText),
                    _aiCruiser.FactoryProvider.GlobalBoostProviders,
                    boostProvider),

                new DelayWaitStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _deferrer,
                    waitTimeInS: 3.3f),

                new RemoveAircraftBoostStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _aiCruiser.FactoryProvider.GlobalBoostProviders,
                    boostProvider)
            };
        }
    }
}