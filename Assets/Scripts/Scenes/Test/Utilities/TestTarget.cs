using BattleCruisers.Buildables;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class TestTarget : Target
    {
        public TargetType targetType;
        public override TargetType TargetType => targetType;

        private Vector2 _size;
        public override Vector2 Size => _size;

        public void Initialise(Faction faction)
        {
            StaticInitialise();

            Faction = faction;

            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(renderer);
            _size = renderer.bounds.size;
        }
    }
}