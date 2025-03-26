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

        public IList<ITutorialStep> CreateSteps()
        {
            List<ITutorialStep> steps = new List<ITutorialStep>();

            steps.AddRange(_autNavigationStepFactory.CreateSteps(CameraFocuserTarget.AICruiser));

            ITutorialStepArgs args
                = _argsFactory.CreateTutorialStepArgs(
                    LocTableCache.TutorialTable.GetString("Steps/EnemyCruiser"),
                    _aiCruiser);

            steps.Add(_explanationDismissableStepFactory.CreateStep(args));

            return steps;
        }
    }
}