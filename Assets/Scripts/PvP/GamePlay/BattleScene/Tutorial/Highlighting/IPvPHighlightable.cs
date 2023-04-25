namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting
{
    public interface IPvPHighlightable
    {
        PvPHighlightArgs CreateHighlightArgs(IPvPHighlightArgsFactory highlightArgsFactory);
    }
}
