using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading
{
    public interface IPvPConstantDeferrer
    {
        void Defer(Action action);
    }
}