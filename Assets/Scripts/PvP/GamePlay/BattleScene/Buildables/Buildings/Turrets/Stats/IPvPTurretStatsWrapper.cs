using BattleCruisers.Buildables.Buildings.Turrets.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    /// <summary>
    /// Wraps a TurretStats object.  Allows this underlying TurretStats object
    /// to be switched (eg: from normal TurretStats to BoostedTurretStats) without
    /// the consumer noticing.
    /// </summary>
    public interface IPvPTurretStatsWrapper : ITurretStats
    {
        ITurretStats pvpTurretStats { set; }
    }
}
