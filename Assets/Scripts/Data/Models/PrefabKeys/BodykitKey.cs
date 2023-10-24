using System;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    [Serializable]
    public class BodykitKey : PrefabKey
    {
        private const string BODYKITS_FOLDER_NAME = "Bodykits";
        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + BODYKITS_FOLDER_NAME + PATH_SEPARATOR;
            }
        }

        public BodykitKey(string prefabFileName)
            : base(prefabFileName) { }
    }
}

