using BattleCruisers.Projectiles.Stats.Wrappers;

namespace BattleCruisers.Projectiles.DamageAppliers
{
    public interface IDamageApplierFactory
    {
        IDamageStats CreateDamageStats(float damage, float damageRadiusInM);
        IDamageApplier CreateSingleDamageApplier(IDamageStats damageStats);
        IDamageApplier CreateAreaOfDamageApplier(IDamageStats damageStats);
    }
}
