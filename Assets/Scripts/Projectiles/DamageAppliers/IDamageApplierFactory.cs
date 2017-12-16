using BattleCruisers.Projectiles.Stats.Wrappers;

namespace BattleCruisers.Projectiles.DamageAppliers
{
    public interface IDamageApplierFactory
    {
        IDamageApplier CreateSingleDamageApplier(IDamageStats damageStats);
        IDamageApplier CreateAreaOfDamageApplier(IDamageStats damageStats);
    }
}
