using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public interface IExplosionPoolProvider
    {
        IPool<IExplosion, Vector3> BulletImpactPool { get; }

        // Radius 0.75m => Bomber, mortar, SAM site
        IPool<IExplosion, Vector3> SmallExplosionsPool { get; }
        
        // Radius 1m    => Destroyer missiles, archon front launcher missiles
        IPool<IExplosion, Vector3> MediumExplosionsPool { get; }
        
        // Radius 1.5m  => Artillery, rocket launcher, broadsides, archon back missile launcher, archon primary cannon
        IPool<IExplosion, Vector3> LargeExplosionsPool { get; }
        
        // Radius 5m    => Nuke
        IPool<IExplosion, Vector3> HugeExplosionsPool { get; }
    }
}