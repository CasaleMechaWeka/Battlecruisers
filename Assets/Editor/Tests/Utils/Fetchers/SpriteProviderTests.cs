using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.UIWrappers;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Utils.Fetchers
{
    public class SpriteProviderTests
    {
        private ISpriteProvider _provider;
        private ISpriteFetcher _fetcher;
        private ISpriteWrapper _slotSprite;
        private IList<ISpriteWrapper> _bomberSprites, _fighterSprites;

        private const int NUM_OF_BOMBER_SPRITES = 8;
        private const int NUM_OF_FIGHTER_SPRITES = 7;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _fetcher = Substitute.For<ISpriteFetcher>();
            _provider = new SpriteProvider(_fetcher);

            _slotSprite = Substitute.For<ISpriteWrapper>();

            _bomberSprites = CreateSprites(NUM_OF_BOMBER_SPRITES);
            _fighterSprites = CreateSprites(NUM_OF_FIGHTER_SPRITES);
        }

        private IList<ISpriteWrapper> CreateSprites(int numOfSprites)
        {
            IList<ISpriteWrapper> sprites = new List<ISpriteWrapper>();

            for (int i = 0; i < numOfSprites; ++i)
            {
                sprites.Add(Substitute.For<ISpriteWrapper>());
            }

            return sprites;
        }

        [Test]
        public void GetSlotSprite()
        {
            SlotType slotType = SlotType.Mast;
            string slotSpritePath = "Sprites/Slots/slot-" + slotType.ToString();

            _fetcher.GetSprite(slotSpritePath).ReturnsForAnyArgs(_slotSprite);
            ISpriteWrapper slotSprite = _provider.GetSlotSprite(slotType);

            Assert.AreSame(_slotSprite, slotSprite);
        }

        [Test]
        public void GetBomberSprites_ReversesList()
        {
            string bomberSpritesPath = "Sprites/Buildables/Units/Aircraft/bomber";
            _fetcher.GetMultiSprites(bomberSpritesPath).Returns(_bomberSprites);

            IList<ISpriteWrapper> expectedSprites = _bomberSprites.Reverse().ToList();
            IList<ISpriteWrapper> bomberSprites = _provider.GetBomberSprites();

            Assert.IsTrue(Enumerable.SequenceEqual(expectedSprites, bomberSprites));
        }

        [Test]
        public void GetBomberSprites_InvalidNumberOfSprites_Throws()
        {
            string bomberSpritesPath = "Sprites/Buildables/Units/Aircraft/bomber";
            _bomberSprites.RemoveAt(0);
            _fetcher.GetMultiSprites(bomberSpritesPath).Returns(_bomberSprites);

            Assert.Throws<UnityAsserts.AssertionException>(() => _provider.GetBomberSprites());
        }

        [Test]
        public void GetFighterSprites_ReversesList()
        {
            string fighterSpritesPath = "Sprites/Buildables/Units/Aircraft/fighter";
            _fetcher.GetMultiSprites(fighterSpritesPath).Returns(_fighterSprites);

            IList<ISpriteWrapper> expectedSprites = _fighterSprites.Reverse().ToList();
            IList<ISpriteWrapper> fighterSprites = _provider.GetFighterSprites();

            Assert.IsTrue(Enumerable.SequenceEqual(expectedSprites, fighterSprites));
        }

        [Test]
        public void GetFighterSprites_InvalidNumberOfSprites_Throws()
        {
            string fighterSpritesPath = "Sprites/Buildables/Units/Aircraft/fighter";
            _fighterSprites.RemoveAt(0);
            _fetcher.GetMultiSprites(fighterSpritesPath).Returns(_fighterSprites);

            Assert.Throws<UnityAsserts.AssertionException>(() => _provider.GetFighterSprites());
        }
    }
}
