using System;
using BattleCruisers.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Utils.BattleScene.Update
{
    public interface IUpdater : IDeltaTimeProvider
    {
        event EventHandler Updated;
    }
}