using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    public interface IPvPGameEndMonitor
    {
        event EventHandler GameEnded;
        void RegisterAIOfLeftPlayer(IManagedDisposable ai_LeftPlayer);
        void RegisterAIOfRightPlayer(IManagedDisposable ai_RightPlayer);
    }
}