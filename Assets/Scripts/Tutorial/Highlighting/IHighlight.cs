using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting
{
    public interface IHighlight
    {
        // FELIX  Remove initialise :(
        void Initialise(float radius, Vector2 position);
        void Destroy();
    }
}
