namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.InGameHints
{
    public interface IPvPHintDisplayer
    {
        void ShowHint(string hint);

        /// <summary>
        /// If a hint is displayed and it matches the given hint, hide the hint.
        /// </summary>
        void HideHint(string hint);
        void HideAllHints();
    }
}