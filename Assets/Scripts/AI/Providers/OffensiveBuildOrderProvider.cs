using System.Collections.Generic;
using System.Linq;
using BattleCruisers.AI.Providers.Strategies;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers
{
    public class OffensiveBuildOrderProvider : IOffensiveBuildOrderProvider
    {
        /// <summary>
        /// NOTE:  Must use IList as a parateter instead if IEnumerable.  Initially I used
        /// IEnumerable from a LINQ Select query, but every time I looped through this
        /// IEnumerable I would get a fresh copy of the object, so any changes I made to
        /// those objects were lost!!!
        /// </summary>
        public IList<IPrefabKey> CreateBuildOrder(int numOfPlatformSlots, IList<IOffensiveRequest> requests)
        {
            IList<IPrefabKey> buildOrder = new List<IPrefabKey>();

            // Only the naval request requires the bow slot, so can always assign 
            // this slot to the naval request.
            IOffensiveRequest navalRequest = requests.FirstOrDefault(request => request.Type == OffensiveType.Naval);
            if (navalRequest != null)
            {
                buildOrder.Add(navalRequest.BuildingKeyProvider.Next);
            }

			// All non-naval requests require platform slots, so need to split the available
			// platform slots between these requests.
			// FELIX  Handle ArchonBattleship Ultra, which may be the exception :P
			IEnumerable<IOffensiveRequest> platformRequests = requests.Where(request => request.Type != OffensiveType.Naval);
			AssignPlatformSlots(platformRequests, numOfPlatformSlots);

			foreach (IOffensiveRequest request in platformRequests)
            {
                for (int i = 0; i < request.NumOfSlotsToUse; ++i)
				{
                    buildOrder.Add(request.BuildingKeyProvider.Next);
				}
            }

            return buildOrder;
        }

        /// <summary>
        /// 1. High focus requests are served once before low focus requests
        /// 2. Low focus request get one slot at most
        /// 3. Remaining slots are split evenly between high focus requests
        /// </summary>
        private void AssignPlatformSlots(IEnumerable<IOffensiveRequest> platformRequests, int numOfPlatformSlots)
        {
            IEnumerable<IOffensiveRequest> highFocusRequests = platformRequests.Where(request => request.Focus == OffensiveFocus.High);
            IEnumerable<IOffensiveRequest> lowFocusRequest = platformRequests.Where(request => request.Focus == OffensiveFocus.Low);

            int numOfSlotsUsed = 0;

            // Assign a round of high focus requests
            numOfSlotsUsed = AssignSlotsToRequests(highFocusRequests, numOfPlatformSlots, numOfSlotsUsed);

			// Assign a round of low focus request
			numOfSlotsUsed = AssignSlotsToRequests(lowFocusRequest, numOfPlatformSlots, numOfSlotsUsed);

			// Assign remaining slots to high focus requests
			if (highFocusRequests.Any())
            {
	            while (numOfSlotsUsed < numOfPlatformSlots)
	            {
	                numOfSlotsUsed = AssignSlotsToRequests(highFocusRequests, numOfPlatformSlots, numOfSlotsUsed);
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
