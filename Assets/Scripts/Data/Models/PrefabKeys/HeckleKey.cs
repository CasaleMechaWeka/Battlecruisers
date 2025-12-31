using System;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    [Serializable]
    public class HeckleKey : PrefabKey
    {
        private const string HULLS_FOLDER_NAME = "Heckles";

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + HULLS_FOLDER_NAME + PATH_SEPARATOR;
            }
        }

        public HeckleKey(string prefabFileName)
            : base(prefabFileName) { }
    }
}
