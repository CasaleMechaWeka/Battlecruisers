using System;
using UnityEngine;

namespace BattleCruisers.Cruisers.Fog
{
    public class FogOfWar : MonoBehaviour, IFogOfWar
    {
        // FELIX  UPdate :)
        // We never want to show fog over the player's cruiser, even if they have
        // a working stealth generator.  Simply want the enemy cruiser to be blinded
        // (ie, global target finder no longer can pinpoint targets).
        private bool _shouldShowFog;

        private const float STRONG_FOG_ALPHA = 1;
        private const float WEAK_FOG_ALPHA = 0.08f;

		private bool _isFogEnabled;
		public bool IsFogEnabled
		{
			get { return _isFogEnabled; }
			private set
			{
                if (_isFogEnabled != value)
                {
                    _isFogEnabled = value;

                    if (_shouldShowFog)
                    {
                        gameObject.SetActive(_isFogEnabled);
					}

					if (IsFogEnabledChanged != null)
					{
						IsFogEnabledChanged.Invoke(this, EventArgs.Empty);
					}
				}
			}
		}

        // FELIX  Shouldn't need this event??
        public event EventHandler IsFogEnabledChanged;

        public void Initialise(FogStrength fogStrength)
        {
            IsFogEnabled = false;

            float fogAlpha = fogStrength == FogStrength.Weak ? WEAK_FOG_ALPHA : STRONG_FOG_ALPHA;
            // Black
            Color fogColor = new Color(r: 0, g: 0, b: 0, a: fogAlpha);

            SpriteRenderer[] clouds = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer cloud in clouds)
            {
                cloud.color = fogColor;
            }
        }

        // FELIX  Extract logic to other class, makes testable
        public void UpdateIsEnabled(int numOfFriendlyStealthGenerators, int numOfEnemySpySatellites)
        {
            IsFogEnabled = numOfFriendlyStealthGenerators != 0 && numOfEnemySpySatellites == 0;
		}
    }
}
