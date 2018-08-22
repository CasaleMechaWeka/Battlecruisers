using UnityEngine;

namespace BattleCruisers
{
    // FELIX  Move to platform abstractions namespace :)
    public interface IGameObject
    {
        bool IsVisible { get; set; }
        Vector2 Position { get; }
    }
}
