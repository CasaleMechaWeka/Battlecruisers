using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class ExplanationDismissableStepFactory : TutorialFactoryBase, IExplanationDismissableStepFactory
    {
        private readonly IExplanationDismissButton _okButton, _doneButton;

        public ExplanationDismissableStepFactory(
            ITutorialStepArgsFactory argsFactory,
            ILocTable tutorialStrings,
            IExplanationDismissButton okButton,
            IExplanationDismissButton doneButton) 
            : base(argsFactory, tutorialStrings)
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