using NUnit.Framework;
using SaintSender.Core.Models;
using System;

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
        
        [Test]
        public void EncodingWorksCorrectly()
        {
            Assert.AreEqual("aY+iFC7eLraKwtD2P/2Gv/E77VfMqxzq", Hasher.Encode("MyPasswordString"));
        }

        [Test]
        public void DecodingWorksCorrectly()
        {
            Assert.AreEqual("MyPasswordString", Hasher.Decode("aY+iFC7eLraKwtD2P/2Gv/E77VfMqxzq"));
        }
    }
}
