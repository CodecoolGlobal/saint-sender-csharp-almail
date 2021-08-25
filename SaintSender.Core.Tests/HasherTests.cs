using NUnit.Framework;
using SaintSender.Core.Models;

namespace SaintSender.Core.Tests
{
    [TestFixture]
    class HasherTests
    {
        private Hasher hasher;
        [SetUp]
        public void SetUp()
        {
            hasher = new Hasher();
        }

        [Test]
        public void HasherReturnsCorrectHash()
        {
            Assert.AreEqual(hasher.Hash("password"), "nddrXNRviNxQKFNy/tAn0TVtVH2IpfaXmWKuqXtC4bk=");
        }

        [TearDown]
        public void TearDown()
        {
            hasher = null;
        }
    }
}
