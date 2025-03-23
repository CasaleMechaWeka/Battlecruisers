using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions.Pools
{
    public interface IExplosionPoolProvider
    {
        // Ship turret, air turret, boat direct fire guns
        Pool<IPoolable<Vector3>, Vector3> BulletImpactPool { get; }

        //Steamcopter
        Pool<IPoolable<Vector3>, Vector3> HighCalibreBulletImpactPool { get; }

        // CIWS
        Pool<IPoolable<Vector3>, Vector3> TinyBulletImpactPool { get; }

        // Radius 0.75  => Small shells: Mortar, frigate & destroyer front cannon.
        Pool<IPoolable<Vector3>, Vector3> SmallExplosionsPool { get; }

        // SAM site (variaton of 0.75)
        Pool<IPoolable<Vector3>, Vector3> FlakExplosionsPool { get; }

        // Bomber (variation of 0.75m)
        Pool<IPoolable<Vector3>, Vector3> BombExplosionPool { get; }

        // Radius 1m    => Missiles: Destroyer missiles, archon front launcher missiles
        Pool<IPoolable<Vector3>, Vector3> MediumExplosionsPool { get; }

        // Radius 2m    => Firecracker warheads
        Pool<IPoolable<Vector3>, Vector3> FirecrackerExplosionsPool { get; }

        // Missile Fighter missile
        Pool<IPoolable<Vector3>, Vector3> MFExplosionsPool { get; }

        // Radius 1.5m  => Artillery, rocket launcher, broadsides, archon back missile launcher, archon primary cannon
        Pool<IPoolable<Vector3>, Vector3> LargeExplosionsPool { get; }

        // Nova Shell Radius 2m  => Nova Artillery
        Pool<IPoolable<Vector3>, Vector3> NovaShellImpactPool { get; }

        // RocketShell Radius 1m  => Broadsword RocketShells
        Pool<IPoolable<Vector3>, Vector3> RocketShellImpactPool { get; }

        // Radius 5m    => Nuke
        Pool<IPoolable<Vector3>, Vector3> HugeExplosionsPool { get; }

        // Radius 1.5m  => Five shell cluster
        Pool<IPoolable<Vector3>, Vector3> FiveShellClusterExplosionsPool { get; }


    }
}