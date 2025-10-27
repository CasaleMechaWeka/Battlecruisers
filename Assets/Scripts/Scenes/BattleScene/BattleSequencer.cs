using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings;
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
                    if (buildingAction.Operation == SequencePoint.BuildingAction.BuildingOp.Remove)
                    {
                        foreach(Slot slot in cruiser.SlotWrapperController.Slots)
                            if(slot.Index == buildingAction.SlotID)
                                if (slot.Building.Value != null)
                                    slot.Building.Value.Destroy();
                    }
                    else if (buildingAction.Operation == SequencePoint.BuildingAction.BuildingOp.Add)
                    {
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
        }
    }

    [Serializable]
    public class SequencePoint
    {
        public int DelayMS = 0;
        public Faction Faction;
        public BuildingAction[] BuildingActions;
        public BoostAction[] BoostActions;

        [Serializable]
        public class BuildingAction
        {

            public enum BuildingOp
            {
                Add = 0,
                Remove = 1,
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