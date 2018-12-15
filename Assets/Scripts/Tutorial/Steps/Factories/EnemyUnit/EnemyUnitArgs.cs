using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.Factories.EnemyUnit
{
    public class EnemyUnitArgs
    {
        public ICreateProducingFactoryStepsFactory CreateProducingFactoryStepsFactory { get; private set; }
        public IAutoNavigationStepFactory AutoNavigationStepFactory { get; private set; }
        public IExplanationDismissableStepFactory ExplanationDismissableStepFactory { get; private set; }
        public IConstructBuildingStepsFactory ConstructBuildingStepsFactory { get; private set; }
        public IChangeCruiserBuildSpeedStepFactory ChangeCruiserBuildSpeedStepFactory { get; private set; }
        public ITutorialProvider TutorialProvider { get; private set; }

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