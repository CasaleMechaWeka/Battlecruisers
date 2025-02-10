using BattleCruisers.AI;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    public interface IPvPGameEndMonitor
    {
        event EventHandler GameEnded;
        void RegisterAIOfLeftPlayer(IArtificialIntelligence ai_LeftPlayer);
        void RegisterAIOfRightPlayer(IArtificialIntelligence ai_RightPlayer);
    }
}