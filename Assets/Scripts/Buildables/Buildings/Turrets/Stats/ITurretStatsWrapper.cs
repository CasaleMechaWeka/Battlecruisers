namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    /// <summary>
    /// Wraps a TurretStats object.  Allows this underlying TurretStats object
    /// to be switched (eg: from normal TurretStats to BoostedTurretStats) without
    /// the consumer noticing.
    /// </summary>
    public interface ITurretStatsWrapper : ITurretStats
    {
        ITurretStats TurretStats { set; }
    }
}
