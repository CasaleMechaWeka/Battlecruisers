using System.Collections.Generic;
using BattleCruisers.Tutorial.Highlighting;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Tutorial.Highlighting
{
    public class StaticHighlightableProviderTests
    {
        private IHighlightablesProvider _provider;
        private IHighlightable _highlightable1, _highlightable2;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _highlightable1 = Substitute.For<IHighlightable>();
            _highlightable2 = Substitute.For<IHighlightable>();

            _provider = new StaticHighlightableProvider(_highlightable1, _highlightable2);
        }

        [Test]
        public void FindHighlightables()
        {
            IList<IHighlightable> highlightables = _provider.FindHighlightables();

            Assert.IsTrue(highlightables.Contains(_highlightable1));
            Assert.IsTrue(highlightables.Contains(_highlightable2));
        }
    }
}
