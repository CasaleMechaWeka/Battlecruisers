using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public class StarController : MonoBehaviour
    {
        private Image _enabledStar, _disabledStar;

        public bool Enabled
        {
            set
            {
                _enabledStar.enabled = value;
                _disabledStar.enabled = !value;
            }
        }

        public Color Color
        {
            set
            {
                _enabledStar.color = value;
                _disabledStar.color = value;
            }
        }

        public void Initialise()
        {
            _enabledStar = transform.FindNamedComponent<Image>("EnabledStar");
            _disabledStar = transform.FindNamedComponent<Image>("DisabledStar");
        }
    }
}