using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.PositionValidators
{
    public interface ITargetPositionValidator
    {
        bool IsValid(Vector2 targetPosition, Vector2 sourcePosition, bool isSourceMirrored);
    }
}
