using System;

namespace BattleCruisers.UI.BattleScene.InGameHints
{
    public interface IBuildingMonitor
    {
        event EventHandler AirFactoryStarted;
        event EventHandler NavalFactoryStarted;
        event EventHandler OffensiveStarted;
    }
}