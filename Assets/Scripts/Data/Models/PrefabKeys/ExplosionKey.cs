namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class ExplosionKey : EffectKey
    {
        private const string EXPLOSION_FOLDER_NAME = "Explosions";

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + EXPLOSION_FOLDER_NAME + PATH_SEPARATOR;
            }
        }

        public ExplosionKey(string prefabFileName)
            : base(prefabFileName) { }
    }
}
