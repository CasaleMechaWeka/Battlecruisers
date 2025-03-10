using BattleCruisers.Movement.Velocity;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class SpyPlaneController : AircraftController, IUnit
    {
        // IList is not picked up by the Unity inspector
        public List<Vector2> patrolPoints;
        public IList<Vector2> PatrolPoints
        {
            get { return patrolPoints; }
            set { patrolPoints = new List<Vector2>(value); }
        }

        private bool _useDummyMovementController = false;
        public bool UseDummyMovementController
        {
            private get { return _useDummyMovementController; }
            set
            {
                _useDummyMovementController = value;

                if (_useDummyMovementController)
                {
                    // Create bogus patrol points so PatrollingMovementController is 
                    // created correctly in AircraftController base class
                    PatrolPoints = new List<Vector2> { new Vector2(0, 0), new Vector2(1, 1) };
                }
            }
        }

        protected async override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            if (UseDummyMovementController)
            {
                ActiveMovementController = DummyMovementController;
            }

            _spriteChooser = await _factoryProvider.SpriteChooserFactory.CreateAircraftSpriteChooserAsync(BCUtils.PrefabKeyName.Unit_SpyPlane, this);
        }

        protected override IList<IPatrolPoint> GetPatrolPoints()
        {
            return BCUtils.Helper.ConvertVectorsToPatrolPoints(patrolPoints);
        }
    }
}
