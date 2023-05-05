using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPBombPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPBombController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>
    {
        public IPvPPool<PvPBombController, PvPProjectileActivationArgs<IPvPProjectileStats>>
            ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.BombsPool;
        }
    }
}