using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Fog
{
    public class FogOfWar : IFogOfWar
    {
        private readonly GameObject _fogGameObject;

		private bool _isFogEnabled;
		public bool IsFogEnabled
		{
			get { return _isFogEnabled; }
			private set
			{
				if (_isFogEnabled != value)
				{
					_isFogEnabled = value;
                    _fogGameObject.SetActive(_isFogEnabled);

					if (IsFogEnabledChanged != null)
					{
						IsFogEnabledChanged.Invoke(this, EventArgs.Empty);
					}
				}
			}
		}

        public event EventHandler IsFogEnabledChanged;

        public FogOfWar(GameObject fogGameObject)
        {
            Assert.IsNotNull(fogGameObject);

            _fogGameObject = fogGameObject;
            IsFogEnabled = false;
        }

        public void UpdateIsEnabled(int numOfFriendlyStealthGenerators, int numOfEnemySpySatellites)
        {
            IsFogEnabled = numOfFriendlyStealthGenerators != 0 && numOfEnemySpySatellites == 0;
		}
    }
}
