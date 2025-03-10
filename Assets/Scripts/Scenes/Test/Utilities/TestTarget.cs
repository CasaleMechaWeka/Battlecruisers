using BattleCruisers.Buildables;
using BattleCruisers.Utils.Localisation;
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

        public void Initialise(ILocTable commonStrings, Faction faction)
        {
            StaticInitialise(commonStrings);

            Faction = faction;

            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(renderer);
            _size = renderer.bounds.size;
        }
    }
}