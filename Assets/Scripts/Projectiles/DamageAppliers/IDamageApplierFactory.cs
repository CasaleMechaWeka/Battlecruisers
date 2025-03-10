using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Projectiles.DamageAppliers
{
    public interface IDamageApplierFactory
    {
        IDamageStats CreateDamageStats(float damage, float damageRadiusInM);
        IDamageApplier CreateSingleDamageApplier(IDamageStats damageStats);
        IDamageApplier CreateAreaOfDamageApplier(IDamageStats damageStats);
        IDamageApplier CreateFactionSpecificAreaOfDamageApplier(IDamageStats damageStats, Faction enemyFaction);
    }
}
