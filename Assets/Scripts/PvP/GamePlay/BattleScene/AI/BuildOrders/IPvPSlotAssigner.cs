using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Requests;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders
{
    public interface IPvPSlotAssigner
    {
        void AssignSlots(IEnumerable<IPvPOffensiveRequest> slotRequests, int numOfSlotsAvailable);
    }
}
