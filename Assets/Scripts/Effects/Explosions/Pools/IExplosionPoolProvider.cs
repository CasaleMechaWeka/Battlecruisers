using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions.Pools
{
    public interface IExplosionPoolProvider
    {
        // Ship turret, air turret, boat direct fire guns
        IPool<IExplosion, Vector3> BulletImpactPool { get; }

        //Steamcopter
        IPool<IExplosion, Vector3> HighCalibreBulletImpactPool { get; }

        // CIWS
        IPool<IExplosion, Vector3> TinyBulletImpactPool { get; }

        // Radius 0.75  => Small shells: Mortar, frigate & destroyer front cannon.
        IPool<IExplosion, Vector3> SmallExplosionsPool { get; }

        // SAM site (variaton of 0.75)
        IPool<IExplosion, Vector3> FlakExplosionsPool { get; }

        // Bomber (variation of 0.75m)
        IPool<IExplosion, Vector3> BombExplosionPool { get; }

        // Radius 1m    => Missiles: Destroyer missiles, archon front launcher missiles
        IPool<IExplosion, Vector3> MediumExplosionsPool { get; }
        
        // Radius 1.5m  => Artillery, rocket launcher, broadsides, archon back missile launcher, archon primary cannon
        IPool<IExplosion, Vector3> LargeExplosionsPool { get; }

        // Nova Shell Radius 2m  => Nova Artillery
        IPool<IExplosion, Vector3> NovaShellImpactPool { get; }
        
        
        // Radius 5m    => Nuke
        IPool<IExplosion, Vector3> HugeExplosionsPool { get; }

 
    }
}