using BattleCruisers.Tutorial.Highlighting;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface INavigationButtonsWrapper : IHighlightable
    {
        IButton PlayerCruiserButton { get; }
        IButton AICruiserButton { get; }
    }
}
