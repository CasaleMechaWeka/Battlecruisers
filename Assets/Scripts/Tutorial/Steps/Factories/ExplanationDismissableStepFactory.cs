using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class ExplanationDismissableStepFactory : TutorialFactoryBase
    {
        private readonly ExplanationDismissButton _okButton, _doneButton;

        public ExplanationDismissableStepFactory(
            TutorialStepArgsFactory argsFactory,
            ExplanationDismissButton okButton,
            ExplanationDismissButton doneButton)
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(okButton, doneButton);

            _okButton = okButton;
            _doneButton = doneButton;
        }

        public TutorialStep CreateStep(TutorialStepArgs args)
        {
            return new ExplanationDismissableStep(args, _okButton);
        }

        public TutorialStep CreateStepWithSecondaryButton(TutorialStepArgs args)
        {
            return new ExplanationDismissableStep(args, _doneButton);
        }
    }
}