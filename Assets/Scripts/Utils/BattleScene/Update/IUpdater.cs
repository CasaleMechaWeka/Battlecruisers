using System;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.Utils.BattleScene.Update
{
    public interface IUpdater : IDeltaTimeProvider
    {
        event EventHandler Updated;
    }
}