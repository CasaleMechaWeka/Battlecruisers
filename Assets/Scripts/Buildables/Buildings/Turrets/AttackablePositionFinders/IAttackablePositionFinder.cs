using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public interface IAttackablePositionFinder
    {
        Vector2 FindClosestAttackablePosition(Vector2 sourcePosition, ITarget target);
    }
}