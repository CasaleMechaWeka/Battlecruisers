namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys
{
    public class PvPBuildableOutlineKey : PvPPrefabKey
    {
        private const string BuildableOutlines_FOLDER_NAME = "BuildableOutlines";

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + BuildableOutlines_FOLDER_NAME + PATH_SEPARATOR;
            }
        }

        public PvPBuildableOutlineKey(string prefabFileName)
            : base(prefabFileName) { }
    }
}