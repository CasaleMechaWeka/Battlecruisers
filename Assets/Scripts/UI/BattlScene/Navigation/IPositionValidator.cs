using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface IPositionValidator
    {
        bool IsValid(Vector2 position);
    }
}