namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys
{
    public class PvPEffectKey : PvPPrefabKey
    {
        private const string EFFECTS_FOLDER_NAME = "Effects";

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + EFFECTS_FOLDER_NAME + PATH_SEPARATOR;
            }
        }

        public PvPEffectKey(string prefabFileName)
            : base(prefabFileName) { }
    }
}
