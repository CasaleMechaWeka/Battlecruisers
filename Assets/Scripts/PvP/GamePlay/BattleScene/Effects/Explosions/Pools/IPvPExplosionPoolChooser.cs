using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools
{
    public interface IPvPExplosionPoolChooser
    {
        IPvPPool<IPvPExplosion, Vector3> ChoosePool(IPvPExplosionPoolProvider explosionPoolProvider);
    }
}