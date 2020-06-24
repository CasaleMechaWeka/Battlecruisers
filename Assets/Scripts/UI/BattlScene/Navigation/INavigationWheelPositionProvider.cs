using BattleCruisers.UI.Cameras.Targets;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface INavigationWheelPositionProvider
    {
        ICameraTarget PlayerCruiserTarget { get; }
        ICameraTarget PlayerCruiserDeathTarget { get; }
        ICameraTarget PlayerCruiserNukedTarget { get; }
        ICameraTarget PlayerNavalFactoryTarget { get; }

        ICameraTarget AICruiserTarget { get; }
        ICameraTarget AICruiserDeathTarget { get; }
        ICameraTarget AICruiserNukedTarget { get; }
        ICameraTarget AINavalFactoryTarget { get; }

        ICameraTarget MidLeftTarget { get; }
        ICameraTarget OverviewTarget { get; }
    }
}