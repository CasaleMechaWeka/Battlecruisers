using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    public class PvPRocketTarget : PvPTarget
    {
        private Rigidbody2D _rigidBody;
        private IPvPRemovable _parentProjectile;

        public override PvPTargetType TargetType => PvPTargetType.Rocket;
        public override Vector2 Velocity => _rigidBody.velocity;

        private Vector2 _size;
        public override Vector2 Size => _size;

        public void Initialise(ILocTable commonStrings, PvPFaction faction, Rigidbody2D rigidBody, IPvPRemovable parentProjectile)
        {
            Helper.AssertIsNotNull(rigidBody, parentProjectile);

            StaticInitialise(commonStrings);

            Faction = faction;
            _rigidBody = rigidBody;
            _parentProjectile = parentProjectile;

            SpriteRenderer rocketRenderer = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(rocketRenderer);

            _size = rocketRenderer.bounds.size;
        }

        protected override void InternalDestroy()
        {
            _parentProjectile.RemoveFromScene();
        }
    }
}