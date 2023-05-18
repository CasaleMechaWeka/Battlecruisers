using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools
{
    public interface IPvPExplosionPoolProvider
    {
        // Ship turret, air turret, boat direct fire guns
        IPvPPool<IPvPExplosion, Vector3> BulletImpactPool { get; }

        //Steamcopter
        IPvPPool<IPvPExplosion, Vector3> HighCalibreBulletImpactPool { get; }

        // CIWS
        IPvPPool<IPvPExplosion, Vector3> TinyBulletImpactPool { get; }

        // Radius 0.75  => Small shells: Mortar, frigate & destroyer front cannon.
        IPvPPool<IPvPExplosion, Vector3> SmallExplosionsPool { get; }

        // SAM site (variaton of 0.75)
        IPvPPool<IPvPExplosion, Vector3> FlakExplosionsPool { get; }

        // Bomber (variation of 0.75m)
        IPvPPool<IPvPExplosion, Vector3> BombExplosionPool { get; }

        // Radius 1m    => Missiles: Destroyer missiles, archon front launcher missiles
        IPvPPool<IPvPExplosion, Vector3> MediumExplosionsPool { get; }

        // Radius 1.5m  => Artillery, rocket launcher, broadsides, archon back missile launcher, archon primary cannon
        IPvPPool<IPvPExplosion, Vector3> LargeExplosionsPool { get; }

        // Nova Shell Radius 2m  => Nova Artillery
        IPvPPool<IPvPExplosion, Vector3> NovaShellImpactPool { get; }

        // RocketShell Radius 1m  => Broadsword RocketShells
        IPvPPool<IPvPExplosion, Vector3> RocketShellImpactPool { get; }

        // Radius 5m    => Nuke
        IPvPPool<IPvPExplosion, Vector3> HugeExplosionsPool { get; }


    }
}