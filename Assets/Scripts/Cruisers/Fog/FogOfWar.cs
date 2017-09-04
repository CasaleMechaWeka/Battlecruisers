using System;

namespace BattleCruisers.Cruisers.Fog
{
    public class FogOfWar : IFogOfWar
    {
		private bool _isFogEnabled;
		public bool IsFogEnabled
		{
			get { return _isFogEnabled; }
			private set
			{
				if (_isFogEnabled != value)
				{
					_isFogEnabled = value;

					if (IsFogEnabledChanged != null)
					{
						IsFogEnabledChanged.Invoke(this, EventArgs.Empty);
					}
				}
			}
		}

        public event EventHandler IsFogEnabledChanged;

        public void UpdateIsEnabled(int numOfFriendlyStealthGenerators, int numOfEnemySpySatellites)
        {
            IsFogEnabled = numOfFriendlyStealthGenerators != 0 && numOfEnemySpySatellites == 0;
		}
    }
}
