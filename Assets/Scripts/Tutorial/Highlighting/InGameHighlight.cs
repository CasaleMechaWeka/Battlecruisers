using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting
{
    public class InGameHighlight : ScalableCircle, IHighlight
    {
        public void Initialise(float radiusInM, Vector2 position)
        {
            base.Initialise(radiusInM);
            transform.position = position;
        }

		public void Destroy()
		{
            Destroy(gameObject);
		}
    }
}
