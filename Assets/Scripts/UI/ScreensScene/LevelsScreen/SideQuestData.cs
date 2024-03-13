using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Sound;
using System;
using UnityEngine;


namespace BattleCruisers.Data
{
    [Serializable]
    public class SideQuestData : ISideQuestData
    {
        [SerializeField]
        private bool _playerTalksFirst;
        public bool PlayerTalksFirst => _playerTalksFirst;

        [SerializeField]
        private IPrefabKey _enemyCaptainExo;
        public IPrefabKey EnemyCaptainExo => _enemyCaptainExo;

        [SerializeField]
        private int _unlockRequirementLevel;
        public int UnlockRequirementLevel => _unlockRequirementLevel;
        [SerializeField]
        private string _unlockRequirementQuestID;
        public string UnlockRequirementQuestID => _unlockRequirementQuestID;
        //public RewardKey StaticPrefabKeys { get; }
        [SerializeField]
        private PrefabKey _hull;
        public PrefabKey Hull => _hull;
        [SerializeField]
        private SoundKeyPair _musicBackgroundKey;
        public SoundKeyPair MusicBackgroundKey => _musicBackgroundKey;
        //public SkyMaterials SkyMaterial { get; }
        [SerializeField]
        private bool _isCompleted;
        public bool IsCompleted => _isCompleted;
        [SerializeField]
        private int _sideLevelNum;
        public int SideLevelNum => _sideLevelNum;

        [SerializeField]
        private string _skyMaterial;
        public string SkyMaterial => _skyMaterial;

        public SideQuestData(
        bool playerTalksFirst,
        IPrefabKey enemyCaptainExo,
        int unlockRequirementLevel,
        string unlockRequirementQuestID,
        PrefabKey hull,
        SoundKeyPair musicBackgroundKey,
        bool isCompleted,
        int sideLevelNum,
        string skyMaterial)
        {
            _playerTalksFirst = playerTalksFirst;
            _enemyCaptainExo = enemyCaptainExo;
            _unlockRequirementLevel = unlockRequirementLevel;
            _unlockRequirementQuestID = unlockRequirementQuestID;
            _hull = hull;
            _musicBackgroundKey = musicBackgroundKey;
            _isCompleted = isCompleted;
            _sideLevelNum = sideLevelNum;
            _skyMaterial = skyMaterial;
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