using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public class DummyPositionFinderWrapper : MonoBehaviour, IAttackablePositionFinderWrapper
    {
        public IAttackablePositionFinder CreatePositionFinder()
        {
            return new DummyPositionFinder();
        }
    }
}