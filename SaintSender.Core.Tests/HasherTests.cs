using NUnit.Framework;
using SaintSender.Core.Models;

namespace SaintSender.Core.Tests
{
    [TestFixture]
    class HasherTests
    {
        [Test]
        public void HasherReturnsCorrectHash()
        {
            Assert.AreEqual(Hasher.Hash("password"), "nddrXNRviNxQKFNy/tAn0TVtVH2IpfaXmWKuqXtC4bk=");
        }
    }
}
