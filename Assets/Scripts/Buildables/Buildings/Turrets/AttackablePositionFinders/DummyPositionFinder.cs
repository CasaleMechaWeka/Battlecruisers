using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public class DummyPositionFinder : IAttackablePositionFinder
    {
        public Vector2 FindClosestAttackablePosition(Vector2 sourcePosition, ITarget target)
        {
            return target.Position;
        }
    }
}