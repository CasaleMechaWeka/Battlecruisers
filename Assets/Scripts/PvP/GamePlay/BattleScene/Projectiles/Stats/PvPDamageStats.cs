namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats
{
    public class PvPDamageStats : IPvPDamageStats
    {
        public float Damage { get; }
        public float DamageRadiusInM { get; }

        public PvPDamageStats(float damage, float damageRadiusInM)
        {
            Damage = damage;
            DamageRadiusInM = damageRadiusInM;
        }
    }
}
