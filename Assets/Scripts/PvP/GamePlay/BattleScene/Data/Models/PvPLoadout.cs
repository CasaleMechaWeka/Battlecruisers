using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models
{
    [Serializable]
    public class PvPLoadout : IPvPLoadout
    {
        [SerializeField]
        private PvPHullKey _hull;

        [SerializeField]
        private List<PvPBuildingKey> _buildings;

        [SerializeField]
        private List<PvPUnitKey> _units;

        public PvPHullKey Hull
        {
            get { return _hull; }
            set
            {
                Assert.IsNotNull(value);
                _hull = value;
            }
        }

        public PvPLoadout(
            PvPHullKey hull,
            List<PvPBuildingKey> buildings,
            List<PvPUnitKey> units)
        {
            Hull = hull;
            _buildings = buildings;
            _units = units;
        }

        public IList<PvPBuildingKey> GetBuildings(PvPBuildingCategory buildingCategory)
        {
            return _buildings.Where(buildingKey => buildingKey.BuildingCategory == buildingCategory).ToList();
        }

        public IList<PvPUnitKey> GetUnits(PvPUnitCategory unitCategory)
        {
            return _units.Where(unitKey => unitKey.UnitCategory == unitCategory).ToList();
        }

        public void AddBuilding(PvPBuildingKey buildingToAdd)
        {
            if (!_buildings.Contains(buildingToAdd))
            {
                _buildings.Add(buildingToAdd);
            }
        }

        public void RemoveBuilding(PvPBuildingKey buildingToRemove)
        {
            bool removedSuccessfully = _buildings.Remove(buildingToRemove);
            Assert.IsTrue(removedSuccessfully);
        }

        public void AddUnit(PvPUnitKey unitToAdd)
        {
            if (!_units.Contains(unitToAdd))
            {
                _units.Add(unitToAdd);
            }
        }

        public void RemoveUnit(PvPUnitKey unitToRemove)
        {
            bool removedSuccessfully = _units.Remove(unitToRemove);
            Assert.IsTrue(removedSuccessfully);
        }

        public override bool Equals(object obj)
        {
            PvPLoadout other = obj as PvPLoadout;

            return other != null
                && Hull.SmartEquals(other.Hull)
                && Enumerable.SequenceEqual(_buildings, other._buildings)
                && Enumerable.SequenceEqual(_units, other._units);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(_hull, _buildings, _units);
        }
    }
}