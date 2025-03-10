using BattleCruisers.UI.Filters;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public interface IFeaturePermitterStepFactory
    {
        ITutorialStep CreateStep(IPermitter featurePermitter, bool enableFeature);
    }
}