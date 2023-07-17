using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    public interface IPvPGameEndMonitor
    {
        event EventHandler GameEnded;
        void RegisterAIOfLeftPlayer(IPvPArtificialIntelligence ai_LeftPlayer);
        void RegisterAIOfRightPlayer(IPvPArtificialIntelligence ai_RightPlayer);
    }
}