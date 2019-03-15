using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class ExplanationDismissableStepFactory : TutorialFactoryBase, IExplanationDismissableStepFactory
    {
        private readonly IExplanationDismissButton _okButton, _doneButton;

        public ExplanationDismissableStepFactory(
            ITutorialStepArgsFactory argsFactory, 
            IExplanationDismissButton okButton,
            IExplanationDismissButton doneButton) 
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(okButton, doneButton);

            _okButton = okButton;
            _doneButton = doneButton;
        }

        public ITutorialStep CreateStep(ITutorialStepArgs args)
        {
            return new ExplanationDismissableStep(args, _okButton);
        }

        public ITutorialStep CreateStepWithSecondaryButton(ITutorialStepArgs args)
        {
            return new ExplanationDismissableStep(args, _doneButton);
        }
    }
}