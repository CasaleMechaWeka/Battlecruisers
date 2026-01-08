using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Loading
{
    public static class HintProvider
    {
        private static List<string> basicHints;
        private static List<string> advancedHints;

        // The level local boosters are unlocked (one of the advanced hints refers
        // to local boosters)
        public const int ADVANCED_HINT_LEVEL_REQUIREMENT = 9;
        private static bool isInitialised = false;

        public static void Initialise()
        {
            basicHints = new List<string>()
            {
                LocTableCache.CommonTable.GetString("Hints/Factories"),
                LocTableCache.CommonTable.GetString("Hints/BuilderAutomaticRepair"),
                LocTableCache.CommonTable.GetString("Hints/DifficultyIsChangeable"),
                LocTableCache.CommonTable.GetString("Hints/HowToChangeCruiser"),
                LocTableCache.CommonTable.GetString("Hints/CheckEnemyCruiser")
            };

            //we need to do this due to Localisation
            string buildingsAreDeleteableBase = LocTableCache.CommonTable.GetString("Hints/BuildingsAreDeletable");
            string deleteButtonText = LocTableCache.CommonTable.GetString("UI/Informator/DemolishButton");
            basicHints.Add(string.Format(buildingsAreDeleteableBase, deleteButtonText));

            advancedHints = new List<string>()
            {
                LocTableCache.CommonTable.GetString("Hints/CruisersAreUnique"),
                LocTableCache.CommonTable.GetString("Hints/ConstructBuildingsSequentially"),
                LocTableCache.CommonTable.GetString("Hints/PauseUnitProduction")
            };

            if (!SystemInfoBC.Instance.IsHandheld)
                advancedHints.Add(LocTableCache.CommonTable.GetString("Hints/Hotkeys"));

            string targetButtonBase = LocTableCache.CommonTable.GetString("Hints/TargetButton");
            string targetButtonText = LocTableCache.CommonTable.GetString("UI/Informator/TargetButton");
            advancedHints.Add(string.Format(targetButtonBase, targetButtonText));
            string buildersButtonBase = LocTableCache.CommonTable.GetString("Hints/BuildersButton");
            string buildersButtonText = LocTableCache.CommonTable.GetString("Common/Builders");
            advancedHints.Add(string.Format(buildersButtonBase, buildersButtonText));

            isInitialised = true;
        }

        public static string GetHint()
        {
            if (!isInitialised)
                Initialise();

            if (DataProvider.GameModel.NumOfLevelsCompleted > ADVANCED_HINT_LEVEL_REQUIREMENT
                && RandomGenerator.NextBool())
            {
                return advancedHints[Random.Range(0, advancedHints.Count)];
            }
            else
            {
                return basicHints[Random.Range(0, basicHints.Count)];
            }
        }
    }
}