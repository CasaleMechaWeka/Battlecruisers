using BattleCruisers.Cruisers.Slots;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BattleCruisers.Editor
{
    public class SlotIndexValidatorWindow : EditorWindow
    {
        private Vector2 scrollPos;
        private List<string> validationResults = new List<string>();
        private bool showDetails = true;

        [MenuItem("BattleCruisers/Tools/Slot Index Validator %#&v")]
        static void Init()
        {
            SlotIndexValidatorWindow window = (SlotIndexValidatorWindow)EditorWindow.GetWindow(typeof(SlotIndexValidatorWindow));
            window.titleContent = new GUIContent("Slot Index Validator");
            window.Show();
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("Slot Index Validator", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("🎯 This tool validates and fixes duplicate slot indices in cruiser prefabs.\n\n" +
                                  "• Traverses the hierarchy from top to bottom\n" +
                                  "• Finds slots with duplicate indices\n" +
                                  "• Assigns unique indices starting from 1\n" +
                                  "• Marks prefabs as dirty so changes are saved\n\n" +
                                  "⚠️  Always backup your prefabs before running!",
                                  MessageType.Info);

            EditorGUILayout.Space();

            if (GUILayout.Button("Validate Selected Cruisers", GUILayout.Height(30)))
            {
                ValidateSelectedCruisers();
            }

            if (GUILayout.Button("Validate All Cruisers in Scene", GUILayout.Height(30)))
            {
                ValidateAllCruisersInScene();
            }

            if (GUILayout.Button("Validate All Cruiser Prefabs", GUILayout.Height(30)))
            {
                ValidateAllCruiserPrefabs();
            }

            EditorGUILayout.Space();
            showDetails = EditorGUILayout.Toggle("Show Details", showDetails);

            if (validationResults.Count > 0)
            {
                EditorGUILayout.LabelField("Validation Results:", EditorStyles.boldLabel);

                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));

                foreach (string result in validationResults)
                {
                    if (result.StartsWith("ERROR:"))
                    {
                        EditorGUILayout.LabelField(result, EditorStyles.miniLabel);
                    }
                    else if (result.StartsWith("FIXED:"))
                    {
                        EditorGUILayout.LabelField(result, new GUIStyle(EditorStyles.miniLabel) { normal = { textColor = Color.yellow } });
                    }
                    else if (result.StartsWith("OK:"))
                    {
                        if (showDetails)
                        {
                            EditorGUILayout.LabelField(result, new GUIStyle(EditorStyles.miniLabel) { normal = { textColor = Color.green } });
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField(result, EditorStyles.miniLabel);
                    }
                }

                EditorGUILayout.EndScrollView();
            }

            if (GUILayout.Button("Clear Results"))
            {
                validationResults.Clear();
            }
        }

        private void ValidateSelectedCruisers()
        {
            validationResults.Clear();

            GameObject[] selectedObjects = Selection.gameObjects;
            if (selectedObjects.Length == 0)
            {
                validationResults.Add("❌ ERROR: No objects selected. Please select the main cruiser GameObject.");
                UnityEditor.EditorUtility.DisplayDialog(
                    "Slot Index Validation - Error",
                    "❌ No GameObjects selected.\n\nPlease select one or more cruiser GameObjects in the Hierarchy before running validation.",
                    "OK"
                );
                return;
            }

            foreach (GameObject selectedObj in selectedObjects)
            {
                ValidateCruiserSlots(selectedObj);
            }

            // Show summary popup
            ShowValidationSummary();
        }

        private void ValidateAllCruisersInScene()
        {
            validationResults.Clear();

            // Find all SlotWrapperController components in the scene
            SlotWrapperController[] controllers = FindObjectsOfType<SlotWrapperController>(true);

            if (controllers.Length == 0)
            {
                validationResults.Add("❌ ERROR: No SlotWrapperController components found in scene.");
                UnityEditor.EditorUtility.DisplayDialog(
                    "Slot Index Validation - Error",
                    "❌ No cruisers found in the current scene.\n\nMake sure you have cruiser GameObjects with SlotWrapperController components.",
                    "OK"
                );
                return;
            }

            foreach (SlotWrapperController controller in controllers)
            {
                ValidateCruiserSlots(controller.gameObject);
            }

            // Show summary popup
            ShowValidationSummary();
        }

        private void ValidateAllCruiserPrefabs()
        {
            validationResults.Clear();

            // Find all prefab assets that contain SlotWrapperController
            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
            List<GameObject> cruiserPrefabs = new List<GameObject>();

            foreach (string guid in prefabGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab != null && prefab.GetComponentInChildren<SlotWrapperController>(true) != null)
                {
                    cruiserPrefabs.Add(prefab);
                }
            }

            if (cruiserPrefabs.Count == 0)
            {
                validationResults.Add("❌ ERROR: No cruiser prefabs found with SlotWrapperController.");
                UnityEditor.EditorUtility.DisplayDialog(
                    "Slot Index Validation - Error",
                    "❌ No cruiser prefabs found.\n\nMake sure you have cruiser prefabs with SlotWrapperController components in your Assets folder.",
                    "OK"
                );
                return;
            }

            foreach (GameObject prefab in cruiserPrefabs)
            {
                ValidateCruiserSlots(prefab);
            }

            // Show summary popup
            ShowValidationSummary();
        }

        private void ValidateCruiserSlots(GameObject cruiserRoot)
        {
            if (cruiserRoot == null)
            {
                validationResults.Add("ERROR: Null cruiser object provided.");
                return;
            }

            // Find all slots in this cruiser hierarchy
            Slot[] allSlots = cruiserRoot.GetComponentsInChildren<Slot>(true);

            if (allSlots.Length == 0)
            {
                validationResults.Add($"OK: No slots found in {cruiserRoot.name}");
                return;
            }

            validationResults.Add($"Processing cruiser: {cruiserRoot.name} ({allSlots.Length} slots)");

            // Track used indices
            HashSet<float> usedIndices = new HashSet<float>();
            List<Slot> slotsToFix = new List<Slot>();

            // First pass: identify conflicts
            foreach (Slot slot in allSlots)
            {
                if (usedIndices.Contains(slot.index))
                {
                    slotsToFix.Add(slot);
                }
                else
                {
                    usedIndices.Add(slot.index);
                }
            }

            // Second pass: fix conflicts by finding lowest unused index starting from 1
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

                validationResults.Add($"✅ FIXED: '{slot.gameObject.name}' index {originalIndex} → {newIndex}");

                // Mark prefab as dirty if this is a prefab instance
                if (PrefabUtility.IsPartOfPrefabInstance(slot.gameObject))
                {
                    PrefabUtility.RecordPrefabInstancePropertyModifications(slot);
                }
            }

            // Third pass: final validation
            usedIndices.Clear();
            bool hasDuplicates = false;

            foreach (Slot slot in allSlots)
            {
                if (usedIndices.Contains(slot.index))
                {
                    validationResults.Add($"❌ ERROR: Still duplicate index {slot.index} in {cruiserRoot.name}");
                    hasDuplicates = true;
                }
                else
                {
                    usedIndices.Add(slot.index);
                }
            }

            if (!hasDuplicates && slotsToFix.Count == 0)
            {
                validationResults.Add($"✅ SUCCESS: All {allSlots.Length} slot indices are unique in '{cruiserRoot.name}'");
            }
            else if (!hasDuplicates && slotsToFix.Count > 0)
            {
                validationResults.Add($"🎉 SUCCESS: Fixed {slotsToFix.Count} duplicate indices in '{cruiserRoot.name}'");
            }

            validationResults.Add(""); // Empty line for separation
        }

        private void ShowValidationSummary()
        {
            int errorCount = validationResults.Count(r => r.StartsWith("❌ ERROR:"));
            int successCount = validationResults.Count(r => r.StartsWith("✅ SUCCESS:") || r.StartsWith("🎉 SUCCESS:"));

            if (errorCount > 0)
            {
                UnityEditor.EditorUtility.DisplayDialog(
                    "Slot Index Validation - Issues Found",
                    $"❌ Validation completed with {errorCount} error(s).\n\nCheck the results window for details.",
                    "OK"
                );
            }
            else if (successCount > 0)
            {
                UnityEditor.EditorUtility.DisplayDialog(
                    "Slot Index Validation - SUCCESS!",
                    $"✅ All validations completed successfully!\n\nProcessed {successCount} cruiser(s) with no errors.",
                    "OK"
                );
            }
        }
    }
}
