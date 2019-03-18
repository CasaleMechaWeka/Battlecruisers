using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.ThreatMonitors
{
    /// <summary>
    /// Monitors the number of drones used by all factories of a specific unit type.
    /// 
    /// Evaluates the threat level when:
    /// + A factory is started
    /// + A factory is completed
    /// + The number of drones used by a factory change
    /// </summary>
    public class FactoryThreatMonitor : ImmediateThreatMonitor
    {
		private readonly UnitCategory _threatCategory;
        private readonly IList<IFactory> _factories;

        public FactoryThreatMonitor(ICruiserController enemyCruiser, IThreatEvaluator threatEvaluator, UnitCategory threatCategory)
            : base(enemyCruiser, threatEvaluator)
        {
			_threatCategory = threatCategory;
            _factories = new List<IFactory>();

            _enemyCruiser.BuildingStarted += _enemyCruiser_BuildingStarted;
        }

        private void _enemyCruiser_BuildingStarted(object sender, BuildingStartedEventArgs e)
        {
            IFactory factory = e.Buildable as IFactory;

            if (factory != null 
                && factory.UnitCategory == _threatCategory)
            {
                Assert.IsFalse(_factories.Contains(factory));
                _factories.Add(factory);

                factory.Destroyed += Factory_Destroyed;
                factory.DroneNumChanged += Factory_DroneNumChanged;

				EvaluateThreatLevel();
			}
        }

        private void Factory_Destroyed(object sender, DestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= Factory_Destroyed;

            IFactory destroyedFactory = e.DestroyedTarget.Parse<IFactory>();
            Assert.IsTrue(_factories.Contains(destroyedFactory));

            _factories.Remove(destroyedFactory);
            destroyedFactory.DroneNumChanged -= Factory_DroneNumChanged;

			EvaluateThreatLevel();
		}

        private void Factory_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            EvaluateThreatLevel();
        }

        protected override float FindThreatEvaluationParameter()
        {
            return _factories.Sum(factory => factory.NumOfDrones);
        }
    }
}
