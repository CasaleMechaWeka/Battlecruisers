using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters
{
    public class PvPBroadcastingFilter : IPvPBroadcastingFilter, IPvPPermitter
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

        public PvPBroadcastingFilter(bool isMatch)
        {
            _isMatch = isMatch;
        }
    }
}
