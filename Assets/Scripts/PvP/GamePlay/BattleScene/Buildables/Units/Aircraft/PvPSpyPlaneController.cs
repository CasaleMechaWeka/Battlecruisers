using BattleCruisers.Movement.Velocity;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPSpyPlaneController : PvPAircraftController, IPvPUnit
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

            _spriteChooser = await _factoryProvider.SpriteChooserFactory.CreateAircraftSpriteChooserAsync(BattleCruisers.Utils.PrefabKeyName.Unit_SpyPlane, this);

        }

        protected override IList<IPatrolPoint> GetPatrolPoints()
        {
            return BCUtils.PvPHelper.ConvertVectorsToPatrolPoints(patrolPoints);
        }
    }
}
