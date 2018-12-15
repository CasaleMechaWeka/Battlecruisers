namespace BattleCruisers.Tutorial.Steps.Factories
{
    public interface IExplanationDismissableStepFactory
    {
        ITutorialStep CreateStep(ITutorialStepArgs args);
    }
}
