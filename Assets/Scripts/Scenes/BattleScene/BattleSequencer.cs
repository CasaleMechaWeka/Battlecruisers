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
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static BattleCruisers.Scenes.BattleScene.BattleSequencer;
using static BattleCruisers.Utils.PrefabKeyName;
using BattleCruisers.UI.BattleScene.Manager;

namespace BattleCruisers.Scenes.BattleScene
{
    public class BattleSequencer : MonoBehaviour
    {
        [HideInInspector] public Cruiser[] Cruisers;

        public SequencePoint[] sequencePoints;  // -> this is currently assigned in BattleScene!
        [Serializable] public class ScriptCallAction : UnityEvent { }

#if UNITY_EDITOR
        // Optional: toggle if you sometimes want to assign other targets
        [SerializeField] bool autoBindEventTargetsToSelf = true;

        void OnValidate()
        {
            if (!autoBindEventTargetsToSelf) return;

            var so = new SerializedObject(this);
            var it = so.GetIterator();
            bool modified = false;

            // Walk every serialized field (including inside nested classes/lists)
            while (it.NextVisible(true))
            {
                if (it.propertyType != SerializedPropertyType.Generic) continue;

                // UnityEvent fields have this structure
                var calls = it.FindPropertyRelative("m_PersistentCalls.m_Calls");
                if (calls == null) continue;

                for (int i = 0; i < calls.arraySize; i++)
                {
                    var call = calls.GetArrayElementAtIndex(i);
                    var target = call.FindPropertyRelative("m_Target");
                    if (target != null && target.objectReferenceValue == null)
                    {
                        target.objectReferenceValue = this; // <- auto "self"
                        modified = true;
                    }
                }
            }

            if (modified) so.ApplyModifiedPropertiesWithoutUndo();
        }
#endif

        public async void StartF()
        {
            if (sequencePoints != null)
                foreach (SequencePoint pt in sequencePoints)
                    await ProcessSequencePoint(pt);
        }

        public async Task ProcessSequencePoint(SequencePoint sq)
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
                    else if(boostAction.Operation == SequencePoint.BoostAction.BoostOp.Replace)
                    {
                        cruiser.RemoveBoost(new Cruiser.BoostStats() { boostType = boostAction.BoostType });
                        cruiser.AddBoost(
                            new Cruiser.BoostStats()
                            {
                                boostType = boostAction.BoostType,
                                boostAmount = boostAction.BoostAmount
                            });
                    }
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
                    {
                        Debug.LogError($"Invalid UnitAction: {unitAction}");
                        continue;
                    }

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
            if (sq.ScriptCallActions != null)
                sq.ScriptCallActions.Invoke();
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

        #region Pre-Placed Buildables Initialization

        /// <summary>
        /// Initialize all pre-placed BuildingWrappers and UnitWrappers in the cruiser hierarchy.
        /// Buildings/units stay as children of wherever you placed them (so they animate with their parent).
        /// Call from ScriptCallActions: InitializePrePlacedBuildables(0) for Blues, (1) for Reds.
        /// </summary>
        public void InitializePrePlacedBuildables(int factionIndex)
        {
            if (Cruisers == null || factionIndex >= Cruisers.Length || Cruisers[factionIndex] == null)
            {
                Debug.LogError($"[BattleSequencer] Cannot initialize pre-placed buildables: invalid faction index {factionIndex}");
                return;
            }
            InitializePrePlacedBuildables(Cruisers[factionIndex]);
        }

        /// <summary>
        /// Initialize all pre-placed BuildingWrappers and UnitWrappers in the cruiser hierarchy.
        /// </summary>
        public void InitializePrePlacedBuildables(Cruiser cruiser)
        {
            InitializePrePlacedBuildings(cruiser);
            InitializePrePlacedUnits(cruiser);
        }

        /// <summary>
        /// Initialize only pre-placed BuildingWrappers in the cruiser hierarchy.
        /// Call from ScriptCallActions: InitializePrePlacedBuildings(0) for Blues, (1) for Reds.
        /// </summary>
        public void InitializePrePlacedBuildings(int factionIndex)
        {
            if (Cruisers == null || factionIndex >= Cruisers.Length || Cruisers[factionIndex] == null)
            {
                Debug.LogError($"[BattleSequencer] Cannot initialize pre-placed buildings: invalid faction index {factionIndex}");
                return;
            }
            InitializePrePlacedBuildings(Cruisers[factionIndex]);
        }

