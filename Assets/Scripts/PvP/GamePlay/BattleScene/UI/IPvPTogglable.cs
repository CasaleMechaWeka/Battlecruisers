using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI
{
    public interface IPvPTogglable
    {
        event EventHandler EnabledChange;

        bool Enabled { get; set; }
    }
}