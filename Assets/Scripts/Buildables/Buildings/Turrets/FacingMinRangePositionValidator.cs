using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.PositionValidators
{
    public class FacingMinRangePositionValidator
    {
        private readonly float _minRangeInM;
        private readonly bool _isDummy;

        public FacingMinRangePositionValidator(float minRangeInM, bool isDummy = false)
        {
            Assert.IsTrue(minRangeInM > 0 || isDummy);
            _minRangeInM = minRangeInM;
            _isDummy = isDummy;
        }

        public bool IsValid(Vector2 targetPosition, Vector2 sourcePosition, bool isSourceMirrored)
        {
            if (_isDummy)
                return true;

            float distanceInM = Vector2.Distance(targetPosition, sourcePosition);
            return
                distanceInM >= _minRangeInM
                && Helper.IsFacingTarget(targetPosition, sourcePosition, isSourceMirrored);
        }
    }
}
