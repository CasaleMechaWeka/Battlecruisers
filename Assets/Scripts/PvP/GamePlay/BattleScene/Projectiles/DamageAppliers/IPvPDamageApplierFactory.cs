using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers
{
    public interface IPvPDamageApplierFactory
    {
        IPvPDamageStats CreateDamageStats(float damage, float damageRadiusInM);
        IPvPDamageApplier CreateSingleDamageApplier(IPvPDamageStats damageStats);
        IPvPDamageApplier CreateAreaOfDamageApplier(IPvPDamageStats damageStats);
        IPvPDamageApplier CreateFactionSpecificAreaOfDamageApplier(IPvPDamageStats damageStats, PvPFaction enemyFaction);
    }
}
