using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions.Pools
{
    public interface IExplosionPoolProvider
    {
        // Ship turret, air turret, boat direct fire guns
        IPool<IExplosion, Vector3> BulletImpactPool { get; }

        // Radius 0.75m => Mortar, SAM site
        IPool<IExplosion, Vector3> SmallExplosionsPool { get; }
        
        // Bomber (variation of 0.75m)
        IPool<IExplosion, Vector3> BombExplosionPool { get; }

        // Radius 1m    => Destroyer missiles, archon front launcher missiles
        IPool<IExplosion, Vector3> MediumExplosionsPool { get; }
        
        // Radius 1.5m  => Artillery, rocket launcher, broadsides, archon back missile launcher, archon primary cannon
        IPool<IExplosion, Vector3> LargeExplosionsPool { get; }
        
        // Radius 5m    => Nuke
        IPool<IExplosion, Vector3> HugeExplosionsPool { get; }
    }
}