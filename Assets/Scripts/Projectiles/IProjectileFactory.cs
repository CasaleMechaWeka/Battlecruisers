using BattleCruisers.Utils.Factories;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
    public interface IProjectileFactory
    {
        ProjectileController CreateBullet(Vector3 spawnPosition, IFactoryProvider factoryProvider);
        ProjectileController CreateShellSmall(Vector3 spawnPosition, IFactoryProvider factoryProvider);
        ProjectileController CreateShellLarge(Vector3 spawnPosition, IFactoryProvider factoryProvider);
        BombController CreateBomb(Vector3 spawnPosition, IFactoryProvider factoryProvider);
        NukeController CreateNuke(Vector3 spawnPosition, IFactoryProvider factoryProvider);
        RocketController CreateRocket(Vector3 spawnPosition, IFactoryProvider factoryProvider);
        MissileController CreateMissileSmall(Vector3 spawnPosition, IFactoryProvider factoryProvider);
        MissileController CreateMissileMedium(Vector3 spawnPosition, IFactoryProvider factoryProvider);
        MissileController CreateMissileLarge(Vector3 spawnPosition, IFactoryProvider factoryProvider);
    }
}