using System.Collections.Generic;
using BattleCruisers.Data.Static;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class ShipVsShipBalancingTestGod : MultiCameraTestGod<ShipVsShipBalancingTest>
    {
        private IPrefabFactory _prefabFactory;
        private ICircularList<IPrefabKeyPair> _scenarioPrefabKeys;

        protected override void Initialise()
        {
            _prefabFactory = new PrefabFactory(new PrefabFetcher());

            // Have 4 boat units.  Want to see how each fairs against the others.
            IList<IPrefabKeyPair> scenarioPrefabKeys = new List<IPrefabKeyPair>()
            {
                new PrefabKeyPair(StaticPrefabKeys.Units.AttackBoat, StaticPrefabKeys.Units.Frigate),
                new PrefabKeyPair(StaticPrefabKeys.Units.AttackBoat, StaticPrefabKeys.Units.Destroyer),
                new PrefabKeyPair(StaticPrefabKeys.Units.AttackBoat, StaticPrefabKeys.Units.ArchonBattleship),
                new PrefabKeyPair(StaticPrefabKeys.Units.Frigate, StaticPrefabKeys.Units.Destroyer),
                new PrefabKeyPair(StaticPrefabKeys.Units.Frigate, StaticPrefabKeys.Units.ArchonBattleship),
                new PrefabKeyPair(StaticPrefabKeys.Units.Destroyer, StaticPrefabKeys.Units.ArchonBattleship),
            };

            _scenarioPrefabKeys = new CircularList<IPrefabKeyPair>(scenarioPrefabKeys);
        }

        protected override void InitialiseScenario(ShipVsShipBalancingTest scenario)
        {
            IPrefabKeyPair keyPair = _scenarioPrefabKeys.Next();
            scenario.Initialise(_prefabFactory, keyPair.LeftKey, keyPair.RightKey);
        }
    }
}
