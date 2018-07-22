using System;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class PlayerCruiserBuildingFilter : IBroadcastingFilter<IBuilding>
    {
        private readonly ICruiser _playerCruiser;

        #pragma warning disable 67  // Unused event
        public event EventHandler PotentialMatchChange;
        #pragma warning restore 67  // Unused event

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
