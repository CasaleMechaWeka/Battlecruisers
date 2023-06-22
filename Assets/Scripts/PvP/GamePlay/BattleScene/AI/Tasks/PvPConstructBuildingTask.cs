using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using System;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks
{
    public class PvPConstructBuildingTask : IPvPTask
    {
        private readonly IPvPPrefabKey _buildingToConstruct;
        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IPvPCruiserController _parentCruiser;

        private IPvPBuildable _building;

        public event EventHandler Completed;

        public PvPConstructBuildingTask(IPvPPrefabKey buildingToconstruct, IPvPPrefabFactory prefabFactory, IPvPCruiserController parentCruiser)
        {
            PvPHelper.AssertIsNotNull(buildingToconstruct, prefabFactory, parentCruiser);

            _buildingToConstruct = buildingToconstruct;
            _prefabFactory = prefabFactory;
            _parentCruiser = parentCruiser;
        }

        public async Task<bool> Start()
        {
            IPvPBuildableWrapper<IPvPBuilding> buildingWrapperPrefab = _prefabFactory.GetBuildingWrapperPrefab(_buildingToConstruct);

            bool haveStartedTask = false;

            if (_parentCruiser.IsAlive
                && _parentCruiser.SlotAccessor.IsSlotAvailable(buildingWrapperPrefab.Buildable.SlotSpecification)
                && buildingWrapperPrefab.Buildable.NumOfDronesRequired <= _parentCruiser.DroneManager.NumOfDrones)
            {
                IPvPSlot slot = _parentCruiser.SlotAccessor.GetFreeSlot(buildingWrapperPrefab.Buildable.SlotSpecification);
                Assert.IsNotNull(slot);

                _building = await _parentCruiser.ConstructBuilding(buildingWrapperPrefab.UnityObject, slot);
                _building.CompletedBuildable += _building_CompletedBuildable;
                _building.Destroyed += _building_Destroyed;

                haveStartedTask = true;
            }

            return haveStartedTask;
        }


        public void Stop()
        {
            // Empty
        }

        public void Resume()
        {
            // Emtpy
        }

        private void _building_CompletedBuildable(object sender, EventArgs e)
        {
            TaskCompleted();
        }

        private void _building_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            TaskCompleted();
        }

        private void TaskCompleted()
        {
            _building.CompletedBuildable -= _building_CompletedBuildable;
            _building.Destroyed -= _building_Destroyed;

            EmitCompletedEvent();
        }

        private void EmitCompletedEvent()
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }

        public override string ToString()
        {
            return base.ToString() + "  building to consruct: " + _buildingToConstruct;
        }
    }
}
