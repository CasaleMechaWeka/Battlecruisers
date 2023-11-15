using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading
{
    public interface IPvPDeferrer
    {
        void Defer(Action action, float delayInS);
    }
}
