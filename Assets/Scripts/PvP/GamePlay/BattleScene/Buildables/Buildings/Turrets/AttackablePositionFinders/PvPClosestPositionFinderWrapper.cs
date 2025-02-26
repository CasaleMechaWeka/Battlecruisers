using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public class PvPClosestPositionFinderWrapper : MonoBehaviour, IPvPAttackablePositionFinderWrapper
    {
        public IAttackablePositionFinder CreatePositionFinder()
        {
            return new PvPClosestPositionFinder();
        }
    }
}