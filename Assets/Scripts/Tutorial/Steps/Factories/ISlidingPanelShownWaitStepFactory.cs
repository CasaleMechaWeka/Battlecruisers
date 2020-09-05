namespace BattleCruisers.Tutorial.Steps.Factories
{
    public interface ISlidingPanelShownWaitStepFactory
    {
        ITutorialStep CreateInformatorShownWaitStep();
        ITutorialStep CreateSelectorShownWaitStep();
    }
}