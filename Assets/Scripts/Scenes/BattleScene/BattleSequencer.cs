using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using static BattleCruisers.Utils.PrefabKeyName;

namespace BattleCruisers.Scenes.BattleScene
{
    public class BattleSequencer : MonoBehaviour
    {
        public Cruiser[] Cruisers;

        public SequencePoint[] sequencePoints;  // -> this is currently assigned in BattleScene!

        public async void StartF()
        {
            if (sequencePoints != null)
                foreach (SequencePoint pt in sequencePoints)
                    ProcessSequencePoint(pt);
        }

        public async void ProcessSequencePoint(SequencePoint sq)
        {
            await Task.Delay(sq.DelayMS);
            Cruiser cruiser = Cruisers[(int)sq.Faction];

            if (sq.BuildingActions != null)
                foreach (SequencePoint.BuildingAction buildingAction in sq.BuildingActions)
                {
                    switch (buildingAction.Operation)
                    {
                        case SequencePoint.BuildingAction.BuildingOp.Destroy:
                            foreach (Slot slot in cruiser.SlotWrapperController.Slots)
                                if (slot.Index == buildingAction.SlotID)
                                    if (slot.Building.Value != null)
                                        slot.Building.Value.Destroy();

                            break;

                        case SequencePoint.BuildingAction.BuildingOp.Add:
                            int prefabID = (int)buildingAction.PrefabKeyName;
                            if (prefabID < (int)Building_AirFactory)
                            {
                                Debug.LogError($"Can't instantiate cruisers through BattleSequencer.\n{buildingAction}");
                                return;
                            }

                            if ((prefabID >= (int)Building_AirFactory)
                             && (prefabID < (int)Unit_Bomber))
                            {
                                IBuildableWrapper<IBuilding> building = PrefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeyHelper.GetPrefabKey<BuildingKey>(buildingAction.PrefabKeyName));
                                IList<Slot> slots = cruiser.SlotAccessor.GetFreeSlots(building.Buildable.SlotSpecification.SlotType);
                                Slot slot = slots[Math.Min(slots.Count - 1, buildingAction.SlotID)];
                                if (slot == null)
                                {
                                    Debug.LogError($"{slot.type} Slot #{buildingAction.SlotID} is already occupied!");
                                    return;
                                }
                                cruiser.ConstructBuilding(building, slot, buildingAction.IgnoreDroneReq, buildingAction.IgnoreBuildTime);
                            }
                            else
                            {
                                Debug.LogError($"Invalid BuildingAction: {buildingAction}");
                                return;
                            }

                            break;
                    }
                }

            if (sq.BoostActions != null)
                foreach (SequencePoint.BoostAction boostAction in sq.BoostActions)
                {
                    if (boostAction.Operation == SequencePoint.BoostAction.BoostOp.Remove)
                        cruiser.RemoveBoost(new Cruiser.BoostStats() { boostType = boostAction.BoostType });
                    else if (boostAction.Operation == SequencePoint.BoostAction.BoostOp.Add)
                    {
                        cruiser.AddBoost(
                            new Cruiser.BoostStats()
                            {
                                boostType = boostAction.BoostType,
                                boostAmount = boostAction.BoostAmount
                            });
                    }
                }

            if (sq.UnitActions != null)
                foreach (SequencePoint.UnitAction unitAction in sq.UnitActions)
                {
                    int prefabID = (int)unitAction.PrefabKeyName;
                    if (prefabID < (int)Building_AirFactory)
                    {
                        Debug.LogError($"Can't instantiate cruisers through BattleSequencer.\n{unitAction}");
                        return;
                    }
                    if (prefabID < (int)Unit_Bomber)
                        continue;

                    if (unitAction.Amount == 1)
                        SpawnUnit(unitAction.PrefabKeyName, unitAction.Postion, cruiser);
                    else
                    {
                        for (int i = 0; i < unitAction.Amount; i++)
                        {
                            float x = UnityEngine.Random.Range(0, unitAction.SpawnArea.x);
                            float y = UnityEngine.Random.Range(0, unitAction.SpawnArea.y);

                            Vector2 pos = new Vector2(x + unitAction.Postion.x, y + unitAction.Postion.y);
                            SpawnUnit(unitAction.PrefabKeyName, pos, cruiser);
                        }
                    }
                }
        }

