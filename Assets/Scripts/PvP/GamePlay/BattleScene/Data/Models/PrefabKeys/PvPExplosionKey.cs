namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys
{
    public class PvPExplosionKey : PvPEffectKey
    {
        private const string EXPLOSION_FOLDER_NAME = "Explosions";

        protected override string PrefabPathPrefix
        {
            get
            {
                return base.PrefabPathPrefix + EXPLOSION_FOLDER_NAME + PATH_SEPARATOR;
            }
        }

        public PvPExplosionKey(string prefabFileName)
            : base(prefabFileName) { }
    }
}
