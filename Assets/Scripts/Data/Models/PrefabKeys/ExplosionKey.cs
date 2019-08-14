namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class ExplosionKey : PrefabKey
    {
        private const string EFFECTS_FOLDER_NAME = "Effects";
        private const string EXPLOSION_FOLDER_NAME = "Explosions";

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + EFFECTS_FOLDER_NAME + PATH_SEPARATOR + EXPLOSION_FOLDER_NAME + PATH_SEPARATOR;
            }
        }

        public ExplosionKey(string prefabFileName)
            : base(prefabFileName) { }
    }
}
