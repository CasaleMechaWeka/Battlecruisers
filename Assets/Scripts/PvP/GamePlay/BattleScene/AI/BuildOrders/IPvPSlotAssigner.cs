using System.Collections.Generic;
using BattleCruisers.Data.Static.Strategies.Requests;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders
{
    public interface IPvPSlotAssigner
    {
        void AssignSlots(IEnumerable<IOffensiveRequest> slotRequests, int numOfSlotsAvailable);
    }
}
