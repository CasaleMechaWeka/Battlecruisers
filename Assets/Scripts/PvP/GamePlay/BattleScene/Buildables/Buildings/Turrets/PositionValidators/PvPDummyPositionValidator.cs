using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators
{
    /// <summary>
    /// Null object
    /// </summary>
    public class PvPDummyPositionValidator : IPvPTargetPositionValidator
    {
        public bool IsValid(Vector2 targetPosition, Vector2 sourcePosition, bool isSourceMirrored)
        {
            return true;
        }
    }
}
