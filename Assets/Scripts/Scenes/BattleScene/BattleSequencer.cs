using System;
using System.Threading.Tasks;
using BattleCruisers.Buildables;
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

            int prefabID = (int)sq.PrefabKeyName;
            if (prefabID < (int)Building_AirFactory)
            {
                Debug.LogError($"Can't instantiate cruisers through BattleSequencer.\n{sq}");
                return;
            }

            if ((prefabID > (int)Building_AirFactory)
             && (prefabID < (int)Unit_Bomber))
            {
                IBuildableWrapper<IBuilding> building = PrefabFactory.GetBuildingWrapperPrefab(StaticPrefabKeyHelper.GetPrefabKey<BuildingKey>(sq.PrefabKeyName));
                Cruiser cruiser = Cruisers[(int)sq.Faction];
                Slot slot = cruiser.SlotAccessor.GetFreeSlots(building.Buildable.SlotSpecification.SlotType)[sq.SlotID];
                cruiser.ConstructBuilding(building, slot, sq.IgnoreDroneReq, sq.IgnoreBuildTime);
            }
            else
            {
                Debug.LogError($"Invalid SequencePoint: {sq}");
                return;
            }
        } 
    }

    [Serializable]
    public class SequencePoint
    {
        public int DelayMS = 0;
        public PrefabKeyName PrefabKeyName;
        public Faction Faction;
        public byte SlotID = 0;
        public bool IgnoreDroneReq = false;
        public bool IgnoreBuildTime = false;
    }
}