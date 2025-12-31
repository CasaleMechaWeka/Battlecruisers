namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class ProjectileKey : PrefabKey
    {
        private const string PROJECTILES_FOLDER_NAME = "Projectiles";

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + PROJECTILES_FOLDER_NAME + PATH_SEPARATOR;
            }
        }

        public ProjectileKey(string prefabFileName)
            : base(prefabFileName) { }
    }
}
