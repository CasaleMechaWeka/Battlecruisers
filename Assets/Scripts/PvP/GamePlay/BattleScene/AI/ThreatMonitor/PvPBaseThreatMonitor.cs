using System;
using BattleCruisers.AI.ThreatMonitors;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors
{
    public abstract class PvPBaseThreatMonitor : IThreatMonitor
    {
        private ThreatLevel _currentThreatLevel;
        public ThreatLevel CurrentThreatLevel
        {
            get { return _currentThreatLevel; }
            protected set
            {
                if (_currentThreatLevel != value)
                {
                    _currentThreatLevel = value;

                    ThreatLevelChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler ThreatLevelChanged;

        public PvPBaseThreatMonitor()
        {
            CurrentThreatLevel = ThreatLevel.None;
        }
    }
}
