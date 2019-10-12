// FELIX Fix :)
//using BattleCruisers.Utils.Fetchers;
//using BattleCruisers.Utils.PlatformAbstractions.UI;
//using NSubstitute;
//using NUnit.Framework;
//using System.Collections.Generic;
//using System.Linq;
//using UnityAsserts = UnityEngine.Assertions;

//namespace BattleCruisers.Tests.Utils.Fetchers
//{
//    public class SpriteProviderTests
//    {
//        private ISpriteProvider _provider;
//        private ISpriteFetcher _fetcher;
//        private IList<ISpriteWrapper> _bomberSprites, _fighterSprites;

//        private const int NUM_OF_BOMBER_SPRITES = 8;
//        private const int NUM_OF_FIGHTER_SPRITES = 7;

//        [SetUp]
//        public void SetuUp()
//        {
//            UnityAsserts.Assert.raiseExceptions = true;

//            _fetcher = Substitute.For<ISpriteFetcher>();
//            _provider = new SpriteProvider(_fetcher);

//            _bomberSprites = CreateSprites(NUM_OF_BOMBER_SPRITES);
//            _fighterSprites = CreateSprites(NUM_OF_FIGHTER_SPRITES);
//        }

//        private IList<ISpriteWrapper> CreateSprites(int numOfSprites)
//        {
//            IList<ISpriteWrapper> sprites = new List<ISpriteWrapper>();

//            for (int i = 0; i < numOfSprites; ++i)
//            {
//                sprites.Add(Substitute.For<ISpriteWrapper>());
//            }

//            return sprites;
//        }

//        [Test]
//        public void GetBomberSprites_ReversesList()
//        {
//            string bomberSpritesPath = "Sprites/Buildables/Units/Aircraft/bomber";
//            _fetcher.GetMultiSpritesAsync(bomberSpritesPath).Returns(_bomberSprites);

//            IList<ISpriteWrapper> expectedSprites = _bomberSprites.Reverse().ToList();
//            IList<ISpriteWrapper> bomberSprites = _provider.GetBomberSpritesAsync();

//            Assert.IsTrue(Enumerable.SequenceEqual(expectedSprites, bomberSprites));
//        }

//        [Test]
//        public void GetBomberSprites_InvalidNumberOfSprites_Throws()
//        {
//            string bomberSpritesPath = "Sprites/Buildables/Units/Aircraft/bomber";
//            _bomberSprites.RemoveAt(0);
//            _fetcher.GetMultiSpritesAsync(bomberSpritesPath).Returns(_bomberSprites);

//            Assert.Throws<UnityAsserts.AssertionException>(() => _provider.GetBomberSpritesAsync());
//        }

//        [Test]
//        public void GetFighterSprites_ReversesList()
//        {
//            string fighterSpritesPath = "Sprites/Buildables/Units/Aircraft/fighter";
//            _fetcher.GetMultiSpritesAsync(fighterSpritesPath).Returns(_fighterSprites);

//            IList<ISpriteWrapper> expectedSprites = _fighterSprites.Reverse().ToList();
//            IList<ISpriteWrapper> fighterSprites = _provider.GetFighterSpritesAsync();

//            Assert.IsTrue(Enumerable.SequenceEqual(expectedSprites, fighterSprites));
//        }

//        [Test]
//        public void GetFighterSprites_InvalidNumberOfSprites_Throws()
//        {
//            string fighterSpritesPath = "Sprites/Buildables/Units/Aircraft/fighter";
//            _fighterSprites.RemoveAt(0);
//            _fetcher.GetMultiSpritesAsync(fighterSpritesPath).Returns(_fighterSprites);

//            Assert.Throws<UnityAsserts.AssertionException>(() => _provider.GetFighterSpritesAsync());
//        }
//    }
//}
