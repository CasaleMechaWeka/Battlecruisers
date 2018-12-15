using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class ConstructDroneStationStespFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IConstructBuildingStepsFactory _constructBuildingStepsFactory;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public ConstructDroneStationStespFactory(
            ITutorialStepArgsFactory argsFactory,
            ITutorialArgs tutorialArgs,
            IConstructBuildingStepsFactory constructBuildingStepsFactory,
            IExplanationDismissableStepFactory explanationDismissableStepFactory)
            : base(argsFactory, tutorialArgs)
        {
            Helper.AssertIsNotNull(constructBuildingStepsFactory, explanationDismissableStepFactory);

            _constructBuildingStepsFactory = constructBuildingStepsFactory;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            List<ITutorialStep> steps = new List<ITutorialStep>();

            steps.AddRange(
                _constructBuildingStepsFactory.CreateSteps(
                    BuildingCategory.Factory,
                    new BuildableInfo(StaticPrefabKeys.Buildings.DroneStation, "builder bay"),
                    new SlotSpecification(SlotType.Utility, BuildingFunction.Generic, preferCruiserFront: true),
                    "To get more builders construct a builder bay."));

            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs("Nice!  You have gained 2 builders :D")));

            return steps;
        }
    }
}