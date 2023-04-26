using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity
{
    public interface IPvPBomberMovementController : IPvPMovementController
    {
        Vector2 TargetVelocity { get; set; }
    }
}
