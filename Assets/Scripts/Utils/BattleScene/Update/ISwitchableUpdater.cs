namespace BattleCruisers.Utils.BattleScene.Update
{
    public interface ISwitchableUpdater : IUpdater
    {
        bool Enabled { get; set; }
    }
}