namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update
{
    public interface IPvPSwitchableUpdater : IPvPUpdater
    {
        bool Enabled { get; set; }
    }
}