using BattleCruisers.AI.Providers.BuildingKey;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Providers
{
    public class OffensiveRequest : IOffensiveRequest
    {
        public OffensiveType Type { get; private set; }
        public OffensiveFocus Focus { get; private set; }
        public IBuildingKeyProvider BuildingKeyProvider { get; private set; }
        public int NumOfSlotsToUse { get; set; }

        public OffensiveRequest(OffensiveType type, OffensiveFocus focus, IBuildingKeyProvider buildingKeyProvider)
        {
            Assert.IsNotNull(buildingKeyProvider);

            this.Type = type;
            this.Focus = focus;
            this.BuildingKeyProvider = buildingKeyProvider;
            this.NumOfSlotsToUse = 0;
        }
    }
}
