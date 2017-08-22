using System.Collections.Generic;
using BattleCruisers.Data.Static.Strategies.Requests;

namespace BattleCruisers.AI.Providers.BuildingKey
{
    public interface ISlotAssigner
	{
		void AssignSlots(IEnumerable<IOffensiveRequest> slotRequests, int numOfSlotsAvailable);
	}
}
