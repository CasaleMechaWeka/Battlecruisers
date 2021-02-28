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
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Tutorial.Steps.Factories.EnemyUnit
{
    public class EnemyBomberStepsFactory : EnemyUnitDefenceStepsFactory
    {
        private readonly ICruiser _aiCruiser;
        private readonly IDeferrer _deferrer;

        protected override IPrefabKey FactoryKey => StaticPrefabKeys.Buildings.AirFactory;
        protected override CameraFocuserTarget UnitCameraFocusTarget => CameraFocuserTarget.AICruiser;

        private readonly BuildableInfo _unitToBuild;
        protected override BuildableInfo UnitToBuild => _unitToBuild;

        private readonly ISingleBuildableProvider _unitBuiltProvider;
        protected override ISingleBuildableProvider UnitBuiltProvider => _unitBuiltProvider;

        private readonly BuildableInfo _defenceToBuild;
        protected override BuildableInfo DefenceToBuild => _defenceToBuild;

        private readonly ISlotSpecification _slotSpecification;
        protected override ISlotSpecification SlotSpecification => _slotSpecification;

        public EnemyBomberStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ILocTable tutorialStrings,
            EnemyUnitArgs enemyUnitArgs,
            ICruiser aiCruiser,
            IDeferrer deferrer,
            ISingleBuildableProvider unitBuiltProvider,
            IPrefabFactory prefabFactory)
            : base(argsFactory, tutorialStrings , enemyUnitArgs)
        {
            Helper.AssertIsNotNull(aiCruiser, deferrer, unitBuiltProvider, prefabFactory);

            _aiCruiser = aiCruiser;
            _deferrer = deferrer;
            _unitBuiltProvider = unitBuiltProvider;

            string bomberName = prefabFactory.GetUnitWrapperPrefab(StaticPrefabKeys.Units.Bomber).Buildable.Name;
            _unitToBuild = new BuildableInfo(StaticPrefabKeys.Units.Bomber, bomberName);

            string airTurretName = prefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeys.Buildings.AntiAirTurret).Buildable.Name;
            _defenceToBuild = new BuildableInfo(StaticPrefabKeys.Buildings.AntiAirTurret, airTurretName);
            
            _slotSpecification = new SlotSpecification(SlotType.Deck, BuildingFunction.AntiAir, preferCruiserFront: true);
        }

        protected override IList<ITutorialStep> CreateSpeedBoostSteps(string unitUpcomingText)
        {
            IBoostProvider boostProvider = new BoostProvider(boostMultiplier: 8);

            return new List<ITutorialStep>()
            {
                new AddAircraftBoostStep(
                    _argsFactory.CreateTutorialStepArgs(unitUpcomingText),
                    _aiCruiser.CruiserSpecificFactories.GlobalBoostProviders,
                    boostProvider),

                new DelayWaitStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _deferrer,
                    waitTimeInS: 3.2f),

                new RemoveAircraftBoostStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _aiCruiser.CruiserSpecificFactories.GlobalBoostProviders,
                    boostProvider)
            };
        }
    }
}