        public void SpawnUnit(PrefabKeyName prefabKey, Vector2 position, Cruiser cruiser)
        {
            IBuildableWrapper<IUnit> unitWrapper = PrefabFactory.GetUnitWrapperPrefab(StaticPrefabKeyHelper.GetPrefabKey<UnitKey>(prefabKey));
            IUnit unit = PrefabFactory.CreateUnit(unitWrapper);
            unit.Transform.Position = new Vector3(position.x, position.y, 0);
            BuildableActivationArgs buildableActivationArgs = new BuildableActivationArgs(cruiser,
                                                                                          cruiser.CruiserSpecificFactories.EnemyCruiser,
                                                                                          cruiser.CruiserSpecificFactories);
            unit.Activate(buildableActivationArgs);
            int droneNum = unit.NumOfDronesRequired;
            unit.NumOfDronesRequired = 1;
            unit.BuildTimeInS *= droneNum;
            unit.StartConstruction();
            ((Buildable<BuildableActivationArgs>)unit).FinishConstruction();
            unit.NumOfDronesRequired = droneNum;
            unit.BuildTimeInS /= droneNum;
        }
    }

    [Serializable]
    public class SequencePoint
    {
        public int DelayMS = 0;
        public Faction Faction;
        public List<BuildingAction> BuildingActions;
        public List<BoostAction> BoostActions;
        public List<UnitAction> UnitActions;

        [Serializable]
        public class BuildingAction
        {

            public enum BuildingOp
            {
                Add = 0,
                Destroy = 1,
            }

            public BuildingOp Operation;
            public PrefabKeyName PrefabKeyName;
            public byte SlotID = 0;
            public bool IgnoreDroneReq = false;
            public bool IgnoreBuildTime = false;

            public override string ToString()
            {
                string s = "BuildingAction: {\n +"
                 + $"\tPrefabKey: {PrefabKeyName}\n"
                 + $"\tSlot: {SlotID}\n"
                 + $"\tIgnoreDroneReq: {IgnoreDroneReq}\n"
                 + $"\tIgnoreBuildTime: {IgnoreBuildTime}\n"
                 + "}\n";
                return s;
            }
        }

        [Serializable]
        public class BoostAction
        {
            public enum BoostOp
            {
                Add = 0,
                Remove = 1,
            }

            public BoostOp Operation;
            public BoostType BoostType;
            public float BoostAmount;
        }

        [Serializable]
        public class UnitAction
        {
            public PrefabKeyName PrefabKeyName;
            public Vector2 Postion;
            public Vector2 SpawnArea;
            [Min(1)] public byte Amount;
        }

        public override string ToString()
        {
            string s = "Sequence Point: {\n"
             + $"\tDelay: {DelayMS} ms"
             + $"\tFaction: {Faction}";
            if (BuildingActions != null)
                foreach (BuildingAction buildingAction in BuildingActions)
                {
                    s += "\tBuilding Actions: {\n"
                     + $"\t\tPrefabKey: {buildingAction.PrefabKeyName}\n"
                     + $"\t\tSlot: {buildingAction.SlotID}\n"
                     + $"\t\tIgnoreDroneReq: {buildingAction.IgnoreDroneReq}\n"
                     + $"\t\tIgnoreBuildTime: {buildingAction.IgnoreBuildTime}\n"
                     + "\t}\n";
                }
            if (BoostActions != null)
                foreach (BoostAction boostAction in BoostActions)
                {
                    s += "\tBoostActions: {\n"
                     + $"\t\tOperation: {boostAction.Operation}\n"
                     + $"\t\tBoostType: {boostAction.BoostType}\n"
                     + $"\t\tBoostAmount: {boostAction.BoostAmount}\n"
                     + "\t}\n";
                }
            s += "}\n";

            return s;
        }
    }
}