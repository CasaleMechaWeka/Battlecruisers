using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Requests;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders
{
    public class PvPSlotAssigner : IPvPSlotAssigner
    {
        /// <summary>
        /// 1. High focus requests are served once before low focus requests
        /// 2. Low focus request get one slot at most
        /// 3. Remaining slots are split evenly between high focus requests
        /// </summary>
        public void AssignSlots(IEnumerable<IPvPOffensiveRequest> slotRequests, int numOfSlotsAvailable)
        {
            IEnumerable<IPvPOffensiveRequest> highFocusRequests = slotRequests.Where(request => request.Focus == PvPOffensiveFocus.High);
            IEnumerable<IPvPOffensiveRequest> lowFocusRequest = slotRequests.Where(request => request.Focus == PvPOffensiveFocus.Low);

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

        private int AssignSlotsToRequests(IEnumerable<IPvPOffensiveRequest> requests, int numOfPlatformSlots, int numOfSlotsUsed)
        {
            foreach (IPvPOffensiveRequest request in requests)
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
