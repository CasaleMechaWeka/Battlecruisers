using BattleCruisers.UI.Filters;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class TouchSwipeStepsFactory : SwipeStepsFactoryBase
    {
        public TouchSwipeStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter swipePermitter,
            IExplanationDismissableStepFactory explanationDismissableStepFactory)
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