using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.UI.BattleScene
{
	public interface IPrefabOrganiser
    {
        IList<IBuildingGroup> GetBuildingGroups();
        IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> GetUnits();
	}
}