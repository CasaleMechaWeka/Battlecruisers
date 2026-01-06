using BattleCruisers.Cruisers.Slots;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BattleCruisers.Editor
{
    public static class SlotIndexContextMenu
    {
        [MenuItem("GameObject/BattleCruisers/Validate Slot Indices %&v", false, 20)]
        private static void ValidateSlotIndices()
        {
            Debug.Log("SlotIndexValidator: Context menu activated");
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject == null)
            {
                Debug.LogError("❌ SlotIndexValidator: No GameObject selected. Please select a cruiser GameObject first.");
                return;
            }

            Debug.Log($"SlotIndexValidator: Starting validation on '{selectedObject.name}'");
            ValidateCruiserSlots(selectedObject);
        }

        [MenuItem("GameObject/BattleCruisers/Validate Slot Indices %&v", true)]
        private static bool ValidateSlotIndicesValidation()
        {
            // Only show menu item if selected object has slots
            GameObject selectedObject = Selection.activeGameObject;
            bool hasSlots = selectedObject != null && selectedObject.GetComponentsInChildren<Slot>(true).Length > 0;
            Debug.Log($"SlotIndexValidator: Menu validation - Selected: {selectedObject?.name}, Has slots: {hasSlots}");
            return hasSlots;
        }

        private static void ValidateCruiserSlots(GameObject cruiserRoot)
        {
            // Find all slots in this cruiser hierarchy
            Slot[] allSlots = cruiserRoot.GetComponentsInChildren<Slot>(true);

            if (allSlots.Length == 0)
            {
                Debug.LogWarning($"⚠️ SlotIndexValidator: No Slot components found in '{cruiserRoot.name}'. Make sure this is a cruiser GameObject.");

                // Show warning popup
                UnityEditor.EditorUtility.DisplayDialog(
                    "Slot Index Validation - Warning",
                    $"⚠️ No Slot components found in '{cruiserRoot.name}'.\n\nMake sure you've selected a cruiser GameObject that contains Slot components.",
                    "OK"
                );
                return;
            }

            Debug.Log($"🔍 SlotIndexValidator: Analyzing cruiser '{cruiserRoot.name}' with {allSlots.Length} slots...");

            // Track used indices
            HashSet<float> usedIndices = new HashSet<float>();
            List<Slot> slotsToFix = new List<Slot>();
            Dictionary<float, List<Slot>> conflictsByIndex = new Dictionary<float, List<Slot>>();

            // First pass: identify conflicts and build conflict map
            foreach (Slot slot in allSlots)
            {
                if (usedIndices.Contains(slot.index))
                {
                    slotsToFix.Add(slot);

                    // Track which slots have the same index
                    if (!conflictsByIndex.ContainsKey(slot.index))
                        conflictsByIndex[slot.index] = new List<Slot>();
                    conflictsByIndex[slot.index].Add(slot);
                }
                else
                {
                    usedIndices.Add(slot.index);
                }
            }

            // Report conflicts found
            if (slotsToFix.Count > 0)
            {
                Debug.LogWarning($"⚠️ SlotIndexValidator: Found {slotsToFix.Count} duplicate slot indices in '{cruiserRoot.name}':");

                string conflictDetails = "";
                foreach (var kvp in conflictsByIndex)
                {
                    string slotNames = string.Join(", ", kvp.Value.Select(s => s.gameObject.name));
                    Debug.LogWarning($"   Index {kvp.Key}: {slotNames}");
                    conflictDetails += $"Index {kvp.Key}: {slotNames}\n";
                }

                // Show dialog about conflicts found
                bool proceed = UnityEditor.EditorUtility.DisplayDialog(
                    "Slot Index Validation - Conflicts Found",
                    $"⚠️ Found {slotsToFix.Count} duplicate slot indices in '{cruiserRoot.name}':\n\n{conflictDetails}\nClick OK to automatically fix them, or Cancel to abort.",
                    "Fix Automatically",
                    "Cancel"
                );

                if (!proceed)
                {
                    Debug.Log("SlotIndexValidator: User cancelled the validation process.");
                    return;
                }

                Debug.Log("SlotIndexValidator: Proceeding with automatic fixes...");
            }

            // Second pass: fix conflicts by finding lowest unused index starting from 1
            int fixesApplied = 0;
            foreach (Slot slot in slotsToFix)
            {
                float originalIndex = slot.index;
                float newIndex = 1; // Start from 1, not 0

                // Find lowest unused positive integer starting from 1
                while (usedIndices.Contains(newIndex))
                {
                    newIndex++;
                }

                slot.index = newIndex;
                usedIndices.Add(newIndex);
                fixesApplied++;

                Debug.Log($"✅ SlotIndexValidator: Fixed '{slot.gameObject.name}' - index {originalIndex} → {newIndex}");

                // Mark prefab as dirty if this is a prefab instance
                if (PrefabUtility.IsPartOfPrefabInstance(slot.gameObject))
                {
                    PrefabUtility.RecordPrefabInstancePropertyModifications(slot);
                }
            }

            // Third pass: final validation
            usedIndices.Clear();
            bool hasDuplicates = false;
            List<float> duplicateIndices = new List<float>();

            foreach (Slot slot in allSlots)
            {
                if (usedIndices.Contains(slot.index))
                {
                    hasDuplicates = true;
                    if (!duplicateIndices.Contains(slot.index))
                        duplicateIndices.Add(slot.index);
                }
                else
                {
                    usedIndices.Add(slot.index);
                }
            }

            if (hasDuplicates)
            {
                Debug.LogError($"❌ SlotIndexValidator: FAILED - Still found duplicate indices in '{cruiserRoot.name}': {string.Join(", ", duplicateIndices)}");

                // Show error popup
                UnityEditor.EditorUtility.DisplayDialog(
                    "Slot Index Validation - FAILED!",
                    $"❌ CRITICAL ERROR: Validation failed!\n\nDuplicate indices still exist in '{cruiserRoot.name}':\n{string.Join(", ", duplicateIndices)}\n\nPlease check the console for details and fix manually.",
                    "OK"
                );
            }
            else
            {
                if (fixesApplied > 0)
                {
                    Debug.Log($"🎉 SlotIndexValidator: SUCCESS - Fixed {fixesApplied} duplicate indices in '{cruiserRoot.name}'. All {allSlots.Length} slots now have unique indices!");

                    // Show success popup
                    UnityEditor.EditorUtility.DisplayDialog(
                        "Slot Index Validation - SUCCESS!",
                        $"✅ SUCCESS!\n\nFixed {fixesApplied} duplicate slot indices in '{cruiserRoot.name}'.\n\nAll {allSlots.Length} slots now have unique indices starting from 1.\n\nChanges have been saved to the prefab.",
                        "OK"
                    );
                }
                else
                {
                    Debug.Log($"✅ SlotIndexValidator: SUCCESS - All {allSlots.Length} slot indices in '{cruiserRoot.name}' are already unique and valid!");

                    // Show success popup
                    UnityEditor.EditorUtility.DisplayDialog(
                        "Slot Index Validation - SUCCESS!",
                        $"✅ SUCCESS!\n\nAll {allSlots.Length} slot indices in '{cruiserRoot.name}' are already unique and valid.\n\nNo changes were needed.",
                        "OK"
                    );
                }
            }
        }
    }
}
