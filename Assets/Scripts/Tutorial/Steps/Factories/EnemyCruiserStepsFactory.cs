using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class EnemyCruiserStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly ICruiser _aiCruiser;
        private readonly IAutoNavigationStepFactory _autNavigationStepFactory;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public EnemyCruiserStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ILocTable tutorialStrings,
            ICruiser aiCruiser,
            IAutoNavigationStepFactory autoNavigationStepFactory,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(argsFactory, tutorialStrings)
        {
            Helper.AssertIsNotNull(aiCruiser, autoNavigationStepFactory, explanationDismissableStepFactory);

            _aiCruiser = aiCruiser;
            _autNavigationStepFactory = autoNavigationStepFactory;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            List<ITutorialStep> steps = new List<ITutorialStep>();

            steps.AddRange(_autNavigationStepFactory.CreateSteps(CameraFocuserTarget.AICruiser));

            ITutorialStepArgs args
                = _argsFactory.CreateTutorialStepArgs(
                    _tutorialStrings.GetString("Steps/EnemyCruiser"),
                    _aiCruiser);

            steps.Add(_explanationDismissableStepFactory.CreateStep(args));

            return steps;
        }
    }
}