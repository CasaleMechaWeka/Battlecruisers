using BattleCruisers.Tutorial.Highlighting;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface INavigationButtonsWrapper
    {
        IButton PlayerCruiserButton { get; }
        IButton MidLeftButton { get; }
        IButton OverviewButton { get; }
        IButton MidRightButton { get; }
        IButton AICruiserButton { get; }
    }
}
