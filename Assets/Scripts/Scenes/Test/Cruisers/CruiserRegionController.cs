using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Cruisers
{
    public class CruiserRegionController : MonoBehaviour
    {
        public Cruiser cruiser;
        public ICruiser Cruiser => cruiser;

        public void StaticInitialise()
        {
            Assert.IsNotNull(cruiser);
        }

        // FELIX  Add buildings for all slots :)
        public void Initialise(Helper helper, BuildingWrapper deckSlotBuilding)
        {
            BCUtils.Helper.AssertIsNotNull(helper, deckSlotBuilding);

            helper.SetupCruiser(cruiser);

            // FELIX  Build deck slot building
            // FELIX  On all slots!
        }
    }
}