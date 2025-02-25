using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools
{
    public interface IPvPExplosionPoolProvider
    {
        // Ship turret, air turret, boat direct fire guns
        IPvPPool<IPoolable<Vector3>, Vector3> BulletImpactPool { get; }

        //Steamcopter
        IPvPPool<IPoolable<Vector3>, Vector3> HighCalibreBulletImpactPool { get; }

        // CIWS
        IPvPPool<IPoolable<Vector3>, Vector3> TinyBulletImpactPool { get; }

        // Radius 0.75  => Small shells: Mortar, frigate & destroyer front cannon.
        IPvPPool<IPoolable<Vector3>, Vector3> SmallExplosionsPool { get; }

        // SAM site (variaton of 0.75)
        IPvPPool<IPoolable<Vector3>, Vector3> FlakExplosionsPool { get; }

        // Bomber (variation of 0.75m)
        IPvPPool<IPoolable<Vector3>, Vector3> BombExplosionPool { get; }

        // Radius 1m    => Missiles: Destroyer missiles, archon front launcher missiles
        IPvPPool<IPoolable<Vector3>, Vector3> MediumExplosionsPool { get; }

        // Radius 1m    => MF Missile
        IPvPPool<IPoolable<Vector3>, Vector3> MFExplosionsPool { get; }

        IPvPPool<IPoolable<Vector3>, Vector3> FirecrackerExplosionsPool { get; }

        // Radius 1.5m  => Artillery, rocket launcher, broadsides, archon back missile launcher, archon primary cannon
        IPvPPool<IPoolable<Vector3>, Vector3> LargeExplosionsPool { get; }

        // Nova Shell Radius 2m  => Nova Artillery
        IPvPPool<IPoolable<Vector3>, Vector3> NovaShellImpactPool { get; }

        // RocketShell Radius 1m  => Broadsword RocketShells
        IPvPPool<IPoolable<Vector3>, Vector3> RocketShellImpactPool { get; }

        // Radius 5m    => Nuke
        IPvPPool<IPoolable<Vector3>, Vector3> HugeExplosionsPool { get; }

        // Radius 1.5m  => Five shell cluster
        IPvPPool<IPoolable<Vector3>, Vector3> FiveShellClusterExplosionsPool { get; }


    }
}