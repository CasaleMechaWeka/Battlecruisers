using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.Tutorial.Steps.Factories.EnemyUnit
{
    public class EnemyShipStepsFactory : EnemyUnitDefenceStepsFactory
    {
        protected override IPrefabKey FactoryKey => StaticPrefabKeys.Buildings.NavalFactory;
        protected override CameraFocuserTarget UnitCameraFocusTarget => CameraFocuserTarget.AICruiserNavalFactory;

        private readonly BuildableInfo _unitToBuild;
        protected override BuildableInfo UnitToBuild => _unitToBuild;

        private readonly ISingleBuildableProvider _unitBuiltProvider;
        protected override ISingleBuildableProvider UnitBuiltProvider => _unitBuiltProvider;

        private readonly BuildableInfo _defenceToBuild;
        protected override BuildableInfo DefenceToBuild => _defenceToBuild;

        private readonly ISlotSpecification _slotSpecification;
        protected override ISlotSpecification SlotSpecification => _slotSpecification;

        public EnemyShipStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ILocTable tutorialStrings,
            EnemyUnitArgs enemyUnitArgs,
            ISingleBuildableProvider unitBuiltProvider,
            IPrefabFactory prefabFactory)
            : base(argsFactory, tutorialStrings, enemyUnitArgs)
        {
            Helper.AssertIsNotNull(unitBuiltProvider);

            _unitBuiltProvider = unitBuiltProvider;

            string attackBoatName = prefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeys.Units.AttackBoat).Buildable.Name;
            _unitToBuild = new BuildableInfo(StaticPrefabKeys.Units.AttackBoat, attackBoatName);

            string shipTurretName = prefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeys.Buildings.AntiShipTurret).Buildable.Name;
            _defenceToBuild = new BuildableInfo(StaticPrefabKeys.Buildings.AntiShipTurret, shipTurretName);

            _slotSpecification = new SlotSpecification(SlotType.Deck, BuildingFunction.AntiShip, preferCruiserFront: true);
        }
    }
}