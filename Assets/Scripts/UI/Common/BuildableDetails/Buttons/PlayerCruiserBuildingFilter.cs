using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    // FELIX  Delete?
    public class PlayerCruiserBuildingFilter : IFilter<IBuilding>
    {
        private readonly ICruiser _playerCruiser;

        public PlayerCruiserBuildingFilter(ICruiser playerCruiser)
        {
            Assert.IsNotNull(playerCruiser);
            _playerCruiser = playerCruiser;
        }

        public bool IsMatch(IBuilding building)
        {
            return ReferenceEquals(_playerCruiser, building.ParentCruiser);
        }
    }
}
