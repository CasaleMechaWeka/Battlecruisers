using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.Factories.EnemyUnit
{
    public class EnemyUnitArgs
    {
        public ICreateProducingFactoryStepsFactory CreateProducingFactoryStepsFactory { get; }
        public IAutoNavigationStepFactory AutoNavigationStepFactory { get; }
        public IExplanationDismissableStepFactory ExplanationDismissableStepFactory { get; }
        public IConstructBuildingStepsFactory ConstructBuildingStepsFactory { get; }
        public IChangeCruiserBuildSpeedStepFactory ChangeCruiserBuildSpeedStepFactory { get; }
        public ITutorialProvider TutorialProvider { get; }

        public EnemyUnitArgs(
           ICreateProducingFactoryStepsFactory createProducingFactoryStepsFactory,
           IAutoNavigationStepFactory autoNavigationStepFactory,
           IExplanationDismissableStepFactory explanationDismissableStepFactory,
           IConstructBuildingStepsFactory constructBuildingStepsFactory,
           IChangeCruiserBuildSpeedStepFactory changeCruiserBuildSpeedStepFactory,
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