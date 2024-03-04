using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Sound;
using BattleCruisers.AI;

namespace BattleCruisers.Data
{
    public class SideQuestData : ISideQuestData
    {
        public bool PlayerTalksFirst { get; }
        public IPrefabKey EnemyCaptainExo { get; }
        public int UnlockRequirementLevel { get; }
        public string UnlockRequirementQuestID { get; }
        //public RewardKey StaticPrefabKeys { get; }
        //public EnemyHull Hull { get; }
        public SoundKeyPair MusicBackgroundKey { get; }
        //public SkyMaterials SkyMaterial { get; }
        public BackgroundImageStats BackgroundImage { get; }
        public IArtificialIntelligence AIStrategy { get; }
        public bool IsCompleted { get; }
        public int SideLevelNum { get; }

        public SideQuestData(
        bool playerTalksFirst,
        IPrefabKey enemyCaptainExo,
        int unlockRequirementLevel,
        string unlockRequirementQuestID,
        SoundKeyPair musicBackgroundKey,
        BackgroundImageStats backgroundImage,
        IArtificialIntelligence aiStrategy,
        bool isCompleted,
        int sideLevelNum)
        {
            PlayerTalksFirst = playerTalksFirst;
            EnemyCaptainExo = enemyCaptainExo;
            UnlockRequirementLevel = unlockRequirementLevel;
            UnlockRequirementQuestID = unlockRequirementQuestID;
            MusicBackgroundKey = musicBackgroundKey;
            BackgroundImage = backgroundImage;
            AIStrategy = aiStrategy;
            IsCompleted = isCompleted;
            SideLevelNum = sideLevelNum;
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