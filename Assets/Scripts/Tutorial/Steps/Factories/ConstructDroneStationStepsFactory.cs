using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class ConstructDroneStationStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly ConstructBuildingStepsFactory _constructBuildingStepsFactory;
        private readonly ExplanationDismissableStepFactory _explanationDismissableStepFactory;
        private readonly PrefabFactory _prefabFactory;

        public ConstructDroneStationStepsFactory(
            TutorialStepArgsFactory argsFactory,
            ConstructBuildingStepsFactory constructBuildingStepsFactory,
            ExplanationDismissableStepFactory explanationDismissableStepFactory,
            PrefabFactory prefabFactory)
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(constructBuildingStepsFactory, explanationDismissableStepFactory, prefabFactory);

            _constructBuildingStepsFactory = constructBuildingStepsFactory;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
            _prefabFactory = prefabFactory;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            List<ITutorialStep> steps = new List<ITutorialStep>();

            string builderBayName = _prefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeys.Buildings.DroneStation).Buildable.Name;
            string promptBase = LocTableCache.TutorialTable.GetString("Steps/ConstructDroneStation/Prompt");

            steps.AddRange(
                _constructBuildingStepsFactory.CreateSteps(
                    BuildingCategory.Factory,
                    new BuildableInfo(StaticPrefabKeys.Buildings.DroneStation, builderBayName),
                    new SlotSpecification(SlotType.Utility, BuildingFunction.Generic, preferCruiserFront: true),
                    string.Format(promptBase, builderBayName)));

            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs(
                        LocTableCache.TutorialTable.GetString("Steps/ConstructDroneStation/CompletionMessage"))));

            return steps;
        }
    }
}