using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.Factories.EnemyUnit
{
    public class EnemyUnitArgs
    {
        public CreateProducingFactoryStepsFactory CreateProducingFactoryStepsFactory { get; }
        public AutoNavigationStepFactory AutoNavigationStepFactory { get; }
        public ExplanationDismissableStepFactory ExplanationDismissableStepFactory { get; }
        public ConstructBuildingStepsFactory ConstructBuildingStepsFactory { get; }
        public ChangeCruiserBuildSpeedStepFactory ChangeCruiserBuildSpeedStepFactory { get; }
        public TutorialHelper TutorialProvider { get; }

        public EnemyUnitArgs(
           CreateProducingFactoryStepsFactory createProducingFactoryStepsFactory,
           AutoNavigationStepFactory autoNavigationStepFactory,
           ExplanationDismissableStepFactory explanationDismissableStepFactory,
           ConstructBuildingStepsFactory constructBuildingStepsFactory,
           ChangeCruiserBuildSpeedStepFactory changeCruiserBuildSpeedStepFactory,
           TutorialHelper tutorialProvider)
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