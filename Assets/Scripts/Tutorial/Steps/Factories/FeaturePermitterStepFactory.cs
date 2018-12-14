using BattleCruisers.Tutorial.Steps.FeatureModifierSteps;
using BattleCruisers.UI.Filters;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class FeaturePermitterStepFactory : TutorialFactoryBase, IFeaturePermitterStepFactory
    {
        public FeaturePermitterStepFactory(ITutorialStepArgsFactory argsFactory, ITutorialArgs tutorialArgs) 
            : base(argsFactory, tutorialArgs)
        {
            // empty
        }

        public ITutorialStep CreateTutorialStep(BroadcastingFilter featurePermitter, bool enableFeature)
        {
            return
                new FeaturePermitterStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    featurePermitter,
                    enableFeature);
        }
    }
}