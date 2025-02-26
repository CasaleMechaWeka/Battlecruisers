using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers
{
    public interface IPvPDamageApplierFactory
    {
        IDamageStats CreateDamageStats(float damage, float damageRadiusInM);
        IDamageApplier CreateSingleDamageApplier(IDamageStats damageStats);
        IDamageApplier CreateAreaOfDamageApplier(IDamageStats damageStats);
        IDamageApplier CreateFactionSpecificAreaOfDamageApplier(IDamageStats damageStats, Faction enemyFaction);
    }
}
