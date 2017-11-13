using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils.UIWrappers;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Fetchers
{
    public class SpriteProviderTests
    {
        private ISpriteProvider _provider;
        private ISpriteFetcher _fetcher;
        private ISpriteWrapper _slotSprite;
        private IList<ISpriteWrapper> _bomberSprites;

        private const int NUM_OF_BOMBER_SPRITES = 8;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _fetcher = Substitute.For<ISpriteFetcher>();
            _provider = new SpriteProvider(_fetcher);
			
            _slotSprite = Substitute.For<ISpriteWrapper>();

            _bomberSprites = new List<ISpriteWrapper>();

            for (int i = 0; i < NUM_OF_BOMBER_SPRITES; ++i)
            {
                _bomberSprites.Add(Substitute.For<ISpriteWrapper>());
            }
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
    }
}
