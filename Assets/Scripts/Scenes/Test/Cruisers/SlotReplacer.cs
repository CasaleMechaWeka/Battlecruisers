using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using System;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Cruisers
{
    // NOTE:  Need to comment out Cruiser.Update() to avoid having to setup cruiser :)
    public class SlotReplacer : MonoBehaviour
    {
        public Slot deckSlotPrefab;
        public Slot platformSlotPrefab;
        public Slot utilitySlotPrefab;
        public Slot mastSlotPrefab;
        public Slot bowSlotPrefab;

        void Start()
        {
            // Replace slots
            Slot[] slots = FindObjectsOfType<Slot>();

            foreach (Slot oldSlot in slots)
            {
                // Create new slot
                Slot prefab = FindPrefab(oldSlot.Type);

                Slot newSlot = Instantiate(prefab, oldSlot.transform.parent);
                newSlot.index = oldSlot.index;
                newSlot.buildingFunctionAffinity = oldSlot.buildingFunctionAffinity;
                newSlot.direction = oldSlot.direction;
                newSlot.transform.SetPositionAndRotation(oldSlot.transform.position, oldSlot.transform.rotation);

                // Remove old slot
                DestroyImmediate(oldSlot.gameObject);
            }

            // Save cruisers to new prefabs :)
            Cruiser[] cruisers = FindObjectsOfType<Cruiser>();
            string prefabName = "LegitCruiser";
            int index = 0;
            string prefabFolderPath = "Assets/Resources/Prefabs/BattleScene/Hulls/";

            foreach (Cruiser cruiser in cruisers)
            {
                string uniquePrefabname = prefabFolderPath + prefabName + index + ".prefab";
                index++;

                // TEMP  Cannot build game with this, so uncommented :)
                //PrefabUtility.CreatePrefab(uniquePrefabname, cruiser.gameObject, ReplacePrefabOptions.ReplaceNameBased);
            }
        }

        private Slot FindPrefab(SlotType slotType)
        {
            switch (slotType)
            {
                case SlotType.Bow:
                    return bowSlotPrefab;

                case SlotType.Deck:
                    return deckSlotPrefab;

                case SlotType.Platform:
                    return platformSlotPrefab;

                case SlotType.Mast:
                    return mastSlotPrefab;

                case SlotType.Utility:
                    return utilitySlotPrefab;

                default:
                    throw new ArgumentException();
            }
        }
    }
}