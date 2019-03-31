using System;

namespace BattleCruisers.UI.Filters
{
    public class BroadcastingFilter : IBroadcastingFilter, IPermitter
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
                if (_isMatch != value)
                {
                    _isMatch = value;
                    PotentialMatchChange?.Invoke(this, EventArgs.Empty);
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
