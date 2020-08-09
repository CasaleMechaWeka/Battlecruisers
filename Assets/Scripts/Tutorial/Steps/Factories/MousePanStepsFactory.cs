using BattleCruisers.UI.Filters;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class MousePanStepsFactory : SwipeStepsFactoryBase
    {
        public MousePanStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter swipePermitter,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(
                  argsFactory,
                  featurePermitterStepFactory,
                  swipePermitter,
                  explanationDismissableStepFactory,
                  "You can click drag to pan.  Give it a try :)")
        {
        }
   }
}