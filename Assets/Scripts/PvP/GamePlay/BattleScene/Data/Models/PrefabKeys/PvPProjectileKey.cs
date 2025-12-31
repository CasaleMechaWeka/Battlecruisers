namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys
{
    public class PvPProjectileKey : PvPPrefabKey
    {
        private const string PROJECTILES_FOLDER_NAME = "Projectiles";

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + PROJECTILES_FOLDER_NAME + PATH_SEPARATOR;
            }
        }

        public PvPProjectileKey(string prefabFileName)
            : base(prefabFileName) { }
    }
}
