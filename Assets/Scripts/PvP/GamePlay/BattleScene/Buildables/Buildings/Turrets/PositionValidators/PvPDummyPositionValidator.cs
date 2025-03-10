using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators
{
    /// <summary>
    /// Null object
    /// </summary>
    public class PvPDummyPositionValidator : ITargetPositionValidator
    {
        public bool IsValid(Vector2 targetPosition, Vector2 sourcePosition, bool isSourceMirrored)
        {
            return true;
        }
    }
}
