using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public class PvPMissilesSmartPoolChooser : MonoBehaviour, IPvPProjectilePoolChooser<PvPSmartMissileController, PvPSmartMissileActivationArgs<IPvPSmartProjectileStats>, IPvPSmartProjectileStats>
    {
        public IPvPPool<PvPSmartMissileController, PvPSmartMissileActivationArgs<IPvPSmartProjectileStats>> ChoosePool(IPvPProjectilePoolProvider projectilePoolProvider)
        {
            return projectilePoolProvider.MissilesSmartPool;
        }
    }
}