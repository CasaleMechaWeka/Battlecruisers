using BattleCruisers.Tutorial.Steps.FeatureModifierSteps;
using BattleCruisers.UI.Filters;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class FeaturePermitterStepFactory : TutorialFactoryBase
    {
        public FeaturePermitterStepFactory(TutorialStepArgsFactory argsFactory)
            : base(argsFactory)
        {
            // empty
        }

        public ITutorialStep CreateStep(IPermitter featurePermitter, bool enableFeature)
        {
            return
                new FeaturePermitterStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    featurePermitter,
                    enableFeature);
        }
    }
}