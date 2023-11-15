using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators
{
    public class PvPFacingMinRangePositionValidator : IPvPTargetPositionValidator
    {
        private readonly float _minRangeInM;

        public PvPFacingMinRangePositionValidator(float minRangeInM)
        {
            Assert.IsTrue(minRangeInM > 0);
            _minRangeInM = minRangeInM;
        }

        public bool IsValid(Vector2 targetPosition, Vector2 sourcePosition, bool isSourceMirrored)
        {
            float distanceInM = Vector2.Distance(targetPosition, sourcePosition);
            return
                distanceInM >= _minRangeInM
                && PvPHelper.IsFacingTarget(targetPosition, sourcePosition, isSourceMirrored);
        }
    }
}
