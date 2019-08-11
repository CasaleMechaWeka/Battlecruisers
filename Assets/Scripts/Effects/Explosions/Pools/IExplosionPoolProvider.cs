using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public interface IExplosionPoolProvider
    {
        // Radius 0.75m => Bomber, mortar, SAM site
        IPool<Vector3> SmallExplosionsPool { get; }
        
        // Radius 1m    => Destroyer missiles, archon front launcher missiles
        IPool<Vector3> MediumExplosionsPool { get; }
        
        // Radius 1.5m  => Artillery, rocket launcher, broadsides, archon back missile launcher, archon primary cannon
        IPool<Vector3> LargeExplosionsPool { get; }
        
        // Radius 5m    => Nuke
        IPool<Vector3> HugeExplosionsPool { get; }
    }
}