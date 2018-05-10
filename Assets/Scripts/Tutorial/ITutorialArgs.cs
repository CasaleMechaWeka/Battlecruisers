using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.Navigation;

namespace BattleCruisers.Tutorial
{
    public interface ITutorialArgs
    {
        ICruiser PlayerCruiser { get; }
        ICruiser AICruiser { get; }
        INavigationButtonsWrapper NavigationButtonsWrapper { get; }
        IGameSpeedWrapper GameSpeedWrapper { get; }
        ICruiserInfo PlayerCruiserInfo { get; }
        IBuildMenuButtons BuildMenuButtons { get; }
        ITutorialProvider TutorialProvider { get; }
    }
}
