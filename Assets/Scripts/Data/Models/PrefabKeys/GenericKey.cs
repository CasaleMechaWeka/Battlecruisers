namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class GenericKey : PrefabKey
    {
        private readonly string _prefabPath;

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + _prefabPath + PATH_SEPARATOR;
            }
        }

        public GenericKey(string prefabFileName, string prefabPath)
            : base(prefabFileName) 
        {
            _prefabPath = prefabPath;
        }
    }
}
