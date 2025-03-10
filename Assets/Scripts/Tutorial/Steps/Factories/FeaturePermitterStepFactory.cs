using BattleCruisers.Tutorial.Steps.FeatureModifierSteps;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class FeaturePermitterStepFactory : TutorialFactoryBase, IFeaturePermitterStepFactory
    {
        public FeaturePermitterStepFactory(ITutorialStepArgsFactory argsFactory, ILocTable tutorialStrings) 
            : base(argsFactory, tutorialStrings)
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