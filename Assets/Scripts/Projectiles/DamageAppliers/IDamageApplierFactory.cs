using BattleCruisers.Projectiles.Stats.Wrappers;

namespace BattleCruisers.Projectiles.DamageAppliers
{
    public interface IDamageApplierFactory
    {
        IDamageApplier CreateSingleDamageApplier(IProjectileStats projectileStats);
        IDamageApplier CreateAreaOfDamageApplier(IProjectileStats projectileStats);
    }
}
