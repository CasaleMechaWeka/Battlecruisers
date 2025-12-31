namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class UIKey: PrefabKey
    {
        private const string UI_FOLDER_NAME = "UI";

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + UI_FOLDER_NAME + PATH_SEPARATOR;
            }
        }

        public UIKey(string prefabFileName) 
            : base(prefabFileName) { }
    }
}
