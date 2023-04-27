using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators
{
    public interface IPvPTargetPositionValidator
    {
        bool IsValid(Vector2 targetPosition, Vector2 sourcePosition, bool isSourceMirrored);
    }
}
