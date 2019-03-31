using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Filters
{
    // FELIX  Test :)
    public class CompositeBroadcastingFilter : IBroadcastingFilter, IPermitter
    {
        private readonly BroadcastingFilter[] _filters;

        private bool _isMatch;
        public bool IsMatch
        {
            get { return _isMatch; }
            set
            {
                if (_isMatch != value)
                {
                    _isMatch = value;

                    foreach (BroadcastingFilter filter in _filters)
                    {
                        filter.IsMatch = value;
                    }

                    PotentialMatchChange?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler PotentialMatchChange;

        public CompositeBroadcastingFilter(bool isMatch, params BroadcastingFilter[] filters)
        {
            Assert.IsNotNull(filters);
            Assert.IsTrue(filters.Length > 1);

            IsMatch = isMatch;
        }
    }
}