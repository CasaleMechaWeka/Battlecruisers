using System;
using UnityCommon.PlatformAbstractions.Time;

namespace BattleCruisers.Utils.BattleScene.Update
{
    public interface IUpdater : IDeltaTimeProvider
    {
        event EventHandler Updated;
    }
}