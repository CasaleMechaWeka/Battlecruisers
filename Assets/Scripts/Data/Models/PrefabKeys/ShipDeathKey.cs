namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class ShipDeathKey : PrefabKey
    {
        private const string SHIP_DEATH_PATH = "Buildables/Units/Naval/ShipDeaths";

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + SHIP_DEATH_PATH + PATH_SEPARATOR;
            }
        }

        public ShipDeathKey(string prefabFileName)
            : base(prefabFileName) { }
    }
}
