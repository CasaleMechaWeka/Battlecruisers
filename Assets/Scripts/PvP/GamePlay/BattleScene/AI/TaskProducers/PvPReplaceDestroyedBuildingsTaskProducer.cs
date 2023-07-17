using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers
{
    /// <summary>
    /// Keeps track of all bulidings that are destroyed, and creates 
    /// ConstructBuildingTask for them.
    /// 
    /// The NORMAL priority means these tasks will run BEFORE any normal
    /// ConstructBuildingTask run (which are LOW priority).  This makes sense, 
    /// becaues later bulidings in the build order often rely on earlier 
    /// buildings in the build order (mainly drone stations, so later buildings 
    /// can be afforded).
    /// 
    /// Additionally drone stations are given a HIGH priority, becaues otherwise
    /// we can get into the situation where we try to rebuild a buliding we 
    /// can no longer afford (if all rebuild tasks had the same priority):
    /// 
    /// 1. DS
    /// 2. Artillery
    /// 3. DS destroyed, create rebuild task
    /// 4. Start rebuilding DS
    /// 5. Artillery destroyed, create rebuild task
    /// 6. In progress DS is destroyed, create rebuild task(gets slotted in BEHIND rebuild artillery task)
    /// 7. Start rebuilding artillery => Don't have enough drones :/
    /// </summary>
    public class PvPReplaceDestroyedBuildingsTaskProducer : PvPTaskProducer
    {
        private readonly IDictionary<string, PvPBuildingKey> _buildingNamesToKeys;

        private const float TASK_DELAY_IN_S = 1;

        public PvPReplaceDestroyedBuildingsTaskProducer(
            IPvPTaskList tasks,
            IPvPCruiserController cruiser,
            IPvPPrefabFactory prefabFactory,
            IPvPTaskFactory taskFactory,
            IList<PvPBuildingKey> buildingKeys)
            : base(tasks, cruiser, taskFactory, prefabFactory)
        {
            _buildingNamesToKeys = CreateMap(buildingKeys);

            _cruiser.BuildingMonitor.BuildingDestroyed += _cruiser_BuildingDestroyed;
        }

        private IDictionary<string, PvPBuildingKey> CreateMap(IList<PvPBuildingKey> buildingKeys)
        {
            IDictionary<string, PvPBuildingKey> buildingNamesToKeys = new Dictionary<string, PvPBuildingKey>();

            foreach (PvPBuildingKey key in buildingKeys)
            {
                Assert.IsNotNull(key);
                //Debug.Log(key);
                IPvPBuildableWrapper<IPvPBuilding> buildingWrapper = _prefabFactory.GetBuildingWrapperPrefab(key);
                //Debug.Log(buildingWrapper.Buildable.keyName);
                Assert.IsFalse(buildingNamesToKeys.ContainsKey(buildingWrapper.Buildable.keyName));
                buildingNamesToKeys.Add(buildingWrapper.Buildable.keyName, key);
            }

            return buildingNamesToKeys;
        }

        private void _cruiser_BuildingDestroyed(object sender, PvPBuildingDestroyedEventArgs e)
        {
            Assert.IsTrue(_buildingNamesToKeys.ContainsKey(e.DestroyedBuilding.keyName));

            IPvPPrefabKey key = _buildingNamesToKeys[e.DestroyedBuilding.keyName];
            PvPTaskPriority taskPriority = key.Equals(StaticPrefabKeys.Buildings.DroneStation) ? PvPTaskPriority.High : PvPTaskPriority.Normal;
            _tasks.Add(_taskFactory.CreateConstructBuildingTask(taskPriority, key));
        }

        public override void DisposeManagedState()
        {
            _cruiser.BuildingMonitor.BuildingDestroyed -= _cruiser_BuildingDestroyed;
            base.DisposeManagedState();
        }
    }
}
