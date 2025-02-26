using BattleCruisers.Effects.Explosions.Pools;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools
{
    public class PvPFiveShellClusterExplosionPoolChooser : MonoBehaviour, IPvPExplosionPoolChooser
    {
        public IPool<IPoolable<Vector3>, Vector3> ChoosePool(IExplosionPoolProvider explosionPoolProvider)
        {
            return explosionPoolProvider.FiveShellClusterExplosionsPool;
        }
    }
}