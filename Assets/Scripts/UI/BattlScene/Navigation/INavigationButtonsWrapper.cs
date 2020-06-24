namespace BattleCruisers.UI.BattleScene.Navigation
{
    // FELIX Leftover from pre-NW :D  Amazing!!!
	public interface INavigationButtonsWrapper
    {
        IButton PlayerCruiserButton { get; }
        IButton MidLeftButton { get; }
        IButton OverviewButton { get; }
        IButton MidRightButton { get; }
        IButton AICruiserButton { get; }
    }
}
