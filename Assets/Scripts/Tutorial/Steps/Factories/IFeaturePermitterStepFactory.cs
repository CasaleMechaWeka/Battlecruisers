using BattleCruisers.UI.Filters;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public interface IFeaturePermitterStepFactory
    {
        ITutorialStep CreateStep(BroadcastingFilter featurePermitter, bool enableFeature);
    }
}