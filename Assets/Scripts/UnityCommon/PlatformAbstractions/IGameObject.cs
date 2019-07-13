using UnityEngine;

namespace UnityCommon.PlatformAbstractions
{
    public interface IGameObject
    {
        GameObject PlatformObject { get; }
        ITransform Transform { get; }

        void Destroy();
    }
}