namespace BattleCruisers.UI.BattleScene.Presentables
{
    public interface IPresentable : IDismissableEmitter
    {
        // About to be shown
        void OnPresenting(object activationParameter);

        // About to be hidden
        void OnDismissing();
    }
}
