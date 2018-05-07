using System;

namespace BattleCruisers.UI
{
    public class BasicDecider : IFilter
    {
        private bool _shouldBeEnabled;
        public bool IsMatch
        {
            get
            {
                return _shouldBeEnabled;
            }
            set
            {
                _shouldBeEnabled = value;

                if (PotentialMatchChange != null)
                {
                    PotentialMatchChange.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler PotentialMatchChange;

        public BasicDecider(bool shouldBeEnabled)
        {
            IsMatch = shouldBeEnabled;
        }
    }
}
