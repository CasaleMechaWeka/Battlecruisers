namespace BattleCruisers.Tutorial.Steps.Factories
{
    public interface ITutorialStepFromArgsFactory
    {
        ITutorialStep CreateTutorialStep(ITutorialStepArgs args);
    }
}
