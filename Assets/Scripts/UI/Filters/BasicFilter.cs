using System;

namespace BattleCruisers.UI.Filters
{
    public class BasicFilter : IBroadcastingFilter
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

        public BasicFilter(bool isMatch)
        {
            _isMatch = isMatch;
        }
    }
}
