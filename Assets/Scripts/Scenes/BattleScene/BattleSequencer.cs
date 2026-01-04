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
using BattleCruisers.Utils.Debugging;
using BattleCruisers.Utils.Fetchers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static BattleCruisers.Scenes.BattleScene.BattleSequencer;
using static BattleCruisers.Utils.PrefabKeyName;

namespace BattleCruisers.Scenes.BattleScene
{
    public class BattleSequencer : MonoBehaviour
    {
        [HideInInspector] public Cruiser[] Cruisers;

        [Tooltip("Array of sequence points that define the battle progression. Each point contains timed actions like spawning units, adding buildings, or applying boosts. Assigned in BattleScene.")]
        public SequencePoint[] sequencePoints;
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
                        target.objectReferenceValue = this; // <- auto “self”
                        modified = true;
                    }
                }
            }

            if (modified) so.ApplyModifiedPropertiesWithoutUndo();
        }
#endif

        public async void StartF()
        {
            Debug.Log($"[BattleSequencer] StartF() called. Sequence points: {sequencePoints?.Length ?? 0}");

            if (sequencePoints != null && sequencePoints.Length > 0)
            {
                Debug.Log($"[BattleSequencer] Processing {sequencePoints.Length} sequence point(s)...");
                for (int i = 0; i < sequencePoints.Length; i++)
                {
                    Debug.Log($"[BattleSequencer] Starting sequence point {i + 1}/{sequencePoints.Length} (Delay: {sequencePoints[i].DelayMS}ms)");
                    await ProcessSequencePoint(sequencePoints[i]);
                }
            }
            else
            {
                Debug.LogWarning("[BattleSequencer] No sequence points to process");
            }
        }

        /// <summary>
        /// Process a sequence point immediately, skipping the delay. Useful for triggering from animation events.
        /// </summary>
        /// <param name="sequencePointIndex">Index of the sequence point in the sequencePoints array (0-based)</param>
        public void TriggerSequencePoint(int sequencePointIndex)
        {
            if (sequencePoints == null || sequencePointIndex < 0 || sequencePointIndex >= sequencePoints.Length)
            {
                Debug.LogWarning($"[BattleSequencer] Invalid sequence point index: {sequencePointIndex}. Array length: {sequencePoints?.Length ?? 0}");
                return;
            }

            SequencePoint sq = sequencePoints[sequencePointIndex];
            ProcessSequencePointImmediate(sq);
        }

        /// <summary>
        /// Process a sequence point with its configured delay. Called automatically from StartF().
        /// </summary>
        public async Task ProcessSequencePoint(SequencePoint sq)
        {
            await Task.Delay(sq.DelayMS);
            ProcessSequencePointImmediate(sq);
        }

        /// <summary>
        /// Internal method that executes sequence point actions without delay.
        /// </summary>
        private void ProcessSequencePointImmediate(SequencePoint sq)
        {
            if (Cruisers == null || Cruisers.Length == 0)
            {
                Debug.LogError("[BattleSequencer] Cannot process sequence point: Cruisers array is null or empty");
                return;
            }

            if ((int)sq.Faction >= Cruisers.Length || Cruisers[(int)sq.Faction] == null)
            {
                Debug.LogError($"[BattleSequencer] Cannot process sequence point: Invalid faction {sq.Faction} or cruiser is null");
                return;
            }

            Cruiser cruiser = Cruisers[(int)sq.Faction];
            Debug.Log($"[BattleSequencer] Processing sequence point for {sq.Faction} cruiser. BuildingActions: {sq.BuildingActions?.Count ?? 0}, UnitActions: {sq.UnitActions?.Count ?? 0}, BoostActions: {sq.BoostActions?.Count ?? 0}");

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
                            // Auto-initialize existing buildables already placed in the cruiser hierarchy
                            // instead of spawning new ones
                            IBuilding[] existingBuildings = cruiser.GetComponentsInChildren<IBuilding>();

                            if (existingBuildings.Length == 0)
                            {
                                Debug.LogWarning($"[BattleSequencer] No existing buildings found in cruiser {cruiser.name} hierarchy. Place building prefabs as children of cruiser sections.");
                                break;
                            }

                            // Find the building by type name matching
                            IBuilding targetBuilding = null;
                            foreach (IBuilding b in existingBuildings)
                            {
                                if (b.PrefabName.Contains(buildingAction.PrefabKeyName.ToString()))
                                {
                                    targetBuilding = b;
                                    break;
                                }
                            }

                            if (targetBuilding == null)
                            {
                                Debug.LogWarning($"[BattleSequencer] Could not find building of type {buildingAction.PrefabKeyName} in cruiser hierarchy. Make sure it's placed as a child object.");
                                break;
                            }

                            // Initialize the existing building
                            targetBuilding.Activate(
                                new BuildingActivationArgs(
                                    cruiser,
                                    cruiser.EnemyCruiser,
                                    cruiser.CruiserSpecificFactories,
                                    null, // No slot needed for pre-placed buildings
                                    null));

                            targetBuilding.StartConstruction();
                            ((Buildable<BuildingActivationArgs>)targetBuilding).FinishConstruction();

                            Debug.Log($"[BattleSequencer] Initialized existing building {targetBuilding.Name} on {cruiser.name}");
                            LogBuildingStats(targetBuilding, cruiser);

                            break;
                    }
                }

            if (sq.BoostActions != null)
                foreach (SequencePoint.BoostAction boostAction in sq.BoostActions)
                {
                    if (boostAction.Operation == SequencePoint.BoostAction.BoostOp.Remove)
                    {
                        cruiser.RemoveBoost(new Cruiser.BoostStats() { boostType = boostAction.BoostType });
                        string msg = $"Removed {boostAction.BoostType} boost from {sq.Faction} cruiser";
                        Debug.Log($"[BattleSequencer] {msg}");
                        ShowMessage(msg, BattleSceneMessageDisplay.MessageType.Boost);
                        LogCruiserBoosts(cruiser);
                    }
                    else if(boostAction.Operation == SequencePoint.BoostAction.BoostOp.Replace)
                    {
                        cruiser.RemoveBoost(new Cruiser.BoostStats() { boostType = boostAction.BoostType });
                        cruiser.AddBoost(
                            new Cruiser.BoostStats()
                            {
                                boostType = boostAction.BoostType,
                                boostAmount = boostAction.BoostAmount
                            });
                        string msg = $"Replaced {boostAction.BoostType} boost on {sq.Faction} cruiser: {boostAction.BoostAmount}x";
                        Debug.Log($"[BattleSequencer] {msg}");
                        ShowMessage(msg, BattleSceneMessageDisplay.MessageType.Boost);
                        LogCruiserBoosts(cruiser);
                    }
                    else if (boostAction.Operation == SequencePoint.BoostAction.BoostOp.Add)
                    {
                        cruiser.AddBoost(
                            new Cruiser.BoostStats()
                            {
                                boostType = boostAction.BoostType,
                                boostAmount = boostAction.BoostAmount
                            });
                        string msg = $"Added {boostAction.BoostType} boost to {sq.Faction} cruiser: {boostAction.BoostAmount}x";
                        Debug.Log($"[BattleSequencer] {msg}");
                        ShowMessage(msg, BattleSceneMessageDisplay.MessageType.Boost);
                        LogCruiserBoosts(cruiser);
                    }
                }

            if (sq.UnitActions != null)
                foreach (SequencePoint.UnitAction unitAction in sq.UnitActions)
                {
                    // Auto-initialize existing units already placed in the cruiser hierarchy
                    // instead of spawning new ones
                    IUnit[] existingUnits = cruiser.GetComponentsInChildren<IUnit>();

                    if (existingUnits.Length == 0)
                    {
                        Debug.LogWarning($"[BattleSequencer] No existing units found in cruiser {cruiser.name} hierarchy. Place unit prefabs as children of cruiser sections.");
                        continue;
                    }

                    // Find the unit by type name matching
                    IUnit targetUnit = null;
                    foreach (IUnit u in existingUnits)
                    {
                        if (u.PrefabName.Contains(unitAction.PrefabKeyName.ToString()))
                        {
                            targetUnit = u;
                            break;
                        }
                    }

                    if (targetUnit == null)
                    {
                        Debug.LogWarning($"[BattleSequencer] Could not find unit of type {unitAction.PrefabKeyName} in cruiser hierarchy. Make sure it's placed as a child object.");
                        continue;
                    }

                    // Initialize the existing unit
                    targetUnit.Activate(
                        new BuildableActivationArgs(
                            cruiser,
                            cruiser.EnemyCruiser,
                            cruiser.CruiserSpecificFactories));

                    targetUnit.StartConstruction();
                    ((Buildable<BuildableActivationArgs>)targetUnit).FinishConstruction();

                    Debug.Log($"[BattleSequencer] Initialized existing unit {targetUnit.Name} on {cruiser.name}");
                }
            if (sq.ScriptCallActions != null)
                sq.ScriptCallActions.Invoke();
        }

        public void SpawnUnit(PrefabKeyName prefabKey, Vector2 position, Cruiser cruiser)
        {
            try
            {
                if (cruiser == null)
                {
                    Debug.LogError($"[BattleSequencer] Cannot spawn unit {prefabKey}: cruiser is null");
                    return;
                }

                if (cruiser.CruiserSpecificFactories == null)
                {
                    Debug.LogError($"[BattleSequencer] Cannot spawn unit {prefabKey}: cruiser.CruiserSpecificFactories is null");
                    return;
                }

                IBuildableWrapper<IUnit> unitWrapper = PrefabFactory.GetUnitWrapperPrefab(StaticPrefabKeyHelper.GetPrefabKey<UnitKey>(prefabKey));
                if (unitWrapper == null)
                {
                    Debug.LogError($"[BattleSequencer] Failed to get unit wrapper for {prefabKey}. Check that the prefab key is correct.");
                    return;
                }

                IUnit unit = PrefabFactory.CreateUnit(unitWrapper);
                if (unit == null)
                {
                    Debug.LogError($"[BattleSequencer] Failed to create unit from wrapper for {prefabKey}");
                    return;
                }

                unit.Transform.Position = new Vector3(position.x, position.y, 0);
                BuildableActivationArgs buildableActivationArgs = new BuildableActivationArgs(cruiser,
                                                                                              cruiser.CruiserSpecificFactories.EnemyCruiser,
                                                                                              cruiser.CruiserSpecificFactories);

                int droneNum = unit.NumOfDronesRequired;
                unit.NumOfDronesRequired = 1;
                unit.BuildTimeInS *= droneNum;
                unit.Activate(buildableActivationArgs);
                unit.StartConstruction();
                ((Buildable<BuildableActivationArgs>)unit).FinishConstruction();
                unit.NumOfDronesRequired = droneNum;
                unit.BuildTimeInS /= droneNum;

                Debug.Log($"[BattleSequencer] Spawned {prefabKey} at ({position.x:F1}, {position.y:F1}) for {cruiser.name}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[BattleSequencer] Exception spawning {prefabKey} at {position}: {ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Log detailed building statistics
        /// </summary>
        private void LogBuildingStats(IBuilding building, Cruiser cruiser)
        {
            if (building == null) return;

            Debug.Log($"[BattleSequencer] Building Stats - Name: {building.Name}, " +
                     $"Health: {building.Health}/{building.MaxHealth}, " +
                     $"State: {building.BuildableState}, " +
                     $"Cruiser: {cruiser.name}, " +
                     $"Faction: {cruiser.Faction}");
        }

        /// <summary>
        /// Log all active boosts on a cruiser
        /// </summary>
        private void LogCruiserBoosts(Cruiser cruiser)
        {
            if (cruiser == null || cruiser.Boosts == null)
            {
                Debug.Log($"[BattleSequencer] {cruiser?.name ?? "Unknown"} has no active boosts");
                return;
            }

            if (cruiser.Boosts.Count == 0)
            {
                Debug.Log($"[BattleSequencer] {cruiser.name} has no active boosts");
                ShowMessage($"{cruiser.name}: No active boosts", BattleSceneMessageDisplay.MessageType.Boost);
                return;
            }

            System.Text.StringBuilder boostList = new System.Text.StringBuilder();
            boostList.Append($"{cruiser.name} Active Boosts: ");
            
            foreach (var boost in cruiser.Boosts)
            {
                boostList.Append($"{boost.boostType}={boost.boostAmount}x; ");
            }

            string boostMsg = boostList.ToString().TrimEnd(' ', ';');
            Debug.Log($"[BattleSequencer] {boostMsg}");
            ShowMessage(boostMsg, BattleSceneMessageDisplay.MessageType.Boost);
        }



    [Serializable]
    public class SequencePoint
    {
        [Tooltip("Delay in milliseconds before executing this sequence point's actions after the previous one.")]
        public int DelayMS = 0;

        [Tooltip("Which faction/cruiser this sequence point affects (Blues = Player, Reds = AI).")]
        public Faction Faction;

        [Tooltip("List of building operations to perform (add buildings to slots or destroy existing ones).")]
        public List<BuildingAction> BuildingActions;

        [Tooltip("List of boost operations to perform (add, remove, or replace cruiser-wide boosts).")]
        public List<BoostAction> BoostActions;

        [Tooltip("List of unit spawning operations to perform.")]
        public List<UnitAction> UnitActions;

        [Serializable]
        public class BuildingAction
        {
            public enum BuildingOp
            {
                Add = 0,
                Destroy = 1,
            }

            [Tooltip("Whether to add a new building or destroy an existing one.")]
            public BuildingOp Operation;

            [Tooltip("The prefab key name of the building to add (only used for Add operations).")]
            public PrefabKeyName PrefabKeyName;

            [Tooltip("The slot index where the building should be placed (0-based, only used for Add operations).")]
            public byte SlotID;

            [Tooltip("If true, skips drone requirements for construction (recommended for sequenced battles to avoid assertion errors).")]
            public bool IgnoreDroneReq = true;

            [Tooltip("If true, builds the building instantly without waiting for construction time (recommended for sequenced battles).")]
            public bool IgnoreBuildTime = true;

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

            [Tooltip("Whether to add, remove, or replace the boost.")]
            public BoostOp Operation;

            [Tooltip("The type of boost to modify (e.g., BuildSpeed, FireRate, Health, etc.).")]
            public BoostType BoostType;

            [Tooltip("The boost multiplier value (1.0 = no boost, 2.0 = double effect, etc.). Only used for Add and Replace operations.")]
            public float BoostAmount = 1;
        }

        [Serializable]
        public class UnitAction
        {
            [Tooltip("The prefab key name of the unit to spawn (e.g., Unit_Bomber, Unit_Fighter).")]
            public PrefabKeyName PrefabKeyName;

            [Tooltip("The base/center position where units will spawn. For single units, this is exact spawn location. For multiple units, this is the bottom-left corner of the spawn area.")]
            public Vector2 Postion;

            [Tooltip("The rectangular area (width,height) around the Position where multiple units can spawn randomly. Only used when Amount > 1. Units spawn randomly within the rectangle starting at Position.")]
            public Vector2 SpawnArea;

            [Tooltip("Number of units to spawn. If 1, spawns exactly at Position. If > 1, spawns randomly within the SpawnArea rectangle.")]
            [Min(1)] public byte Amount = 1;

            [Tooltip("Factory building required for this unit type (e.g., AirFactory for aircraft, NavalFactory for ships). Currently not enforced in sequencer.")]
            public PrefabKeyName RequiredFactory;

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

        [Tooltip("Unity events that will be invoked when this sequence point executes. Use for custom scripting or animation triggers.")]
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
}