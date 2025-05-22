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
    public class StartConstructingUnitStep : TutorialStep
    {
        private readonly IPrefabKey _unitToConstruct;
        private readonly IItemProvider<IFactory> _factoryProvider;

        public StartConstructingUnitStep(
            TutorialStepArgs args,
            IPrefabKey unitToConstruct,
            IItemProvider<IFactory> factoryProvider)
            : base(args)
        {
            Helper.AssertIsNotNull(unitToConstruct, factoryProvider);

            _unitToConstruct = unitToConstruct;
            _factoryProvider = factoryProvider;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            IBuildableWrapper<IUnit> unitWrapperPrefab = PrefabFactory.GetUnitWrapperPrefab(_unitToConstruct);
            _factoryProvider.FindItem().StartBuildingUnit(unitWrapperPrefab);

            OnCompleted();
        }
    }
}
