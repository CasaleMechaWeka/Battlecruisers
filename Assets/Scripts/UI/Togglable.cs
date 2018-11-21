using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI
{
    public class Togglable : MonoBehaviour, ITogglable
    {
        protected virtual Image Image { get { return null; } }

        public bool Enabled
        {
            set
            {
                enabled = value;

                if (Image != null)
                {
                    Color color = Image.color;
                    color.a = value ? Constants.ENABLED_UI_ALPHA : Constants.DISABLED_UI_ALPHA;
                    Image.color = color;
                }
            }
        }
    }
}