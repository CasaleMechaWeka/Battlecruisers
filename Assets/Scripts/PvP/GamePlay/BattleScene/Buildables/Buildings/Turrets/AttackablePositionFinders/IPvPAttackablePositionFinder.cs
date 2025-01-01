using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders
{
    public interface IPvPAttackablePositionFinder
    {
        Vector2 FindClosestAttackablePosition(Vector2 sourcePosition, ITarget target);
    }
}