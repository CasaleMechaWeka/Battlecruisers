using System;

namespace BattleCruisers.Buildables.Boost
{
    public class Booster : IBooster
	{
        private const float DEFAULT_BOOST_MULTIPLIER = 1;

        private float _boostMultiplier;
		public float BoostMultiplier 
        { 
            get { return _boostMultiplier; }
            set
            {
                if (_boostMultiplier != value)
                {
                    _boostMultiplier = value;

                    if (BoostChanged != null)
                    {
                        BoostChanged.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

		public event EventHandler BoostChanged;

        public Booster()
        {
            _boostMultiplier = DEFAULT_BOOST_MULTIPLIER;
        }
	}
}
