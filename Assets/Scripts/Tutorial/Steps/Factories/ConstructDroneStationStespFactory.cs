using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class ConstructDroneStationStespFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IConstructBuildingStepsFactory _constructBuildingStepsFactory;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public ConstructDroneStationStespFactory(
            ITutorialStepArgsFactory argsFactory,
            ILocTable tutorialStrings,
            IConstructBuildingStepsFactory constructBuildingStepsFactory,
            IExplanationDismissableStepFactory explanationDismissableStepFactory)
            : base(argsFactory, tutorialStrings)
        {
            Helper.AssertIsNotNull(constructBuildingStepsFactory, explanationDismissableStepFactory);

            _constructBuildingStepsFactory = constructBuildingStepsFactory;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            List<ITutorialStep> steps = new List<ITutorialStep>();

            // FELIX  Loc
            steps.AddRange(
                _constructBuildingStepsFactory.CreateSteps(
                    BuildingCategory.Factory,
                    new BuildableInfo(StaticPrefabKeys.Buildings.DroneStation, "Builder Bay"),
                    new SlotSpecification(SlotType.Utility, BuildingFunction.Generic, preferCruiserFront: true),
                    "Construct a Builder Bay to make more Builders."));

            steps.Add(
                _explanationDismissableStepFactory.CreateStep(
                    _argsFactory.CreateTutorialStepArgs("Nice!  You have gained two Builders :D")));

            return steps;
        }
    }
}