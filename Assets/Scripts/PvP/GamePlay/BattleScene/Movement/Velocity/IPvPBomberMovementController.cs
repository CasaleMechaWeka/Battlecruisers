using BattleCruisers.Movement.Velocity;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity
{
    public interface IPvPBomberMovementController : IMovementController
    {
        Vector2 TargetVelocity { get; set; }
    }
}
