namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public interface IGameSpeedWrapper
    {
        IButton SlowMotionButton { get; }
        IButton PlayButton { get; }
        IButton FastForwardButton { get; }
        IButton DoubleFastForwardButton { get; }
    }
}
