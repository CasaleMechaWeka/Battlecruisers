using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class TouchSwipeStepsFactory : SwipeStepsFactoryBase
    {
        public TouchSwipeStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ILocTable tutorialStrings,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter swipePermitter,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(
                  argsFactory,
                  tutorialStrings,
                  featurePermitterStepFactory,
                  swipePermitter,
                  explanationDismissableStepFactory,
                  // FELIX  Loc, key?
                  "Swipe horizontally to scroll. Swipe vertically to zoom.")
        {
        }
   }
}