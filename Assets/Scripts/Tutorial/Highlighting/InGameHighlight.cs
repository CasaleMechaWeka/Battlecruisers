using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting
{
    public class InGameHighlight : ScalableCircle, IHighlight
    {
        public void Initialise(float radius, Vector2 position)
        {
            base.Initialise(radius);
            transform.position = position;
        }

		void IHighlight.Destroy()
		{
            Destroy(gameObject);
		}
    }
}
