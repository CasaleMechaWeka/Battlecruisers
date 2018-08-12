using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public class ClosestPositionFinderWrapper : MonoBehaviour, IAttackablePositionFinderWrapper
    {
        public IAttackablePositionFinder CreatePositionFinder()
        {
            return new ClosestPositionFinder();
        }
    }
}