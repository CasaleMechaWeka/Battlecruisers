using BattleCruisers.Tutorial.Highlighting;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface INavigationButtonsWrapper : IHighlightable
    {
        IHighlightable PlayerCruiserButton { get; }
        IHighlightable AICruiserButton { get; }
    }
}
