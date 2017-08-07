using BattleCruisers.AI.Providers.BuildingKey;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Providers.Strategies.Requests
{
    public class OffensiveRequest : BasicOffensiveRequest, IOffensiveRequest
    {
        public IBuildingKeyProvider BuildingKeyProvider { get; private set; }
        public int NumOfSlotsToUse { get; set; }

        // FELIX  Take BasicOffensiveRequest as arg
        public OffensiveRequest(OffensiveType type, OffensiveFocus focus, IBuildingKeyProvider buildingKeyProvider)
            : base(type, focus)
        {
            Assert.IsNotNull(buildingKeyProvider);

            this.BuildingKeyProvider = buildingKeyProvider;
            this.NumOfSlotsToUse = 0;
        }
    }
}
