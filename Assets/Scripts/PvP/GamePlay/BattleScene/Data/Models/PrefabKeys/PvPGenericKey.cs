namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys
{
    public class PvPGenericKey : PvPPrefabKey
    {
        private readonly string _prefabPath;

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + _prefabPath + PATH_SEPARATOR;
            }
        }

        public PvPGenericKey(string prefabFileName, string prefabPath)
            : base(prefabFileName)
        {
            _prefabPath = prefabPath;
        }
    }
}
