using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Providers
{
    public enum OffensiveType
    {
        Air, Naval, Buildings, Ultras
    }

    public enum OffensiveFocus
    {
        Low, High
    }

    // FELIX  Own file
    public class OffensiveRequest
    {
        public OffensiveType Type { get; private set; }
        public OffensiveFocus Focus { get; private set; }

        public OffensiveRequest(OffensiveType type, OffensiveFocus focus)
        {
            this.Type = type;
            this.Focus = focus;
        }
    }

    public class OffensiveBuildOrderProvider
    {
        private readonly IStaticData _staticData;

        public OffensiveBuildOrderProvider(IStaticData staticData)
        {
            Assert.IsNotNull(staticData);
            _staticData = staticData;
        }

        /// <summary>
        /// FELIX  Need:
        /// + Available buildings
        ///     + Offensive
        ///     + Ultra
        /// + Number of platform slots
        /// </summary>
        public IList<IPrefabKey> CreateBuildOrder(int levelNum, int numOfPlatformSlots, params OffensiveRequest[] requests)
        {
            IList<IPrefabKey> buildOrder = new List<IPrefabKey>();

            OffensiveRequest navalRequest = requests.FirstOrDefault(request => request.Type == OffensiveType.Naval);
            if (navalRequest != null)
            {
                buildOrder.Add(StaticPrefabKeys.Buildings.NavalFactory);
            }

            // All non-naval requests require platform slots
            // FELIX  Handle ArchonBattleship Ultra, which may be the exception :P
            IEnumerable<OffensiveRequest> platformRequests = requests.Where(request => request.Type != OffensiveType.Naval);

            IDictionary<OffensiveType, int> requestTypeToSlotNum = AssignPlatformSlots(platformRequests, numOfPlatformSlots);

            foreach (KeyValuePair<OffensiveType, int> pair in requestTypeToSlotNum)
            {
                for (int i = 0; i < pair.Value; ++i)
                {
                    // FELIX
                    //buildOrder.Add();
                }
            }

            return buildOrder;
        }

        private IDictionary<OffensiveType, int> AssignPlatformSlots(IEnumerable<OffensiveRequest> platformRequests, int numOfPlatformSlots)
        {
            IEnumerable<OffensiveRequest> highFocusRequests = platformRequests.Where(request => request.Focus == OffensiveFocus.High);
            IEnumerable<OffensiveRequest> lowFocusRequest = platformRequests.Where(request => request.Focus == OffensiveFocus.Low);

            IDictionary<OffensiveType, int> requestTypeToSlotNum = new Dictionary<OffensiveType, int>();

            int numOfSlotsUsed = 0;

            // Assign a round of high focus requests
            numOfSlotsUsed = AssignSlotsToRequests(highFocusRequests, requestTypeToSlotNum, numOfPlatformSlots, numOfSlotsUsed);

            // Assign a round of low focus request
            numOfSlotsUsed = AssignSlotsToRequests(lowFocusRequest, requestTypeToSlotNum, numOfPlatformSlots, numOfSlotsUsed);

            // Assign remaining slots to high focus requests
            while (numOfSlotsUsed < numOfPlatformSlots)
            {
                numOfSlotsUsed = AssignSlotsToRequests(highFocusRequests, requestTypeToSlotNum, numOfPlatformSlots, numOfSlotsUsed);
            }

            return requestTypeToSlotNum;
        }

        private int AssignSlotsToRequests(IEnumerable<OffensiveRequest> requests, IDictionary<OffensiveType, int> requestTypeToSlotNum, int numOfPlatformSlots, int numOfSlotsUsed)
        {
            foreach (OffensiveRequest request in requests)
            {
                if (numOfSlotsUsed < numOfPlatformSlots)
                {
                    if (!requestTypeToSlotNum.ContainsKey(request.Type))
                    {
                        requestTypeToSlotNum.Add(request.Type, value: 0);
                    }

                    requestTypeToSlotNum[request.Type]++;
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
