using BattleCruisers.Utils;

namespace BattleCruisers.Data.Models.PrefabKeys.Wrappers
{
    public abstract class BasePrefabKeyWrapper : IPrefabKeyWrapper
    {
        private BuildingKey[] _buildOrder;


        public bool HasKey
        {
            get
            {
                Logging.Log(Tags.AI_BUILD_ORDERS, $"HasKey: {_buildOrder.Length > 0}  Key: {Key}");
                return _buildOrder.Length > 0;
            }
        }

        public BuildingKey Key { get; private set; }

        protected BasePrefabKeyWrapper()
        {
            Key = null;
        }

        public void Initialise(BuildingKey[] offensiveBuildOrder,
                                BuildingKey[] antiAirBuildOrder,
                                BuildingKey[] antiNavalBuildOrder)
        {
            _buildOrder = GetBuildOrder(offensiveBuildOrder, antiAirBuildOrder, antiNavalBuildOrder);
        }

        protected abstract BuildingKey[] GetBuildOrder(BuildingKey[] offensiveBuildOrder,
                                                        BuildingKey[] antiAirBuildOrder,
                                                        BuildingKey[] antiNavalBuildOrder);
    }
}
