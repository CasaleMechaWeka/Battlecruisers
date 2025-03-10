using BattleCruisers.Data.Static.Strategies.Requests;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.Tests.Data.Static.Strategies.Requests
{
    public class OffensiveRequestsProviderTests
    {
        [Test]
        public void Rush()
        {
            Assert.IsFalse(HasUltrasRequest(OffensiveRequestsProvider.Rush.NoUltras));
            Assert.IsTrue(HasUltrasRequest(OffensiveRequestsProvider.Rush.All));
        }

        [Test]
        public void Balanced()
        {
            Assert.IsFalse(HasUltrasRequest(OffensiveRequestsProvider.Balanced.NoUltras));
            Assert.IsTrue(HasUltrasRequest(OffensiveRequestsProvider.Balanced.All));
        }

        [Test]
        public void Boom()
        {
            Assert.IsFalse(HasUltrasRequest(OffensiveRequestsProvider.Boom.NoUltras));
            Assert.IsTrue(HasUltrasRequest(OffensiveRequestsProvider.Boom.All));
        }

        private bool HasUltrasRequest(IList<IOffensiveRequest[]> requestsList)
        {
            return
                requestsList
                    .Any(requests => requests
                        .Any(request => request.Type == OffensiveType.Ultras));
        }
    }
}