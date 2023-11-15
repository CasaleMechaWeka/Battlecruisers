using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music
{
    /// <summary>
    /// There can be multiple overlapping danger events.  Ie:
    /// 1. Danger event 1 (offensive created) starts
    /// 2. Danger event 2 (cruiser health < 1/3 health) starts
    /// 3. Danger event 2 ends
    /// 4. Danger event 1 ends
    /// </summary>
    public interface IPvPDangerMonitor
    {
        event EventHandler DangerStart;
        event EventHandler DangerEnd;
    }
}