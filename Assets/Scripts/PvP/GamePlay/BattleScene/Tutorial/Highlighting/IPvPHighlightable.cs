using BattleCruisers.Tutorial.Highlighting;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting
{
    public interface IPvPHighlightable
    {
        HighlightArgs CreateHighlightArgs(IPvPHighlightArgsFactory highlightArgsFactory);
    }
}
