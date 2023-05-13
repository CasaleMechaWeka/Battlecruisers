using System;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings
{
    public class PvPBuildingGroupFactory : IPvPBuildingGroupFactory
    {
        public IPvPBuildingGroup CreateBuildingGroup(PvPBuildingCategory category, IList<IPvPBuildableWrapper<IPvPBuilding>> buildings)
        {
            return new PvPBuildingGroup(category, buildings, GetGroupName(category), GetGroupDescription(category));
        }

        private string GetGroupName(PvPBuildingCategory category)
        {
            switch (category)
            {
                case PvPBuildingCategory.Factory:
                    return "Factories";
                case PvPBuildingCategory.Tactical:
                    return "Tactical";
                case PvPBuildingCategory.Defence:
                    return "Defence";
                case PvPBuildingCategory.Offence:
                    return "Offence";
                case PvPBuildingCategory.Ultra:
                    return "Ultras";
                default:
                    throw new ArgumentException();
            }
        }

        private string GetGroupDescription(PvPBuildingCategory category)
        {
            switch (category)
            {
                case PvPBuildingCategory.Factory:
                    return "Buildings that produce units";
                case PvPBuildingCategory.Tactical:
                    return "Specialised buildings";
                case PvPBuildingCategory.Defence:
                    return "Defensive buildings to protect your cruiser";
                case PvPBuildingCategory.Offence:
                    return "Offensive buildings to destroy the enemy cruiser";
                case PvPBuildingCategory.Ultra:
                    return "Ridiculously awesome creations meant to end to game";
                default:
                    throw new ArgumentException();
            }
        }
    }
}
