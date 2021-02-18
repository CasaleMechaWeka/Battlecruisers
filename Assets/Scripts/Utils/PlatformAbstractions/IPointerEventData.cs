using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public interface IPointerEventData
    {
        Vector2 Delta { get; }
        Vector2 Position { get; }
    }
}