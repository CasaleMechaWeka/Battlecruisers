using System;

namespace BattleCruisers.UI
{
    public class BasicDecider : IActivenessDecider
    {
        private bool _shouldBeEnabled;
        public bool ShouldBeEnabled
        {
            get
            {
                return _shouldBeEnabled;
            }
            set
            {
                _shouldBeEnabled = value;

                if (PotentialActivenessChange != null)
                {
                    PotentialActivenessChange.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler PotentialActivenessChange;

        public BasicDecider(bool shouldBeEnabled)
        {
            ShouldBeEnabled = shouldBeEnabled;
        }
    }
}
