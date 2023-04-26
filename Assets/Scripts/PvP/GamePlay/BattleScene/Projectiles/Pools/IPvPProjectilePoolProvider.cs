using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public interface IPvPProjectilePoolProvider
    {
        IPvPPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> BulletsPool { get; }
        IPvPPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> HighCalibreBulletsPool { get; }
        IPvPPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> TinyBulletsPool { get; }
        IPvPPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> ShellsLargePool { get; }
        IPvPPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> NovaShellPool { get; }
        IPvPPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> RocketShellPool { get; }
        IPvPPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> ShellsSmallPool { get; }
        IPvPPool<PvPBombController, PvPProjectileActivationArgs<IPvPProjectileStats>> BombsPool { get; }
        IPvPPool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>> RocketsPool { get; }
        IPvPPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> MissilesSmallPool { get; }
        IPvPPool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>> RocketsSmallPool { get; }
        IPvPPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> MissilesMediumPool { get; }
        IPvPPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> MissilesLargePool { get; }
        IPvPPool<PvPSmartMissileController, PvPSmartMissileActivationArgs<IPvPSmartProjectileStats>> MissilesSmartPool { get; }

    }
}