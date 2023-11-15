using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters
{
    public class PvPCompositeBroadcastingFilter : IPvPBroadcastingFilter, IPvPPermitter
    {
        private readonly PvPBroadcastingFilter[] _filters;

        private bool _isMatch;
        public bool IsMatch
        {
            get { return _isMatch; }
            set
            {
                if (_isMatch != value)
                {
                    _isMatch = value;

                    foreach (PvPBroadcastingFilter filter in _filters)
                    {
                        filter.IsMatch = value;
                    }

                    PotentialMatchChange?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler PotentialMatchChange;

        public PvPCompositeBroadcastingFilter(bool isMatch, params PvPBroadcastingFilter[] filters)
        {
            Assert.IsNotNull(filters);
            Assert.IsTrue(filters.Length > 1);

            _filters = filters;
            IsMatch = isMatch;
        }
    }
}