using System;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class NormalBuildingDeleteButtonDecider : IActivenessDecider<IBuilding>
    {
        private readonly ICruiser _playerCruiser;

        #pragma warning disable 67  // Unused event
        public event EventHandler PotentialActivenessChange;
        #pragma warning restore 67  // Unused event

        public NormalBuildingDeleteButtonDecider(ICruiser playerCruiser)
        {
            Assert.IsNotNull(playerCruiser);
            _playerCruiser = playerCruiser;
        }

        public bool ShouldBeEnabled(IBuilding building)
        {
            return ReferenceEquals(_playerCruiser, building.ParentCruiser);
        }
    }
}
