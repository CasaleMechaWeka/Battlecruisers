using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.ScreensScene.ShopScreen;

namespace BattleCruisers.Data
{
    [System.Serializable]
    public class CruiserPhase
    {
        [Header("Cruiser Configuration")]
        public IPrefabKey hullKey;
        public BodykitData[] bodykits;
        public bool isFinalPhase = false; // If true, battle ends when this phase is defeated

        [Header("Visual & Animation")]
        public int entryAnimationDuration = 10; // Slide-in duration in seconds (default: 10s)

        [Header("Phase Dialog")]
        public List<ChainBattleChat> phaseStartChats = new List<ChainBattleChat>(); // Chats shown when this phase starts

        [Header("Initial Setup")]
        public SequencePoint.BuildingAction[] initialBuildings;
        public SequencePoint.UnitAction[] initialUnits;

        [Header("Phase Bonuses")]
        public Cruiser.BoostStats[] phaseBonuses;

        [Header("Sequence Points")]
        public SequencePoint[] phaseSequencePoints;

        [Header("Reactive Behaviors")]
        public ConditionalAction[] phaseConditionalActions;
    }
}
