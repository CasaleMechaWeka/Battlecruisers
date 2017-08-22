using BattleCruisers.AI.Providers.BuildingKey;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Providers.Strategies.Requests
{
    // FELIX  Delete base class, remove unused BuildingKeyProvider property :)
	public class OffensiveRequest : BasicOffensiveRequest, IOffensiveRequest
    {
        public IBuildingKeyProvider BuildingKeyProvider { get; private set; }
        public int NumOfSlotsToUse { get; set; }

        public OffensiveRequest(OffensiveType type, OffensiveFocus focus)
            : base(type, focus)
        {
            NumOfSlotsToUse = 0;
        }

        // FELIX  Remove obsolete constructors
        public OffensiveRequest(IBasicOffensiveRequest request, IBuildingKeyProvider buildingKeyprovider)
            : this(request.Type, request.Focus, buildingKeyprovider)
        {
        }

        public OffensiveRequest(OffensiveType type, OffensiveFocus focus, IBuildingKeyProvider buildingKeyProvider)
            : base(type, focus)
        {
            Assert.IsNotNull(buildingKeyProvider);

            this.BuildingKeyProvider = buildingKeyProvider;
            this.NumOfSlotsToUse = 0;
        }
    }
}
