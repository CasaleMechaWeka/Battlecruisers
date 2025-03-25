using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.Factories.EnemyUnit
{
    public class EnemyUnitArgs
    {
        public ICreateProducingFactoryStepsFactory CreateProducingFactoryStepsFactory { get; }
        public AutoNavigationStepFactory AutoNavigationStepFactory { get; }
        public ExplanationDismissableStepFactory ExplanationDismissableStepFactory { get; }
        public ConstructBuildingStepsFactory ConstructBuildingStepsFactory { get; }
        public ChangeCruiserBuildSpeedStepFactory ChangeCruiserBuildSpeedStepFactory { get; }
        public ITutorialProvider TutorialProvider { get; }

        public EnemyUnitArgs(
           ICreateProducingFactoryStepsFactory createProducingFactoryStepsFactory,
           AutoNavigationStepFactory autoNavigationStepFactory,
           ExplanationDismissableStepFactory explanationDismissableStepFactory,
           ConstructBuildingStepsFactory constructBuildingStepsFactory,
           ChangeCruiserBuildSpeedStepFactory changeCruiserBuildSpeedStepFactory,
           ITutorialProvider tutorialProvider)
        {
            Helper.AssertIsNotNull(
                createProducingFactoryStepsFactory,
                autoNavigationStepFactory,
                explanationDismissableStepFactory,
                constructBuildingStepsFactory,
                changeCruiserBuildSpeedStepFactory,
                tutorialProvider);

            CreateProducingFactoryStepsFactory = createProducingFactoryStepsFactory;
            AutoNavigationStepFactory = autoNavigationStepFactory;
            ExplanationDismissableStepFactory = explanationDismissableStepFactory;
            ConstructBuildingStepsFactory = constructBuildingStepsFactory;
            ChangeCruiserBuildSpeedStepFactory = changeCruiserBuildSpeedStepFactory;
            TutorialProvider = tutorialProvider;
        }
    }
}