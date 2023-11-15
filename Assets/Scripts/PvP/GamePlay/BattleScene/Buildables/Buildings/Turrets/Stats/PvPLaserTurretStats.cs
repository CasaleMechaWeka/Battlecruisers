namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public class PvPLaserTurretStats : PvPTurretStats, IPvPLaserTurretStats
    {
        public float damagePerS;
        public float DamagePerS => damagePerS;

        public float laserDurationInS;
        public float LaserDurationInS => laserDurationInS;
    }
}
