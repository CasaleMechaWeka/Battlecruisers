using System;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class GameSpeedChangedEventArgs : EventArgs
    {
        public float NewSpeed { get; private set; }

        public GameSpeedChangedEventArgs(float newSpeed)
        {
            NewSpeed = newSpeed;
        }
    }

    public interface IGameSpeedModifier
    {
        event EventHandler<GameSpeedChangedEventArgs> GameSpeedChanged;
    }
}
