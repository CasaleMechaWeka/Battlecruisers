namespace BattleCruisers.Tutorial.Steps.Factories
{
    public interface IExplanationDismissableStepFactory
    {
        ITutorialStep CreateTutorialStep(ITutorialStepArgs args);
    }
}
