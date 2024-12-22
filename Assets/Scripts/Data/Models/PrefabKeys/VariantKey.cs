using System;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    [Serializable]
    public class VariantKey : PrefabKey
    {
        private const string BODYKITS_FOLDER_NAME = "Variants";
        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + BODYKITS_FOLDER_NAME + PATH_SEPARATOR;
            }
        }

        public VariantKey(string prefabFileName)
            : base(prefabFileName) { }
    }
}
