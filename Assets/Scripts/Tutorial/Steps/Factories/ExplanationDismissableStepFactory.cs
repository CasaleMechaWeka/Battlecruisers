using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class ExplanationDismissableStepFactory : TutorialFactoryBase, IExplanationDismissableStepFactory
    {
        private readonly IExplanationDismissButton _dismissButton;

        public ExplanationDismissableStepFactory(ITutorialStepArgsFactory argsFactory, IExplanationDismissButton dismissButton) 
            : base(argsFactory)
        {
            Assert.IsNotNull(dismissButton);
            _dismissButton = dismissButton;
        }

        public ITutorialStep CreateStep(ITutorialStepArgs args)
        {
            return
                new ExplanationDismissableStep(
                    args,
                    _dismissButton);
        }
    }
}