using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public interface IGameObject
    {
        bool IsVisible { get; set; }
        Vector3 Position { get; set; }
    }
}
