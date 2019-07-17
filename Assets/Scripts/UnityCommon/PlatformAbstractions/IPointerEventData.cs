using UnityEngine;

namespace UnityCommon.PlatformAbstractions
{
    public interface IPointerEventData
    {
        Vector2 Delta { get; }
        Vector2 Position { get; }
    }
}