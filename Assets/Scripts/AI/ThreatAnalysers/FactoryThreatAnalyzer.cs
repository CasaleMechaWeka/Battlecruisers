using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.ThreatAnalysers
{
    public class FactoryThreatAnalyzer : IThreatAnalyser
    {
        private readonly ICruiserController _enemyCruiser;
        private readonly UnitCategory _threatCategory;
        private readonly IList<IFactory> _factories;

        private const int NUM_OF_DRONES_FOR_HIGH_THREAT_LEVEL = 6;

        private ThreatLevel _currentThreatLevel;
        public ThreatLevel CurrentThreatLevel 
        { 
            get { return _currentThreatLevel; }
            private set
            {
                if (_currentThreatLevel != value)
                {
                    _currentThreatLevel = value;

                    if (ThreatLevelChanged != null)
                    {
                        ThreatLevelChanged.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        public event EventHandler ThreatLevelChanged;

        public FactoryThreatAnalyzer(ICruiserController enemyCruiser, UnitCategory threatCategory)
        {
            _enemyCruiser = enemyCruiser;
            _threatCategory = threatCategory;
            _factories = new List<IFactory>();

            _enemyCruiser.StartedConstruction += _enemyCruiser_StartedConstruction;
        }

        private void _enemyCruiser_StartedConstruction(object sender, StartedConstructionEventArgs e)
        {
            IFactory factory = e.Buildable as IFactory;

            if (factory != null 
                && factory.UnitCategory == _threatCategory)
            {
                Assert.IsFalse(_factories.Contains(factory));
                _factories.Add(factory);

                factory.Destroyed += Factory_Destroyed;
                factory.DroneNumChanged += Factory_DroneNumChanged;

				OnThreatLevelChanged();
			}
        }

        private void Factory_Destroyed(object sender, DestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= Factory_Destroyed;

            IFactory destroyedFactory = e.DestroyedTarget as IFactory;
            Assert.IsNotNull(destroyedFactory);
            Assert.IsTrue(_factories.Contains(destroyedFactory));

            _factories.Remove(destroyedFactory);
            destroyedFactory.DroneNumChanged -= Factory_DroneNumChanged;

			OnThreatLevelChanged();
		}

        private void Factory_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            OnThreatLevelChanged();
        }

        private void OnThreatLevelChanged()
        {
			int numOfDronesUsed = FindNumOfDronesUsedByFactories(_factories);
            CurrentThreatLevel = FindThreatLevel(numOfDronesUsed);
        }
        
        private int FindNumOfDronesUsedByFactories(IList<IFactory> factories)
        {
			int numOfDronesUsed = 0;

			foreach (IFactory factory in _factories)
			{
				if (factory.DroneConsumer != null)
				{
					numOfDronesUsed += factory.DroneConsumer.NumOfDrones;
				}
			}

            return numOfDronesUsed;
        }

		private ThreatLevel FindThreatLevel(int numOfDronesUsed)
		{
			if (numOfDronesUsed == 0)
			{
				return ThreatLevel.None;
			}
			else if (numOfDronesUsed < NUM_OF_DRONES_FOR_HIGH_THREAT_LEVEL)
			{
				return ThreatLevel.Low;
			}
			return ThreatLevel.High;
		}
    }
}