        /// <summary>
        /// Initialize only pre-placed BuildingWrappers in the cruiser hierarchy.
        /// </summary>
        public void InitializePrePlacedBuildings(Cruiser cruiser)
        {
            BuildingWrapper[] wrappers = cruiser.GetComponentsInChildren<BuildingWrapper>(includeInactive: true);
            UIManager uiManager = BattleSceneGod.Instance.uiManager;

            int count = 0;
            foreach (BuildingWrapper wrapper in wrappers)
            {
                // Skip if already initialized
                if (wrapper.Buildable != null && wrapper.Buildable.IsInitialised)
                    continue;

                // Full initialization chain:
                // 1. StaticInitialise - sets up _parent, _healthBar, _clickHandler
                wrapper.StaticInitialise();

                // 2. Initialise - hooks up click handlers, initializes health bar
                wrapper.Buildable.Initialise(uiManager);

                // 3. Activate - sets parent cruiser, enables the gameobject
                wrapper.Buildable.Activate(
                    new BuildingActivationArgs(
                        cruiser,
                        cruiser.EnemyCruiser,
                        cruiser.CruiserSpecificFactories,
                        null,  // No slot - building stays where you placed it
                        null));

                // 4. StartConstruction and FinishConstruction - completes the building instantly
                wrapper.Buildable.StartConstruction();
                ((Buildable<BuildingActivationArgs>)wrapper.Buildable).FinishConstruction();

                count++;
            }

            Debug.Log($"[BattleSequencer] Initialized {count} pre-placed building(s) on {cruiser.name}");
        }

        /// <summary>
        /// Initialize only pre-placed UnitWrappers in the cruiser hierarchy.
        /// Call from ScriptCallActions: InitializePrePlacedUnits(0) for Blues, (1) for Reds.
        /// </summary>
        public void InitializePrePlacedUnits(int factionIndex)
        {
            if (Cruisers == null || factionIndex >= Cruisers.Length || Cruisers[factionIndex] == null)
            {
                Debug.LogError($"[BattleSequencer] Cannot initialize pre-placed units: invalid faction index {factionIndex}");
                return;
            }
            InitializePrePlacedUnits(Cruisers[factionIndex]);
        }

        /// <summary>
        /// Initialize only pre-placed UnitWrappers in the cruiser hierarchy.
        /// </summary>
        public void InitializePrePlacedUnits(Cruiser cruiser)
        {
            UnitWrapper[] wrappers = cruiser.GetComponentsInChildren<UnitWrapper>(includeInactive: true);
            UIManager uiManager = BattleSceneGod.Instance.uiManager;

            int count = 0;
            foreach (UnitWrapper wrapper in wrappers)
            {
                // Skip if already initialized
                if (wrapper.Buildable != null && wrapper.Buildable.IsInitialised)
                    continue;

                // Full initialization chain:
                wrapper.StaticInitialise();
                wrapper.Buildable.Initialise(uiManager);

                wrapper.Buildable.Activate(
                    new BuildableActivationArgs(
                        cruiser,
                        cruiser.EnemyCruiser,
                        cruiser.CruiserSpecificFactories));

                wrapper.Buildable.StartConstruction();
                ((Buildable<BuildableActivationArgs>)wrapper.Buildable).FinishConstruction();

                count++;
            }

            Debug.Log($"[BattleSequencer] Initialized {count} pre-placed unit(s) on {cruiser.name}");
        }

        #endregion
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
            public byte SlotID;
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
                Replace = 2,
            }

            public BoostOp Operation;
            public BoostType BoostType;
            public float BoostAmount = 1;
        }

        [Serializable]
        public class UnitAction
        {
            public PrefabKeyName PrefabKeyName;
            public Vector2 Postion;
            public Vector2 SpawnArea;
            [Min(1)] public byte Amount = 1;

            public override string ToString()
            {
                string s = "UnitAction: {\n +"
                 + $"\tPrefabKey: {PrefabKeyName}\n"
                 + $"\tPostion: {Postion}\n"
                 + $"\tArea: {SpawnArea}\n"
                 + $"\tAmount: {Amount}\n"
                 + "}\n";
                return s;
            }
        }

        public ScriptCallAction ScriptCallActions;

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
            if (UnitActions != null)
                foreach (UnitAction unitAction in UnitActions)
                {
                    s += "\tUnitAction: {\n +"
                     + $"\t\tPrefabKey: {unitAction.PrefabKeyName}\n"
                     + $"\t\tPostion: {unitAction.Postion}\n"
                     + $"\t\tArea: {unitAction.SpawnArea}\n"
                     + $"\t\tAmount: {unitAction.Amount}\n"
                     + "\t}\n";
                }
            s += "}\n";

            return s;
        }
    }
}
