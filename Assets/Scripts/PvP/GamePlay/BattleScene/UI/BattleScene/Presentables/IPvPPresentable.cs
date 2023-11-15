namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Presentables
{
    public interface IPvPPresentable : IPvPDismissableEmitter
    {
        bool IsPresented { get; }

        // About to be shown
        void OnPresenting(object activationParameter);

        // About to be hidden
        void OnDismissing();
    }
}
