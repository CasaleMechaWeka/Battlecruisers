using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Projectiles
{
    public interface IProjectileFactory
    {
        ProjectileController CreateBullet(IFactoryProvider factoryProvider);
        ProjectileController CreateShellSmall(IFactoryProvider factoryProvider);
        ProjectileController CreateShellLarge(IFactoryProvider factoryProvider);
        BombController CreateBomb(IFactoryProvider factoryProvider);
        NukeController CreateNuke(IFactoryProvider factoryProvider);
        RocketController CreateRocket(IFactoryProvider factoryProvider);
        MissileController CreateMissileSmall(IFactoryProvider factoryProvider);
        MissileController CreateMissileMedium(IFactoryProvider factoryProvider);
        MissileController CreateMissileLarge(IFactoryProvider factoryProvider);
    }
}