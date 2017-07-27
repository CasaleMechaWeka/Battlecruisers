using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.AI.Tasks
{
    public class DeleteBuildingTask : IInternalTask
	{
		private readonly IBuilding _building;

		public event EventHandler Completed;

        public DeleteBuildingTask(IBuilding building)
		{
            _building = building;
		}

		public void Start()
		{
            _building.Destroyed += _building_Destroyed;

            _building.InitiateDelete();
		}

        private void _building_Destroyed(object sender, DestroyedEventArgs e)
        {
            _building.Destroyed -= _building_Destroyed;

			if (Completed != null)
			{
				Completed.Invoke(this, EventArgs.Empty);
			}
        }

        public void Stop()
		{
			// Empty
		}

		public void Resume()
		{
			// Emtpy
		}
	}
}
