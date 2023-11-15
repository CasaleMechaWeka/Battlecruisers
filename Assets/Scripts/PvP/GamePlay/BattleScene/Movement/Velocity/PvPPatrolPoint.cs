using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity
{
    public class PvPPatrolPoint : IPvPPatrolPoint
    {
        public Vector2 Position { get; }
        public bool RemoveOnceReached { get; }
        public Action ActionOnReached { get; }

        public PvPPatrolPoint(Vector2 position, bool removeOnceReached = false, Action actionOnReached = null)
        {
            Position = position;
            RemoveOnceReached = removeOnceReached;
            ActionOnReached = actionOnReached;

            if (ActionOnReached == null)
            {
                ActionOnReached = () => { };
            }
        }

        public override bool Equals(object obj)
        {
            PvPPatrolPoint other = obj as PvPPatrolPoint;
            return other != null
                && Position == other.Position;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}