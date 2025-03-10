namespace BattleCruisers.Tutorial.Steps.Factories
{
    public interface ISlidingPanelWaitStepFactory
    {
        ITutorialStep CreateInformatorShownWaitStep();
        ITutorialStep CreateSelectorShownWaitStep();
        ITutorialStep CreateSelectorHiddenWaitStep();
    }
}