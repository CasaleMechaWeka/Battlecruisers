using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.FactoryManagers
{
    public class MostExpensiveUnitChooser : IUnitChooser
	{
        private readonly IList<IBuildableWrapper<IUnit>> _units;

        public MostExpensiveUnitChooser(IList<IBuildableWrapper<IUnit>> units)
        {
            Assert.IsNotNull(units);
            Assert.IsTrue(units.Count != 0);

            _units = units;
        }

		public IBuildableWrapper<IUnit> ChooseUnit(int numOfDrones)
        {
            return
	            _units
	                .Where(wrapper => wrapper.Buildable.NumOfDronesRequired <= numOfDrones)
	                .OrderByDescending(wrapper => wrapper.Buildable.NumOfDronesRequired)
	                .FirstOrDefault();
        }
	}
}
