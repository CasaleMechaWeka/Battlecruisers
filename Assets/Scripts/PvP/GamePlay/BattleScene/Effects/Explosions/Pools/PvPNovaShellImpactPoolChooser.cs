using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools
{
    public class PvPNovaShellImpactPoolChooser : MonoBehaviour, IPvPExplosionPoolChooser
    {
        public IPvPPool<IPvPExplosion, Vector3> ChoosePool(IPvPExplosionPoolProvider explosionPoolProvider)
        {
            return explosionPoolProvider.NovaShellImpactPool;
        }
    }
}