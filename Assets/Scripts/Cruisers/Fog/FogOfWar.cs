using System;
using UnityEngine;

namespace BattleCruisers.Cruisers.Fog
{
    public class FogOfWar : MonoBehaviour, IFogOfWar
    {
        // We never want to show fog over the player's cruiser, even if they have
        // a working stealth generator.  Simply want the enemy cruiser to be blinded
        // (ie, global target finder no longer can pinpoint targets).
        private bool _shouldShowFog;

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

        public event EventHandler IsFogEnabledChanged;

        public void Initialise(bool shouldShowFog)
        {
            _shouldShowFog = shouldShowFog;
            IsFogEnabled = false;
        }

        public void UpdateIsEnabled(int numOfFriendlyStealthGenerators, int numOfEnemySpySatellites)
        {
            IsFogEnabled = numOfFriendlyStealthGenerators != 0 && numOfEnemySpySatellites == 0;
		}
    }
}
