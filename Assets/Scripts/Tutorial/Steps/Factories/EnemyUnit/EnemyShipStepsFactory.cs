using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.Factories.EnemyUnit
{
    public class EnemyShipStepsFactory : EnemyUnitDefenceStepsFactory
    {
        protected override IPrefabKey FactoryKey { get { return StaticPrefabKeys.Buildings.NavalFactory; } }
        protected override CameraFocuserTarget UnitCameraFocusTarget { get { return CameraFocuserTarget.AICruiserNavalFactory; } }

        private readonly BuildableInfo _unitToBuild;
        protected override BuildableInfo UnitToBuild { get { return _unitToBuild; } }

        private readonly ISingleBuildableProvider _unitBuiltProvider;
        protected override ISingleBuildableProvider UnitBuiltProvider { get { return _unitBuiltProvider; } }

        private readonly BuildableInfo _defenceToBuild;
        protected override BuildableInfo DefenceToBuild { get { return _defenceToBuild; } }

        private readonly SlotSpecification _slotSpecification;
        protected override SlotSpecification SlotSpecification { get { return _slotSpecification; } }

        public EnemyShipStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ITutorialArgs tutorialArgs,
            EnemyUnitArgs enemyUnitArgs,
            ISingleBuildableProvider unitBuiltProvider)
            : base(argsFactory, tutorialArgs, enemyUnitArgs)
        {
            Helper.AssertIsNotNull(unitBuiltProvider);

            _unitBuiltProvider = unitBuiltProvider;
            _unitToBuild = new BuildableInfo(StaticPrefabKeys.Units.AttackBoat, "attack boat");
            _defenceToBuild = new BuildableInfo(StaticPrefabKeys.Buildings.AntiAirTurret, "anti-ship turret");
            _slotSpecification = new SlotSpecification(SlotType.Deck, BuildingFunction.AntiShip, preferCruiserFront: true);
        }
    }
}