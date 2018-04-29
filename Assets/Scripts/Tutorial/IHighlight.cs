using UnityEngine;

namespace BattleCruisers.Tutorial
{
    public interface IHighlight
    {
        void Initialise(float radius, Vector2 position);
        void Destroy();
    }
}
