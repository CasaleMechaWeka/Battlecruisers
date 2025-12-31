using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class SpyPlaneController : AircraftController, IUnit
    {
        protected async override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            IList<Sprite> aircraftSprites = await SpriteProvider.GetAircraftSpritesAsync(PrefabKeyName.Unit_SpyPlane);
            _spriteChooser = new SpriteChooser(aircraftSprites, this);
        }

        protected override IList<IPatrolPoint> GetPatrolPoints()
        {
            return Helper.ConvertVectorsToPatrolPoints(_aircraftProvider.SpyPlanePatrolPoints(transform.position, cruisingAltitudeInM));
        }
    }
}
