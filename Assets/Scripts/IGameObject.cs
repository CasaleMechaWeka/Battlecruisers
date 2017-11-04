using UnityEngine;

namespace BattleCruisers
{
    public interface IGameObject
    {
        bool IsVisible { get; set; }
        Vector2 Position { get; }
    }
}
