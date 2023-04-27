namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Update
{
    public interface IPvPSwitchableUpdater : IPvPUpdater
    {
        bool Enabled { get; set; }
    }
}