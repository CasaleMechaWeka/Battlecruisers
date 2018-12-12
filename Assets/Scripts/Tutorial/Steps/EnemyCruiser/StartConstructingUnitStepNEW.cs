using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    public class StartConstructingUnitStepNEW : TutorialStepNEW
    {
        private readonly IPrefabKey _unitToConstruct;
        private readonly IPrefabFactory _prefabFactory;
        private readonly IItemProvider<IFactory> _factoryProvider;

        public StartConstructingUnitStepNEW(
            ITutorialStepArgsNEW args,
            IPrefabKey unitToConstruct,
            IPrefabFactory prefabFactory,
            IItemProvider<IFactory> factoryProvider)
            : base(args)
        {
            Helper.AssertIsNotNull(unitToConstruct, prefabFactory, factoryProvider);

            _unitToConstruct = unitToConstruct;
            _prefabFactory = prefabFactory;
            _factoryProvider = factoryProvider;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            IBuildableWrapper<IUnit> unitWrapperPrefab = _prefabFactory.GetUnitWrapperPrefab(_unitToConstruct);
            _factoryProvider.FindItem().StartBuildingUnit(unitWrapperPrefab);

            OnCompleted();
        }
    }
}
