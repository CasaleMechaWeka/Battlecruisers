using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class ExplanationDismissableStepFactory : TutorialFactoryBase, IExplanationDismissableStepFactory
    {
        private readonly IExplanationDismissButton _dismissButton;

        public ExplanationDismissableStepFactory(ITutorialStepArgsFactory argsFactory, ITutorialArgs tutorialArgs, IExplanationDismissButton dismissButton) 
            : base(argsFactory, tutorialArgs)
        {
            Assert.IsNotNull(dismissButton);
            _dismissButton = dismissButton;
        }

        public ITutorialStep CreateTutorialStep(ITutorialStepArgs args)
        {
            return
                new ExplanationDismissableStep(
                    args,
                    _dismissButton);
        }
    }
}