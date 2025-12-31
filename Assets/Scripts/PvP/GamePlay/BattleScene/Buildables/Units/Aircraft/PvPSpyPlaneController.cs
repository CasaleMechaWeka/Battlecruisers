using BattleCruisers.Movement.Velocity;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPSpyPlaneController : PvPAircraftController, IPvPUnit
    {
        protected async override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            IList<Sprite> aircraftSprites = await SpriteProvider.GetAircraftSpritesAsync(PrefabKeyName.Unit_SpyPlane);
            _spriteChooser = new PvPSpriteChooser(aircraftSprites, this);
        }

        protected override IList<IPatrolPoint> GetPatrolPoints()
        {
            return BCUtils.PvPHelper.ConvertVectorsToPatrolPoints(_aircraftProvider.SpyPlanePatrolPoints(transform.position, cruisingAltitudeInM));
        }
    }
}
