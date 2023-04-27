namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys
{
    public class PvPShipDeathKey : PvPPrefabKey
    {
        private const string SHIP_DEATH_PATH = "Buildables/Units/Naval/ShipDeaths";

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + SHIP_DEATH_PATH + PATH_SEPARATOR;
            }
        }

        public PvPShipDeathKey(string prefabFileName)
            : base(prefabFileName) { }
    }
}
