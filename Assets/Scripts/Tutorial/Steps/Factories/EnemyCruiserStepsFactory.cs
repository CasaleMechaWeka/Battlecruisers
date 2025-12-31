using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class EnemyCruiserStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly ICruiser _aiCruiser;
        private readonly AutoNavigationStepFactory _autNavigationStepFactory;
        private readonly ExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public EnemyCruiserStepsFactory(
            TutorialStepArgsFactory argsFactory,
            ICruiser aiCruiser,
            AutoNavigationStepFactory autoNavigationStepFactory,
            ExplanationDismissableStepFactory explanationDismissableStepFactory)
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(aiCruiser, autoNavigationStepFactory, explanationDismissableStepFactory);

            _aiCruiser = aiCruiser;
            _autNavigationStepFactory = autoNavigationStepFactory;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<TutorialStep> CreateSteps()
        {
            List<TutorialStep> steps = new List<TutorialStep>();

            steps.AddRange(_autNavigationStepFactory.CreateSteps(CameraFocuserTarget.AICruiser));

            TutorialStepArgs args
                = _argsFactory.CreateTutorialStepArgs(
                    LocTableCache.TutorialTable.GetString("Steps/EnemyCruiser"),
                    _aiCruiser);

            steps.Add(_explanationDismissableStepFactory.CreateStep(args));

            return steps;
        }
    }
}