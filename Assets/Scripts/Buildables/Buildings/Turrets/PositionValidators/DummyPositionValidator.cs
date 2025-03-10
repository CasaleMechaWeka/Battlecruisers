using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.PositionValidators
{
    /// <summary>
    /// Null object
    /// </summary>
    public class DummyPositionValidator : ITargetPositionValidator
    {
        public bool IsValid(Vector2 targetPosition, Vector2 sourcePosition, bool isSourceMirrored)
        {
            return true;
        }
    }
}
