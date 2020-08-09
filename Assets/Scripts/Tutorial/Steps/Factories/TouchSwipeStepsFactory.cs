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
                  "You can also swipe your finger to look around.  Swipe horizontally to scroll.  Swipe vertically to zoom.  Give it a try :)")
        {
        }
   }
}