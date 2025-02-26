using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Projectiles.DamageAppliers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers
{
    public interface IPvPDamageApplierFactory
    {
        IPvPDamageStats CreateDamageStats(float damage, float damageRadiusInM);
        IDamageApplier CreateSingleDamageApplier(IPvPDamageStats damageStats);
        IDamageApplier CreateAreaOfDamageApplier(IPvPDamageStats damageStats);
        IDamageApplier CreateFactionSpecificAreaOfDamageApplier(IPvPDamageStats damageStats, Faction enemyFaction);
    }
}
