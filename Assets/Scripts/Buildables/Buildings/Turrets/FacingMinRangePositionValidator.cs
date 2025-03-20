using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.PositionValidators
{
    public class FacingMinRangePositionValidator
    {
        private readonly float _minRangeInM;

        public FacingMinRangePositionValidator(float minRangeInM)
        {
            Assert.IsTrue(minRangeInM > 0 || minRangeInM == float.NaN);
            _minRangeInM = minRangeInM;
        }

        public bool IsValid(Vector2 targetPosition, Vector2 sourcePosition, bool isSourceMirrored)
        {
            if (_minRangeInM == float.NaN)
                return true;

            float distanceInM = Vector2.Distance(targetPosition, sourcePosition);
            return
                distanceInM >= _minRangeInM
                && Helper.IsFacingTarget(targetPosition, sourcePosition, isSourceMirrored);
        }
    }
}
