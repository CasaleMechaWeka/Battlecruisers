using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors
{
    public abstract class PvPBaseThreatMonitor : IPvPThreatMonitor
    {
        private PvPThreatLevel _currentThreatLevel;
        public PvPThreatLevel CurrentThreatLevel
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
            CurrentThreatLevel = PvPThreatLevel.None;
        }
    }
}
