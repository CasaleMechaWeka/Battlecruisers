using BattleCruisers.Utils.BattleScene.Update;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update
{
    public interface IPvPSwitchableUpdater : IUpdater
    {
        bool Enabled { get; set; }
    }
}