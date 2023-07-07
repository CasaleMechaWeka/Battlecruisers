using System.Collections.Generic;
using System.Linq;
using BattleCruisers.AI;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders
{
    public class PvPInfiniteBuildOrder : IPvPDynamicBuildOrder
    {
        private readonly ILevelInfo _levelInfo;
        private readonly IList<PvPBuildingKey> _availableBuildings;

        public PvPBuildingKey Current { get; private set; }

        public PvPInfiniteBuildOrder(
            PvPBuildingCategory buildingCategory,
            ILevelInfo levelInfo,
            IList<PvPBuildingKey> bannedBuildings)
        {
            Assert.IsNotNull(levelInfo);

            _levelInfo = levelInfo;

            _availableBuildings = _levelInfo.GetAvailableBuildings(buildingCategory);
            Assert.IsTrue(_availableBuildings.Count != 0, $"No available buildings for: {buildingCategory}");

            RemoveBuildingsToIgnore(bannedBuildings);
            Assert.IsTrue(_availableBuildings.Count != 0, $"No available buildings for: {buildingCategory}");
        }

        private void RemoveBuildingsToIgnore(IList<PvPBuildingKey> bannedBuildings)
        {
            if (bannedBuildings == null)
            {
                return;
            }

            foreach (PvPBuildingKey bannedBuilding in bannedBuildings)
            {
                _availableBuildings.Remove(bannedBuilding);
            }
        }

        public bool MoveNext()
        {
            Current
                = _availableBuildings
                    .Where(_levelInfo.CanConstructBuilding)
                    .Shuffle()
                    .FirstOrDefault();

            return Current != null;
        }
    }
}
