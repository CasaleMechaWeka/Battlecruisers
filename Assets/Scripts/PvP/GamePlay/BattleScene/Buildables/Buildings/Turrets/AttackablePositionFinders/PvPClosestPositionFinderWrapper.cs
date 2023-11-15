using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public class PvPClosestPositionFinderWrapper : MonoBehaviour, IPvPAttackablePositionFinderWrapper
    {
        public IPvPAttackablePositionFinder CreatePositionFinder()
        {
            return new PvPClosestPositionFinder();
        }
    }
}