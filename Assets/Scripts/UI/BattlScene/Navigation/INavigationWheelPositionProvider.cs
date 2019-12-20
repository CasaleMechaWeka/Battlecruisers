using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface INavigationWheelPositionProvider
    {
        Vector2 PlayerCruiserPosition { get; }
        Vector2 PlayerCruiserDeathPosition { get; }
        Vector2 PlayerNavalFactoryPosition { get; }
        Vector2 AICruiserPosition { get; }
        Vector2 AICruiserDeathPosition { get; }
        Vector2 AINavalFactoryPosition { get; }
        Vector2 MidLeftPosition { get; }
        Vector2 OverviewPosition { get; }
    }
}