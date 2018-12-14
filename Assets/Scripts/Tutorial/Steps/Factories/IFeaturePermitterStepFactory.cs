using BattleCruisers.UI.Filters;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public interface IFeaturePermitterStepFactory
    {
        ITutorialStep CreateTutorialStep(BroadcastingFilter featurePermitter, bool enableFeature);
    }
}