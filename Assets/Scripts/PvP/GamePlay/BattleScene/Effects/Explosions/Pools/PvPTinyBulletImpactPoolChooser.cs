using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools
{
    public class PvPTinyBulletImpactPoolChooser : MonoBehaviour, IPvPExplosionPoolChooser
    {
        public IPvPPool<IPvPExplosion, Vector3> ChoosePool(IPvPExplosionPoolProvider explosionPoolProvider)
        {
            return explosionPoolProvider.TinyBulletImpactPool;
        }
    }
}