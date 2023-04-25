using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity
{
    public interface IPvPPatrolPoint
    {
        Vector2 Position { get; }
        bool RemoveOnceReached { get; }
        Action ActionOnReached { get; }
    }
}