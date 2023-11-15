namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public interface IPvPLaserTurretStats : IPvPTurretStats
    {
        float DamagePerS { get; }
        float LaserDurationInS { get; }
    }
}
