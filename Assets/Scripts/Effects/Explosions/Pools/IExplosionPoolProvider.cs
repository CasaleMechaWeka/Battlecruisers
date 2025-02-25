using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions.Pools
{
    public interface IExplosionPoolProvider
    {
        // Ship turret, air turret, boat direct fire guns
        IPool<IPoolable<Vector3>, Vector3> BulletImpactPool { get; }

        //Steamcopter
        IPool<IPoolable<Vector3>, Vector3> HighCalibreBulletImpactPool { get; }

        // CIWS
        IPool<IPoolable<Vector3>, Vector3> TinyBulletImpactPool { get; }

        // Radius 0.75  => Small shells: Mortar, frigate & destroyer front cannon.
        IPool<IPoolable<Vector3>, Vector3> SmallExplosionsPool { get; }

        // SAM site (variaton of 0.75)
        IPool<IPoolable<Vector3>, Vector3> FlakExplosionsPool { get; }

        // Bomber (variation of 0.75m)
        IPool<IPoolable<Vector3>, Vector3> BombExplosionPool { get; }

        // Radius 1m    => Missiles: Destroyer missiles, archon front launcher missiles
        IPool<IPoolable<Vector3>, Vector3> MediumExplosionsPool { get; }

        // Radius 2m    => Firecracker warheads
        IPool<IPoolable<Vector3>, Vector3> FirecrackerExplosionsPool { get; }

        // Missile Fighter missile
        IPool<IPoolable<Vector3>, Vector3> MFExplosionsPool { get; }

        // Radius 1.5m  => Artillery, rocket launcher, broadsides, archon back missile launcher, archon primary cannon
        IPool<IPoolable<Vector3>, Vector3> LargeExplosionsPool { get; }

        // Nova Shell Radius 2m  => Nova Artillery
        IPool<IPoolable<Vector3>, Vector3> NovaShellImpactPool { get; }

        // RocketShell Radius 1m  => Broadsword RocketShells
        IPool<IPoolable<Vector3>, Vector3> RocketShellImpactPool { get; }

        // Radius 5m    => Nuke
        IPool<IPoolable<Vector3>, Vector3> HugeExplosionsPool { get; }

        // Radius 1.5m  => Five shell cluster
        IPool<IPoolable<Vector3>, Vector3> FiveShellClusterExplosionsPool { get; }


    }
}