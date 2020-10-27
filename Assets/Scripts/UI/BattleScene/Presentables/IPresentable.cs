namespace BattleCruisers.UI.BattleScene.Presentables
{
    public interface IPresentable : IDismissableEmitter
    {
        bool IsPresented { get; }

        // About to be shown
        void OnPresenting(object activationParameter);

        // About to be hidden
        void OnDismissing();
    }
}
