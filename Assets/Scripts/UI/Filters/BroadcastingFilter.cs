using System;

namespace BattleCruisers.UI.Filters
{
    public class BroadcastingFilter : IBroadcastingFilter
    {
        private bool _isMatch;
        public bool IsMatch
        {
            get
            {
                return _isMatch;
            }
            set
            {
                _isMatch = value;

                if (PotentialMatchChange != null)
                {
                    PotentialMatchChange.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler PotentialMatchChange;

        public BroadcastingFilter(bool isMatch)
        {
            _isMatch = isMatch;
        }
    }
}
