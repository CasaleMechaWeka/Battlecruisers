using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Sound;
using System;

namespace BattleCruisers.Data
{
    [Serializable]
    public class SideQuestData
    {

        public readonly bool PlayerTalksFirst;

        public readonly IPrefabKey EnemyCaptainExo;
        public readonly int UnlockRequirementLevel;
        public readonly int RequiredSideQuestID;
        public readonly PrefabKey Hull;

        public readonly string SkyMaterial;

        public readonly SoundKeyPair MusicBackgroundKey;
        public readonly bool IsCompleted;
        public readonly int SideLevelNum;

        public readonly HeckleConfig HeckleConfig;
        public readonly bool HasSequencer;

        public SideQuestData(
        bool playerTalksFirst,
        IPrefabKey enemyCaptainExo,
        int unlockRequirementLevel,
        int requiredSideQuestID,
        PrefabKey hull,
        SoundKeyPair musicBackgroundKey,
        string skyMaterial,
        bool isCompleted,
        int sideLevelNum,
        HeckleConfig heckleConfig = null,
        bool hasSequencer = false)
        {
            PlayerTalksFirst = playerTalksFirst;
            EnemyCaptainExo = enemyCaptainExo;
            UnlockRequirementLevel = unlockRequirementLevel;
            RequiredSideQuestID = requiredSideQuestID;
            Hull = hull;
            SkyMaterial = skyMaterial;
            MusicBackgroundKey = musicBackgroundKey;
            IsCompleted = isCompleted;
            SideLevelNum = sideLevelNum;
            HeckleConfig = heckleConfig ?? new HeckleConfig();
            HasSequencer = hasSequencer;
        }
        //Set vars, functions, check screenscenegod for trashtalk, pass all the parameters to battlescenegod
        /*Step 1: secure that the button is receiving input
        Step 2: Take it to trashtalkscene, use a random enemy, talk, cruisers, strategy
        Step 3: Add a new buildable as reward (Assets > Resources_Moved > Prefabs > BattleScene > Buildables)
        Step 4: Complete functions in SideQuestManager
        Step 5: Make it easier to make more SideQuestLevels
        */
    }
}