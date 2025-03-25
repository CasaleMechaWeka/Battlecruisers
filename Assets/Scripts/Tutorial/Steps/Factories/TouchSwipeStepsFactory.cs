using BattleCruisers.UI.Filters;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class TouchSwipeStepsFactory : SwipeStepsFactoryBase
    {
        public TouchSwipeStepsFactory(
            TutorialStepArgsFactory argsFactory,
            FeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter swipePermitter,
            ExplanationDismissableStepFactory explanationDismissableStepFactory)
            : base(
                  argsFactory,
                  featurePermitterStepFactory,
                  swipePermitter,
                  explanationDismissableStepFactory,
                  "Steps/TouchSwipe")
        {
        }
    }
}