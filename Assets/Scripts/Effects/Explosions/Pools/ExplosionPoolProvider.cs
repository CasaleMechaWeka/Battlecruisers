using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions.Pools
{
    // FELIX  Implement
    public class ExplosionPoolProvider : IExplosionPoolProvider
    {
        public IPool<Vector3> SmallExplosionsPool => throw new System.NotImplementedException();

        public IPool<Vector3> MediumExplosionsPool => throw new System.NotImplementedException();

        public IPool<Vector3> LargeExplosionsPool => throw new System.NotImplementedException();

        public IPool<Vector3> HugeExplosionsPool => throw new System.NotImplementedException();
    }
}