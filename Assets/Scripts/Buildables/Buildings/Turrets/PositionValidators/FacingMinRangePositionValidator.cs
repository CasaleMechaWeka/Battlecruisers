using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.PositionValidators
{
    public class FacingMinRangePositionValidator : ITargetPositionValidator
    {
        private readonly float _minRangeInM;

        public FacingMinRangePositionValidator(float minRangeInM)
        {
            Assert.IsTrue(minRangeInM > 0);
            _minRangeInM = minRangeInM;
        }

        public bool IsValid(Vector2 targetPosition, Vector2 sourcePosition, bool isSourceMirrored)
        {
            // FELIX  Undo?
            // FELIX  Really interested in x distance...
            // FELIX  Update tests?
            float distanceInM = Mathf.Abs(targetPosition.x - sourcePosition.x);
            //float distanceInM = Vector2.Distance(targetPosition, sourcePosition);
            return
                distanceInM >= _minRangeInM
                && Helper.IsFacingTarget(targetPosition, sourcePosition, isSourceMirrored);
        }
    }
}
