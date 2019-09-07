namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class EffectKey : PrefabKey
    {
        private const string EFFECTS_FOLDER_NAME = "Effects";

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + EFFECTS_FOLDER_NAME + PATH_SEPARATOR;
            }
        }

        public EffectKey(string prefabFileName)
            : base(prefabFileName) { }
    }
}
