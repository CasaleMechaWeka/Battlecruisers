using UnityEngine;

namespace BattleCruisers.Tutorial
{
    public interface IHighlight
    {
        void Show(Vector2 size);
        void Destroy();
    }
}
