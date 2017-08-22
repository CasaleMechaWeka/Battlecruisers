using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Data.Static.Strategies.Requests;

namespace BattleCruisers.AI.Providers.BuildingKey
{
    public class SlotAssigner : ISlotAssigner
	{
		/// <summary>
		/// 1. High focus requests are served once before low focus requests
		/// 2. Low focus request get one slot at most
		/// 3. Remaining slots are split evenly between high focus requests
		/// </summary>
		public void AssignSlots(IEnumerable<IOffensiveRequest> slotRequests, int numOfSlotsAvailable)
		{
			IEnumerable<IOffensiveRequest> highFocusRequests = slotRequests.Where(request => request.Focus == OffensiveFocus.High);
			IEnumerable<IOffensiveRequest> lowFocusRequest = slotRequests.Where(request => request.Focus == OffensiveFocus.Low);

			int numOfSlotsUsed = 0;

			// Assign a round of high focus requests
			numOfSlotsUsed = AssignSlotsToRequests(highFocusRequests, numOfSlotsAvailable, numOfSlotsUsed);

			// Assign a round of low focus request
			numOfSlotsUsed = AssignSlotsToRequests(lowFocusRequest, numOfSlotsAvailable, numOfSlotsUsed);

			// Assign remaining slots to high focus requests
			if (highFocusRequests.Any())
			{
				while (numOfSlotsUsed < numOfSlotsAvailable)
				{
					numOfSlotsUsed = AssignSlotsToRequests(highFocusRequests, numOfSlotsAvailable, numOfSlotsUsed);
				}
			}
		}

		private int AssignSlotsToRequests(IEnumerable<IOffensiveRequest> requests, int numOfPlatformSlots, int numOfSlotsUsed)
		{
			foreach (IOffensiveRequest request in requests)
			{
				if (numOfSlotsUsed < numOfPlatformSlots)
				{
					request.NumOfSlotsToUse++;
					numOfSlotsUsed++;
				}
				else
				{
					break;
				}
			}

			return numOfSlotsUsed;
		}
	}
}
